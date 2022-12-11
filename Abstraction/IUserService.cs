using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.User;

namespace Abstractions;

public interface IUserService
{
    Task<User> GetUser(int id);
    //todo lepsze zwaracanie
    Task<User> GetUser(HttpContext httpContext);
    IQueryable<User> GetUsersQuery(int? id = default, HttpContext? httpContext = default, string? login = null,
        int? roleId = default, string? displayName = null,
        bool includeRole = true, bool includeFavMovies = false, bool includeReviews = false, bool asNoTracking = false);
    Task<User?> CreateAsync(UserRegisterRequest reqUser);
    Task<bool> ChangeEmail(User reqUser, EmailChange email);
    Task<bool> ChangePassword(User reqUser, PasswordChange passwords);
    Task<User?> EditProfile(User user, EditUserProfile editProfile);
    Task<bool> DeleteAsync(User user);
}