using PoemTown.Service.BusinessModels.RequestModels.RoleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;

namespace PoemTown.Service.Interfaces;

public interface IRoleService
{
    Task CreateRole(CreateRoleRequest request);
    Task<IEnumerable<GetRoleResponse>> GetRoles();
    Task UpdateRole(UpdateRoleRequest request);
}