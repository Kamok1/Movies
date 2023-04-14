using System.Net;
using Data;
using Data.Models;
using Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.User;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task Creating_User_Should_Find_him()
    {
        var mockOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("movieslibrary")
            .Options;
        var db = new AppDbContext(mockOptions);
        db.Role.Add(new Role() { Name = "User", Id = 0 });
        await db.SaveChangesAsync();
        var service = new UserService(db);

        await service.CreateAsync(new UserRegisterRequest(){Email = "test@gmail.com", Name = "Name", Password = "Password"});

        Assert.IsNotNull(await db.User.Where(x=>x.Email == "test@gmail.com").FirstOrDefaultAsync());
    }
}