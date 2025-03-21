using MassTransit;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.Events.AuthenticationEvents;

namespace PoemTown.Service.Consumers.AuthenticationConsumers;

public class TrackingUserLoginConsumer : IConsumer<TrackingUserLoginEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    public TrackingUserLoginConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Consume(ConsumeContext<TrackingUserLoginEvent> context)
    {
        var message = context.Message;
        
        var now = DateTimeHelper.SystemTimeNow;
        
        var loginTracking = await _unitOfWork.GetRepository<LoginTracking>().FindAsync(p => p.UserId == message.UserId 
            && p.LoginDate.Date == now.Date);

        // If user has logged in today, do nothing
        if (loginTracking != null)
        {
            return;
        }
        
        // If user has not logged in today, create new login tracking
        loginTracking = new LoginTracking()
        {
            UserId = message.UserId,
            LoginDate = now
        };
        
        await _unitOfWork.GetRepository<LoginTracking>().InsertAsync(loginTracking);
        await _unitOfWork.SaveChangesAsync();
    }
}