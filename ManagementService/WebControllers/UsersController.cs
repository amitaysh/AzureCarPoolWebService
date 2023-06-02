using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ManagementService.DTO;
using dm = ManagementService.DomainModels;
using AutoMapper;
using ManagementService.Services;

namespace Managements.WebControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieve all users")]
        public async Task<ActionResult<List<User>>> GetUsersAsync()
        {
            var users = _mapper.Map<List<User>>(await _userService.GetAllUsersAsync());
            return Ok(users);
        }

        /// <summary>
        /// Get a user by ID
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Get a user by ID", Description = "Retrieve a user by its ID")]
        public async Task<ActionResult<User>> GetUserByIdAsync(Guid userId)
        {
            var user = _mapper.Map<User>(await _userService.GetUserByIdAsync(userId));
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">The user to add</param>
        [HttpPost]
        [SwaggerOperation(Summary = "Add a new user", Description = "Add a new user to the system")]
        public async Task<User> AddUserAsync(CreateUser user)
        {
            var dmUser = _mapper.Map<dm.User>(user);

            return _mapper.Map<User>(await _userService.AddUserAsync(dmUser));
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="user">The updated user data</param>
        [HttpPut("{userId}")]
        [SwaggerOperation(Summary = "Update a user", Description = "Update an existing user")]
        public async Task<ActionResult<User>> UpdateUserAsync(Guid userId, UpdateUser user)
        {
            try
            {
                return Ok(_mapper.Map<User>(await _userService.UpdateUserAsync(userId, user)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">The ID of the user to delete</param>
        [HttpDelete("{userId}")]
        [SwaggerOperation(Summary = "Delete a user", Description = "Delete a user from the system")]
        public async Task<ActionResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
