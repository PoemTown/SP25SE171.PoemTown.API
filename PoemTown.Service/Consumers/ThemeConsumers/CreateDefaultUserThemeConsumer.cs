using MassTransit;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.ThemeEvents;

namespace PoemTown.Service.Consumers.ThemeConsumers;

public class CreateDefaultUserThemeConsumer : IConsumer<CreateDefaultUserThemeEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDefaultUserThemeConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<CreateDefaultUserThemeEvent> context)
    {
        var message = context.Message;
        var user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == message.UserId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var isUserThemeExist = await _unitOfWork.GetRepository<Theme>()
            .AsQueryable()
            .AnyAsync(p => p.UserId == message.UserId);

        if (isUserThemeExist)
        {
            return;
        }

        Theme theme = new Theme()
        {
            UserId = message.UserId,
            Name = "Theme mặc định",
            IsDefault = true,
            IsInUse = true,
        };

        await _unitOfWork.GetRepository<Theme>().InsertAsync(theme);
        await _unitOfWork.SaveChangesAsync();
    }
}