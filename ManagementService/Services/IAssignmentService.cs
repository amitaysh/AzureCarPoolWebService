using ManagementService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ManagementService.Services
{
    public interface IAssignmentService
    {
        Task AssignCarUserAsync(AssignCarUser assignCarUser);
        Task ClearAssignmentByCarIdAsync(Guid clearByCarId);
        Task ClearAssignmentByUserIdAsync(Guid clearByUserId);
    }
}
