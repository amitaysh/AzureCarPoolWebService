using ManagementService.DomainModels;

namespace ManagementService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);

            return await _userRepository.GetUserByIdAsync(user.Id);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var dmUser = await _userRepository.GetUserByIdAsync(userId);
            if (dmUser?.CarId != null)
            {
                throw new Exception("Cannot delete user since a car is assigned to it");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByCarIdAsync(Guid carId)
        {
            return await _userRepository.GetUserByCarIdAsync(carId);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<User?> UpdateUserAsync(Guid userId, DTO.UpdateUser user)
        {
            var dmUser = await _userRepository.GetUserByIdAsync(userId);
            if (dmUser == null)
            {
                throw new Exception("Could not find user");
            }
            dmUser.Name = user.Name;
            await _userRepository.UpdateUserAsync(dmUser);

            return dmUser;
        }

        public async Task AssignUserToCar(Guid userId, Guid carId)
        {
            var dmUser = await _userRepository.GetUserByIdAsync(userId);
            if (dmUser == null)
                throw new Exception("Could not find user");
            if (dmUser.CarId != null)
                throw new Exception("The user already assigned with a car. Please first clear the assignment and then assign new car.");

            dmUser.CarId = carId;
            await _userRepository.UpdateUserAsync(dmUser);
        }

        public async Task ClearUserAssignment(Guid userId)
        {
            var dmUser = await _userRepository.GetUserByIdAsync(userId);
            if (dmUser == null)
                throw new Exception("Could not find user");
            dmUser.CarId = null;
            await _userRepository.UpdateUserAsync(dmUser);
        }
    }
}
