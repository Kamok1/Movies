using Abstractions;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
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

    public async Task<User> GetUser(int id)
    {
        var user =  await GetUsersQuery(id).FirstOrDefaultAsync();
        if (user == default)
            throw new NotFoundException<User>();
        return user;
    }

    public async Task<User> GetUser(HttpContext httpContext)
    {
        var user = await GetUsersQuery(httpContext: httpContext).FirstOrDefaultAsync();
        if (user == default)
            throw new NotFoundException<User>();
        return user;
    }

    public IQueryable<User> GetUsersQuery(int? id = default, HttpContext? httpContext = default, string? login = null, int? roleId = default, string? displayName = null,
        bool includeRole = true, bool includeFavMovies = false, bool includeReviews = false, bool asNoTracking = false)
    {
        var query = PrepareQuery(includeRole, includeFavMovies, includeReviews, asNoTracking);
        if((id ?? 0) != 0)
            return query.Where(user=> user.Id == id);
        if(int.TryParse(httpContext?.Items["UserId"]?.ToString(), out var userId))
            return query.Where(user=> user.Id == userId);
        if(login != null)
            return query.Where(user=> user.Login == login || user.Email == login);
        if((roleId ?? 0) != 0 && includeRole)
            query = query.Where(user=> user.Role.Id == roleId);
        if(displayName != null)
            query = query.Where(user=>user.DisplayName.Contains(displayName));

        return query;
    }
    private IQueryable<User> PrepareQuery(bool includeRole, bool includeFavMovies, bool includeReviews, bool asNoTracking)
    {
        var query = _db.User
            .AsQueryable();
        
        if(includeFavMovies)
            query = query.Include(user=>user.UserFavouriteMovies);
        if(includeRole)
            query = query.Include(user=>user.Role);
        if(includeReviews)
            query = query.Include(user=>user.Reviews);
        if(asNoTracking)
            query = query.AsNoTracking();

        return query;
    }

    public async Task<User?> CreateAsync(UserRegisterRequest reqUser)
    {
        if (_db.User.Any(x => x.Login == reqUser.Name || x.Email == reqUser.Email))
            return null;
        
        var passwordSalt = GenerateSalt(64);
        var password = GenerateSaltedHash(passwordSalt, reqUser.Password);

        var role = await _db.Role.Where(x => x.Name == "User").FirstOrDefaultAsync();

        if (role == null)
            return null;

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
        return await _db.SaveChangesAsync() != 0 ? newUser : null;
    }

    public async Task<bool> ChangePassword(User user, PasswordChange passwords)
    {
        if (VerifyPassword(passwords.OldPassword,user.Password, user.PasswordSalt) == false)
            return false;

        var passwordSalt = GenerateSalt(64);
        user.Password = GenerateSaltedHash(passwordSalt, passwords.NewPassword);
        user.PasswordSalt = passwordSalt;
        return await _db.SaveChangesAsync() != 0;
    }

    public async Task<User?> EditProfile(User user, EditUserProfile editProfile)
    {
        user.DisplayName = editProfile.DisplayName;
        user.Description = editProfile.Description;
        _db.User.Update(user);
        return await _db.SaveChangesAsync() != 0 ? user : null;
    }

    public async Task<bool> DeleteAsync(User reqUser)
    {
        _db.User.Remove(reqUser);
        return await _db.SaveChangesAsync() != 0;
    }

    public Task<bool> ChangeEmail(User reqUser, EmailChange email)
    {
        throw new NotImplementedException();
    }
}