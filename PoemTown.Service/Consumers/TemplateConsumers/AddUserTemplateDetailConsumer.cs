using MassTransit;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Events.TemplateEvents;

namespace PoemTown.Service.Consumers.TemplateConsumers;

public class AddUserTemplateDetailConsumer : IConsumer<AddUserTemplateDetailEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddUserTemplateDetailConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<AddUserTemplateDetailEvent> context)
    {
        var message = context.Message;

        // Get all users that have the default master template 
        IEnumerable<User> users = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .Where(ut => ut.UserTemplates.Any(mt => mt.MasterTemplateId == message.MasterTemplateId))
            .ToListAsync();

        // Get all user templates that have the default tag name
        IEnumerable<UserTemplate> userTemplates = await _unitOfWork.GetRepository<UserTemplate>()
            .AsQueryable()
            .Where(ut => ut.TagName == "DEFAULT" && users.Select(u => u.Id).Contains(ut.UserId))
            .ToListAsync();

        // Get all master template details that are selected
        IEnumerable<MasterTemplateDetail> masterTemplateDetails = await _unitOfWork.GetRepository<MasterTemplateDetail>()
            .AsQueryable()
            .Where(mtd => message.MasterTemplateDetailIds.Contains(mtd.Id))
            .ToListAsync();

        // Get all themes that are default and belong to the users
        IEnumerable<Theme> themes = await _unitOfWork.GetRepository<Theme>()
            .AsQueryable()
            .Where(t => users.Select(u => u.Id).Contains(t.UserId) && t.IsDefault == true)
            .ToListAsync();
        
        List<UserTemplateDetail> userTemplateDetails = [];
        List<ThemeUserTemplateDetail> themeUserTemplateDetails = [];
        
        // Add user template details for each user template, and add user template detail for each theme user template details
        foreach (var userTemplate in userTemplates)
        {
            foreach (var masterTemplateDetail in masterTemplateDetails)
            {
                // Create a new user template detail
                UserTemplateDetail userTemplateDetail = new UserTemplateDetail
                {
                    Id = new Guid(),
                    UserTemplateId = userTemplate.Id,
                    ColorCode = masterTemplateDetail.ColorCode,
                    Type = masterTemplateDetail.Type,
                    Image = masterTemplateDetail.Image
                };
                userTemplateDetails.Add(userTemplateDetail);
                
                // Get all themes that belong to the user
                var userThemes = themes.Where(t => t.UserId == userTemplate.UserId);
                
                // Add theme user template details for each theme
                foreach (var userTheme in userThemes)
                {
                    themeUserTemplateDetails.Add(new ThemeUserTemplateDetail()
                    {
                        Theme = userTheme,
                        UserTemplateDetail = userTemplateDetail,
                    });
                }
            }
        }

        // Insert user template details
        if (userTemplateDetails.Count != 0)
        {
            await _unitOfWork.GetRepository<UserTemplateDetail>().InsertRangeAsync(userTemplateDetails);
        }
        
        // Insert ThemeUserTemplateDetail for each user template detail
        if (themeUserTemplateDetails.Count != 0)
        {
            await _unitOfWork.GetRepository<ThemeUserTemplateDetail>().InsertRangeAsync(themeUserTemplateDetails);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}