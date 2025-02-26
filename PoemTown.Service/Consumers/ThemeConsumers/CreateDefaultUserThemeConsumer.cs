using MassTransit;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.ThemeEvents;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Consumers.ThemeConsumers;

public class CreateDefaultUserThemeConsumer : IConsumer<CreateDefaultUserThemeEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IThemeService _themeService;
    public CreateDefaultUserThemeConsumer(IUnitOfWork unitOfWork, IThemeService themeService)
    {
        _unitOfWork = unitOfWork;
        _themeService = themeService;
    }

    public async Task Consume(ConsumeContext<CreateDefaultUserThemeEvent> context)
    {
        var message = context.Message;
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == message.UserId);

        // Check if user is null
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Check if user already has a default theme
        var isUserThemeExist = await _unitOfWork.GetRepository<Theme>()
            .AsQueryable()
            .AnyAsync(p => p.UserId == message.UserId && p.IsDefault == true);
        
        // If user already has a default theme, return
        if (isUserThemeExist)
        {
            return;
        }

        await _themeService.CreateDefaultThemeAndUserTemplate(message.UserId, true);
    }
}