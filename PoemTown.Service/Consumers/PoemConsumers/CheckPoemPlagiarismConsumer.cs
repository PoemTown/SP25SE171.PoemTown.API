using MassTransit;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.Reports;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Events.PoemEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.PlagiarismDetector.Interfaces;
using PoemTown.Service.PlagiarismDetector.PDModels;

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
        double averageScore = response.Results.Select(p => p.Score).Average();
        bool isPoemPlagiarism = _poemService.IsPoemPlagiarism(averageScore);
        
        
        // If the poem is plagiarism, Create a plagiarism report and send it to the admin, finally suspend the poem
        if (isPoemPlagiarism)
        {
            var poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == message.PoemId);
            
            // If the poem is not found, return
            if (poem == null)
            {
                return;
            }
            
            
            // Get the list of poems that the current poem is plagiarism from
            IList<SearchPointsResult> plagiarismFromPoems = _poemService.GetListQDrantSearchPoint(response, 3);;
            
            // Suspend the poem
            poem.Status = PoemStatus.Pending;
            _unitOfWork.GetRepository<Poem>().Update(poem);
            
            // Send the report to the admin
            var report = new Report
            {
                PoemId = message.PoemId,
                IsSystem = true,
                ReportReason = "Hệ thống tự động phát hiện đạo văn trong bài thơ",
                Status = ReportStatus.Pending,
                PlagiarismScore = averageScore,
                Poem = poem,
                Type = ReportType.Plagiarism,
                PlagiarismPoemReports = plagiarismFromPoems.Select(p => new PlagiarismPoemReport
                {
                    PlagiarismFromPoemId = Guid.Parse(p.Id),
                    Score = p.Score
                }).ToList()
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