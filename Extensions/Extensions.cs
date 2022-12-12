﻿using Microsoft.AspNetCore.Http;
public static class MyExtensions
{
    private static readonly Random Rand = new();
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? query)
    {
        return query == null || query.Any() == false;
    }
    public static T? GetRandom<T> (this IQueryable<T> collection)
    {
        return collection.IsNullOrEmpty() ? default : collection.ToList()[Rand.Next(collection.Count())];
    }
    public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        foreach (T item in collection)
        {
            action(item);
        }
    }

}