using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Settings;
using System.Linq.Expressions;

public static class MyExtensions
{
    private static readonly Random Rand = new();
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

    public static bool IsPositive(this int? number)
    {
        return (number ?? 0) > 0;
    }
    public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        foreach (T item in collection)
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
        if (sortField.Contains("-"))
        {
            method = "OrderBy";
            sortField = sortField.Replace("-", "");
        }

        if (typeof(T).GetMethod($"get_{sortField}") == default)
            return query;

        var param = Expression.Parameter(typeof(T));
        var prop = Expression.Property(param, sortField);
        var exp = Expression.Lambda(prop, param);
        var types = new[] { query.ElementType, exp.Body.Type };
        var rs = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
        return query.Provider.CreateQuery<T>(rs);
    }

}