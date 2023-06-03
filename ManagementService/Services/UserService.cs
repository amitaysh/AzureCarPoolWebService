using ManagementService.DomainModels;

namespace ManagementService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        // user service ctor
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // add user 
        public async Task<User?> AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);

            return await _userRepository.GetUserByIdAsync(user.Id);
        }

        // delete user
        public async Task DeleteUserAsync(Guid userId)
        {
            var dmUser = await _userRepository.GetUserByIdAsync(userId);
            if (dmUser?.CarId != null)
            {
                throw new Exception("Cannot delete user since a car is assigned to it");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        // get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        // get user by car id
        public async Task<User?> GetUserByCarIdAsync(Guid carId)
        {
            return await _userRepository.GetUserByCarIdAsync(carId);
        }

        // get user by user id
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        // update user
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

        // assign user to car (Starting transaction)
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

        // clear user from car (closing transaction)
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
