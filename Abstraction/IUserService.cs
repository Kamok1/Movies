using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.Movie;
using Models.User;

namespace Abstractions;

public interface IUserService
{
    Task<User> GetUserAsync(int? id = default, HttpContext? httpContext = default, string? login = null, string? refreshToken = null);
    Task<DtoUser> GetUserDto(int? id = default, HttpContext? httpContext = default, string? login = null);
    Task AddUserMovieAsync(User user, int movieId);
    Task DeleteFromUserMovies(User user, int movieId);
    Task<List<DtoMovie>> GetUserMoviesAsync(int id);
    bool IsMovieInFavorites(User user, int movieId);
    void RemoveUserRefreshToken(User user);
    Task CreateAsync(UserRegisterRequest reqUser);
    Task ChangeEmailAsync(User reqUser, EmailChange email);
    Task ChangePasswordAsync(User reqUser, PasswordChange passwords);
    Task EditProfileAsync(User user, EditUserProfile editProfile);
    Task DeleteAsync(User user);
}