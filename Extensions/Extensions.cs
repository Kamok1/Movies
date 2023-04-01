using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Settings;
using System.Linq.Expressions;
using Npgsql.Internal.TypeHandlers.LTreeHandlers;
using Data.Migrations;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

public static class MyExtensions
{
    private static readonly AppSettings AppSetting = new();
    static MyExtensions()
    {
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build()
            .GetSection("AppSettings")
            .Bind(AppSetting);
    }


    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? query)
    {
        return query == null || query.Any() == false;
    }
    public static async Task<T?> GetRandom<T>(this IQueryable<T> collection)
    {
        return collection.IsNullOrEmpty()
            ? default
            : await collection.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
  }

    public static string OnlyAllowedCharacters(this string text)
    {
      return string.Join("", Regex.Matches(text, @"[a-zA-Z0-9]").Select(match => match.Value));
    }

  public static bool IsPositive(this int? number)
    {
        return (number ?? 0) > 0;
    }
    public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
    public static IQueryable<T> Pagination<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (page <= 0)
            return query;

        if (pageSize <= 0)
            pageSize = AppSetting.PageSize;
        return query.Skip(pageSize * --page)
            .Take(pageSize);
    }
    public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> query, string sortField)
    {
        var method = "OrderByDescending";
        var genericType = typeof(T);
        if (sortField.Contains("-"))
        {
            method = "OrderBy";
            sortField = sortField.Replace("-", "");
        }

        if (genericType.GetMethod($"get_{sortField}") == default)
            return query;

        var param = Expression.Parameter(genericType); //param = T e.g Movie
        var prop = Expression.Property(param, sortField); // prop = T.Field e.g Movie.Id
        var exp = Expression.Lambda(prop, param); // exp = T => T.Field eg. Movie => Movie.Id
        var types = new[] { genericType, exp.Body.Type };
        var rs = Expression.Call(typeof(Queryable), method, types, query.Expression, exp); // E.g OrderByDescending(Movie => Movie.Id
        return query.Provider.CreateQuery<T>(rs);
    }

}