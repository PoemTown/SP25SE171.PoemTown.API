using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.Reports;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Consumers.AnnouncementConsumers;
using PoemTown.Service.Events.AnnouncementEvents;
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
    private readonly ILogger<CheckPoemPlagiarismConsumer> _logger;
    
    public CheckPoemPlagiarismConsumer(
        IQDrantService qDrantService,
        IPoemService poemService,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork,
        ILogger<CheckPoemPlagiarismConsumer> logger)
    {
        _qDrantService = qDrantService;
        _poemService = poemService;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    
    public async Task Consume(ConsumeContext<CheckPoemPlagiarismEvent> context)
    {
        var message = context.Message;
        var response = await _qDrantService.SearchSimilarPoemEmbeddingPoint(message.UserId, message.PoemContent);

        double averageScore = response.Results
            .Where(p => p.Score > 0.8)
            .Select(p => p.Score)
            .DefaultIfEmpty(0.0)
            .Max();

        // If the score is not greater than 0.9 then return the average score
        if (averageScore == 0.0)
        {
            averageScore = response.Results.Select(p => p.Score).Average();
        }

        bool isPoemPlagiarism = _poemService.IsPoemPlagiarism(averageScore);
        
        _logger.LogInformation($"Poem ID: {message.PoemId}, Average Score: {averageScore}, Is Plagiarism: {isPoemPlagiarism}");
        // If the poem is plagiarism, Create a plagiarism report and send it to the admin, finally suspend the poem
        if (isPoemPlagiarism)
        {
            var poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == message.PoemId);
            
            _logger.LogInformation($"Poem ID: {message.PoemId}, Status: {poem?.Status}");
            // If the poem is not found, return
            if (poem == null)
            {
                throw new Exception("Poem not found");
            }
            
            // Get the list of poems that the current poem is plagiarism from
            IList<SearchPointsResult> plagiarismFromPoems = _poemService.GetListQDrantSearchPoint(response, 3);
            
            // Get the list of existing poem IDs
            var existingPoemIds = await _unitOfWork.GetRepository<Poem>()
                .AsQueryable()
                .Where(p => p.Status == PoemStatus.Posted)
                .Where(p => plagiarismFromPoems.Select(x => Guid.Parse(x.Id)).Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();
            
            // Get the valid plagiarism reports
            var validPlagiarismReports = plagiarismFromPoems
                .Where(p => existingPoemIds.Contains(Guid.Parse(p.Id)))  // Ensure the ID exists
                .Select(p => new PlagiarismPoemReport
                {
                    PlagiarismFromPoemId = Guid.Parse(p.Id),
                    Score = p.Score
                })
                .ToList();
            
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
                PlagiarismPoemReports = validPlagiarismReports
            };
            
            await _unitOfWork.GetRepository<Report>().InsertAsync(report);
            await _unitOfWork.SaveChangesAsync();
            
            // Send announcement to user about plagiarism
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                UserId = poem.UserId,
                Title = "Thông báo từ hệ thống",
                Content =
                    "Bài thơ của bạn bị nghi ngờ là đạo văn, hệ thống đã tự động GỠ BÀI của bạn và đang được xem xét bởi quản trị viên.",
                IsRead = false
            });
        }
        
        // If the poem is not plagiarism, store the poem embedding
        else
        {
            await _qDrantService.StorePoemEmbeddingAsync(message.PoemId, message.UserId, message.PoemContent);
        }
    }
}