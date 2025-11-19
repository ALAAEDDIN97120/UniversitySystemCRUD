using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms.AuthForms;
using University.Core.Validations;
using University.Data.Entities.Identity;

namespace University.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(SignInManager<User> signInManager,
              UserManager<User> userManager,
              RoleManager<Role> roleManager,
              ILogger<AuthService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        //============================ Business Logic for Authentication ============================//

        // Business Logic for User Registration
        public async Task<UserDTO> Register(RegisterForm form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            // Check if user already exists
            var userExists = await _userManager.FindByEmailAsync(form.Email);
            if (userExists != null)
                throw new BusinessException("User Already exist with this email");
            
            var user = new User
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                UserName = form.Email,

            };

            var result = await _userManager.CreateAsync(user, form.Password);
            _logger.LogInformation("Role that isn't being returned: {Role}", form.Role);

            if (!result.Succeeded)
            {
                throw new BusinessException(result.Errors
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, x => x.Select(e => e.Description).ToList()));
            }

            if (!await _roleManager.RoleExistsAsync(form.Role))
            {
                var roleResult = await _roleManager.CreateAsync(new Role { Name = form.Role });
                if (!roleResult.Succeeded)
                {
                    _logger.LogError(":x: Failed to create role {Role}. Errors: {Errors}",
                        form.Role, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    throw new BusinessException($"Failed to create role: {form.Role}");
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, form.Role);
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError(":x: Failed to add user to role {Role}. Errors: {Errors}",
                    form.Role, string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                throw new BusinessException($"Failed to add user to role: {form.Role}");
            }

            return new UserDTO()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };       
        }

        // Business Logic for User Login
        public async Task<UserDTO> Login(LoginForm form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(string.Join(", ", validation.Errors));

            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", form.Email);
                throw new BusinessException("Invalid email or password.");
            }

            var user = await _userManager.FindByEmailAsync(form.Email);
            if (user == null)
            {
                _logger.LogError("User not found after successful login attempt for email: {Email}", form.Email);
                throw new BusinessException("User not found.");
            }

            return new UserDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };
        }

    }

    //===============================================================================================//
}

// Interface for AuthService
public interface IAuthService
{
    Task<UserDTO> Login(LoginForm form);
    Task<UserDTO> Register(RegisterForm form);
}
