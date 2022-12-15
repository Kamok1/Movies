using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.User;

namespace Abstractions;

public interface IUserService
{
    Task<User> GetUserAsync(int? id = default, HttpContext? httpContext = default, string? login = null);
    Task<DtoUser> GetUserDto(int? id = default, HttpContext? httpContext = default, string? login = null);
    Task CreateAsync(UserRegisterRequest reqUser);
    Task ChangeEmailAsync(User reqUser, EmailChange email);
    Task ChangePasswordAsync(User reqUser, PasswordChange passwords);
    Task EditProfileAsync(User user, EditUserProfile editProfile);
    Task DeleteAsync(User user);
}