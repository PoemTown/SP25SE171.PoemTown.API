using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.Reports;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.PoemEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.PlagiarismDetector.Interfaces;

namespace PoemTown.Service.Consumers.PoemConsumers;

public class CheckPoemPlagiarismConsumer : IConsumer<CheckPoemPlagiarismEvent>
{
    private readonly IQDrantService _qDrantService;
    private readonly IPoemService _poemService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfWork _unitOfWork;
    
    public CheckPoemPlagiarismConsumer(
        IQDrantService qDrantService,
        IPoemService poemService,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork)
    {
        _qDrantService = qDrantService;
        _poemService = poemService;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task Consume(ConsumeContext<CheckPoemPlagiarismEvent> context)
    {
        var message = context.Message;
        var response = await _qDrantService.SearchSimilarPoemEmbeddingPoint(message.UserId, message.PoemContent);

        // Check if the poem is plagiarism
        bool isPoemPlagiarism = _poemService.IsPoemPlagiarism(response.Results.Select(p => p.Score).Average());
        
        
        // If the poem is plagiarism, Create a plagiarism report and send it to the admin, finally suspend the poem
        if (isPoemPlagiarism)
        {
            var poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == message.PoemId);
            
            // If the poem is not found, return
            if (poem == null)
            {
                return;
            }
            
            // Get the poem that the current poem is plagiarism from
            var plagiarismFromPoem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == new Guid(response.Results.First().Id));
            
            // If the plagiarism from poem is not found, return
            if(plagiarismFromPoem == null)
            {
                return;
            }
            
            // Suspend the poem
            poem.Status = PoemStatus.Suspended;
            _unitOfWork.GetRepository<Poem>().Update(poem);
            
            // Send the report to the admin
            var report = new Report
            {
                PoemId = message.PoemId,
                IsSystem = true,
                ReportReason = "Hệ thống tự động phát hiện đạo văn trong bài thơ",
                Status = ReportStatus.Pending,
                Poem = poem,
                PlagiarismFromPoem = plagiarismFromPoem
            };
            
            await _unitOfWork.GetRepository<Report>().InsertAsync(report);
            await _unitOfWork.SaveChangesAsync();
        }
        
        // If the poem is not plagiarism, store the poem embedding
        else
        {
            await _qDrantService.StorePoemEmbeddingAsync(message.PoemId, message.UserId, message.PoemContent);
        }
    }
}