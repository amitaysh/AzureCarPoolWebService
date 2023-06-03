using ManagementService.DomainModels;
using ManagementService.DTO;
using ManagementService.ServiceBus;

namespace ManagementService.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        private readonly IServiceBusPublisher _serviceBusHandler;

        // assignment service ctor
        public AssignmentService(IUserService userService, ICarService carService, IServiceBusPublisher serviceBusHandler)
        {
            _userService = userService;
            _carService = carService;
            _serviceBusHandler = serviceBusHandler;
        }

        // assign car and user (starting transaction)
        public async Task AssignCarUserAsync(AssignCarUser assignCarUser)
        {
            await _userService.AssignUserToCar(assignCarUser.UserId, assignCarUser.CarId);
            await _carService.AssignCarToUser(assignCarUser.CarId, assignCarUser.UserId);

            await _serviceBusHandler.PublishNewAssignment(assignCarUser);
        }

        // clear assignment of car and user by car id (closing transaction)
        public async Task ClearAssignmentByCarIdAsync(Guid clearByCarId)
        {
            var car = await _carService.GetCarByIdAsync(clearByCarId);
            if (car == null)
                throw new Exception("Could not find car");
            if (car.UserId == null)
                throw new Exception("The car does NOT have a user");

            await _userService.ClearUserAssignment((Guid)car.UserId);
            await _carService.ClearCarAssignment(clearByCarId);
            await PublishCloseTransactionMessage(clearByCarId);
        }

        // clear assignment of car and user by user id (closing transaction)
        public async Task ClearAssignmentByUserIdAsync(Guid clearByUserId)
        {
            var user = await _userService.GetUserByIdAsync(clearByUserId);
            if (user == null)
                throw new Exception("Could not find user");
            if (user.CarId == null)
                throw new Exception("The user does NOT have a car");

            await _userService.ClearUserAssignment(clearByUserId);
            await _carService.ClearCarAssignment((Guid)user.CarId);
            await PublishCloseTransactionMessage((Guid)user.CarId);
        }

        // private method to publish messages to service bus
        private async Task PublishCloseTransactionMessage(Guid carId)
        {
            var closeTransactionMessage = new CloseTransaction
            {
                CarId = carId,
                EndDate = DateTime.Now
            };
            await _serviceBusHandler.PublishCloseTransaction(closeTransactionMessage);
        }
    }
}
