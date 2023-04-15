using Microsoft.EntityFrameworkCore;
using Models.Exceptions;

namespace EfExtensions
{
  public static class EfExtensions
  {
    public async static Task<T> FindOrThrowErrorAsync<T>(this DbSet<T> dbSet, int id) where T : class
    {
      return await dbSet.FindAsync(id) ?? throw new NotFoundException<T>();
    }
  }
}