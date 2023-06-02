using ManagementService.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace ManagementService.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByCarIdAsync(Guid carId);
        Task<User?> AddUserAsync(User user);
        Task<User?> UpdateUserAsync(Guid userId, DTO.UpdateUser user);
        Task DeleteUserAsync(Guid userId);
        Task AssignUserToCar(Guid userId, Guid carId);
        Task ClearUserAssignment(Guid userId);
    }
}
