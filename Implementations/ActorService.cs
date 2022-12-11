using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Actor;
using Models.Exceptions;

namespace Implementations
{
    public class ActorService : IActorService
    {
        private readonly AppDbContext _db;
        public ActorService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Actor> EditAsync(RequestActor reqActor, int actorId)
        {
            var actor = await _db.Actor.FindAsync(actorId);
            if (actor == default)
                throw new NotFoundException<Actor>();

            actor.Name = reqActor.Name;
            actor.Description = reqActor.Description;
            actor.DateOfBirth = reqActor.DateOfBirth;
            return await _db.SaveChangesAsync() != 0 ? actor : throw new EditingException<Actor>();
        }

        public async Task<Movie> EditMovieActorsAsync(Movie movie, EditMovieActors editMovieActors)
        {
            var actorsTask = editMovieActors.ActorsId.Select( async id=> await _db.Actor.FindAsync(id));
            var actors = await Task.WhenAll(actorsTask);
            if (actors.IsNullOrEmpty())
                throw new NotFoundException<Actor>();

            movie.Actors = actors!;
            return await _db.SaveChangesAsync() != 0 ? movie : throw new EditingException<Movie>();
        }

        public async Task<Actor> AddAsync(RequestActor reqActor)
        {
            var actor = new Actor
            {
                DateOfBirth = reqActor.DateOfBirth,
                Name = reqActor.Name,
                Description = reqActor.Description,
            };
            _db.Actor.Add(actor);
            return await _db.SaveChangesAsync() != 0 ?  actor : throw new AddingException<Actor>();
        }

        public async Task Delete(int id)
        {
            var actor = await _db.Actor.FindAsync(id);
            if (actor == default)
                throw new NotFoundException<Actor>();

            _db.Actor.Remove(actor);
            if (await _db.SaveChangesAsync() == 0)
                throw new DeletingException<Actor>();
        }

        public async Task<List<DtoActor>> GetActors(int? movieId = default)
        {
            IQueryable<Actor> actors = _db.Actor;
            if ((movieId ?? 0) != 0)
                actors = _db.Actor.Where(actor => actor.Movies.Any(movie => movie.Id == movieId));

            return await actors.Select(actor => new DtoActor(actor)).ToListAsync();
        }

        public async Task<DtoActor> GetActor(int id)
        {
            var actor =await _db.Actor.FindAsync(id);
            if (actor == default)
                throw new NotFoundException<Actor>();

            return new DtoActor(actor);
        }
    }
}
