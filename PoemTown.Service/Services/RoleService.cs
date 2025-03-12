using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.RoleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateRole(CreateRoleRequest request)
    {
        var role = await _unitOfWork.GetRepository<Role>()
            .FindAsync(p => p.Name!.ToLower().Trim() == request.Name.ToLower().Trim());
        
        // Check if role already exists
        if(role != null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role already exists");
        }

        role = new Role()
        {
            Name = StringHelper.CapitalizeString(request.Name),
            NormalizedName = StringHelper.NormalizeString(request.Name),
        };
        await _unitOfWork.GetRepository<Role>().InsertAsync(role);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetRoleResponse>> GetRoles()
    {
        var roles = await _unitOfWork.GetRepository<Role>().AsQueryable().ToListAsync();
        return roles.Select(p => new GetRoleResponse()
        {
            Id = p.Id,
            Name = p.Name,
        }).ToList();
    }

    public async Task UpdateRole(UpdateRoleRequest request)
    {
        var role = await _unitOfWork.GetRepository<Role>().FindAsync(p => p.Id == request.Id);
        
        // Check if role exists
        if (role == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }
        
        role.Name = StringHelper.CapitalizeString(request.Name);
        role.NormalizedName = StringHelper.NormalizeString(request.Name);
        
        _unitOfWork.GetRepository<Role>().Update(role);
        await _unitOfWork.SaveChangesAsync();
    }
}