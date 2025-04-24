using MassTransit;
using PoemTown.Service.Events.PoemEvents;
using PoemTown.Service.PlagiarismDetector.Interfaces;

namespace PoemTown.Service.Consumers.PoemConsumers;

public class DeletePoemPointInQDrantConsumer : IConsumer<DeletePoemPointInQDrantEvent>
{
    private IQDrantService _qDrantService;

    public DeletePoemPointInQDrantConsumer(IQDrantService qDrantService)
    {
        _qDrantService = qDrantService;
    }

    public async Task Consume(ConsumeContext<DeletePoemPointInQDrantEvent> context)
    {
        var message = context.Message;
        if (message.PoemIds == null || message.PoemIds.Count <= 0)
        {
            return;
        }

        await _qDrantService.DeletePoemEmbeddingPoint(message.PoemIds);
    }
}