using Data.Models;
using Models.Actor;

namespace Abstractions
{
    public interface IActorService
    {
        Task<Actor> EditAsync(RequestActor reqActor, int actorId);
        Task<Movie> EditMovieActorsAsync(Movie movie ,EditMovieActors editMovieActor);
        Task<Actor> AddAsync(RequestActor actor);
        Task Delete(int id);
        Task<List<DtoActor>> GetActors(int? movieId);
        Task<DtoActor> GetActor(int id);

    }
}
