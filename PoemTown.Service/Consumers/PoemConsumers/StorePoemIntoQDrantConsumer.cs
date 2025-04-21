using MassTransit;
using PoemTown.Service.Events.PoemEvents;
using PoemTown.Service.PlagiarismDetector.Interfaces;

namespace PoemTown.Service.Consumers.PoemConsumers;

public class StorePoemIntoQDrantConsumer : IConsumer<StorePoemIntoQDrantEvent>
{
    private readonly IQDrantService _qDrantService;
    public StorePoemIntoQDrantConsumer(IQDrantService qDrantService)
    {
        _qDrantService = qDrantService;
    }
    
    public async Task Consume(ConsumeContext<StorePoemIntoQDrantEvent> context)
    {
        var message = context.Message;
        
        // Store the poem embedding into QDrant
        await _qDrantService.StorePoemEmbeddingAsync(message.PoemId, message.PoetId, message.PoemText, message.IsFamousPoem);
    }
}