using Abstractions;
using Data;
using Data.Models;
using EfExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Movie;
using Models.User;
using static Extensions.PasswordServices;

namespace Implementations;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<DtoUser> GetUserDto(int? id = default, HttpContext? httpContext = default, string? login = null)
    {
        return new DtoUser(await GetUserAsync(id, httpContext, login));
    }

    public async Task CreateAsync(UserRegisterRequest reqUser)
    {
        if (_db.User.Any(x => x.Login == reqUser.Name))
            throw new AddingException<User>("That username is already in use");
        if (_db.User.Any(x => x.Email == reqUser.Email))
          throw new AddingException<User>("That e-mail is already in use");
        var passwordSalt = GenerateSalt(64);
        var password = GenerateSaltedHash(passwordSalt, reqUser.Password);
        var role = await _db.Role.Where(x => x.Name == "User").FirstOrDefaultAsync();

        if (role == null)
            throw new AddingException<User>();

        var newUser = new User
        {
            Email = reqUser.Email,
            Login = reqUser.Name,
            Password = password,
            PasswordSalt = passwordSalt,
            Role = role,
            Description = "",
            DisplayName = reqUser.Name
        };

        _db.User.Add(newUser);
        if (await _db.SaveChangesAsync() == 0)
            throw new AddingException<User>();
    }
    public async Task ChangePasswordAsync(User user, PasswordChange passwords)
    {
        if (VerifyPassword(passwords.OldPassword, user.Password, user.PasswordSalt) == false)
            throw new AuthorizationException();

        var passwordSalt = GenerateSalt(64);
        user.Password = GenerateSaltedHash(passwordSalt, passwords.NewPassword);
        user.PasswordSalt = passwordSalt;
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<User>();
    }
    public async Task EditProfileAsync(User user, EditUserProfile editProfile)
    {
        user.DisplayName = editProfile.DisplayName;
        user.Description = editProfile.Description;
        _db.User.Update(user);
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<User>();
    }

    public async Task<List<DtoMovie>> GetUserMoviesAsync(int id)
    {
      var movies = _db.Movie.Include(movie => movie.UsersFavorite)
        .Where(movie => movie.UsersFavorite.Any(user => user.Id == id));
      return await movies.Select(movie => new DtoMovie(movie)).ToListAsync();
    }
  public bool IsMovieInFavorites(User user, int movieId)
    {
      return user.UserFavouriteMovies.Any(movie => movie.Id == movieId);
    }
    public async Task AddUserMovieAsync(User user, int movieId)
    {
    await _db.Entry(user).Collection(user => user.UserFavouriteMovies).LoadAsync();
    if (user.UserFavouriteMovies.Any(movie => movie.Id == movieId))
        return;

      var movie = await _db.Movie.FindOrThrowErrorAsync(movieId);
      user.UserFavouriteMovies.Add(movie);
      _db.User.Update(user);
      if (await _db.SaveChangesAsync() == 0)
        throw new AddingException<Movie>();
    }
    public async Task DeleteFromUserMovies(User user, int movieId)
    {
      await _db.Entry(user).Collection(user => user.UserFavouriteMovies).LoadAsync();
      if (user.UserFavouriteMovies.Any(x => x.Id != movieId))
        return;

      var movie = await _db.Movie.FindOrThrowErrorAsync(movieId);
      user.UserFavouriteMovies.Remove(movie);
      _db.User.Update(user);
      if (await _db.SaveChangesAsync() == 0)
        throw new DeletingException<Movie>();
    }
  public async Task DeleteAsync(User reqUser)
    {
        _db.User.Remove(reqUser);
        if (await _db.SaveChangesAsync() == 0)
            throw new DeletingException<User>();
    }
    public Task ChangeEmailAsync(User reqUser, EmailChange email)
    {
        throw new EditingException<User>();
    }
    public async Task<User> GetUserAsync(int? id = default, HttpContext? httpContext = default, string? login = null, UserRequestRefreshToken? refreshToken = null)
    {
        var query = _db.User.AsQueryable();
        if (id.IsPositive())
            query = query.Where(user => user.Id == id);
        else if (int.TryParse(httpContext?.Items["UserId"]?.ToString(), out var userId))
            query = query.Where(user => user.Id == userId);
        else if (!string.IsNullOrEmpty(login))
          query = query.Where(user => user.Login == login || user.Email == login);
        else if (refreshToken != default)
          query = query.Where(user => user.RefreshTokens.FirstOrDefault(token => token.Ip == refreshToken.Ip).Token == refreshToken.Token);
        else
          throw new NotFoundException<User>();

        return await query.FirstOrDefaultAsync() ?? throw new NotFoundException<User>();
    }
}