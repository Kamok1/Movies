﻿using Abstractions;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Actor;
using Models.Exceptions;
using Models.Settings;

namespace Implementations
{
    public class ActorService : IActorService
    {
        private readonly AppDbContext _db;
        private readonly IFileService _fileService;
        private readonly FileSettings _fileSettings;
        public ActorService(AppDbContext db, IFileService fileService, AppSettings settings)
        {
            _db = db;
            _fileService = fileService;
            _fileSettings = settings.File;
        }

        public async Task EditAsync(RequestActor reqActor, int actorId)
        {
            var actor = await GetActorAsync(actorId);

            actor.Name = reqActor.Name;
            actor.Description = reqActor.Description;
            actor.DateOfBirth = reqActor.DateOfBirth;
            if (await _db.SaveChangesAsync() == 0)
                throw new EditingException<Actor>();
        }

        public async Task EditActorPicture(IFormFile picture, int actorId)
        {
          var actor = await GetActorAsync(actorId);
          await _fileService.SaveFile(picture, $"{actor.Id}.jpg", Path.Combine(_fileSettings.ResourcesPath, _fileSettings.ActorPicturesPath));
          actor.PhotoPath = Path.Combine(_fileSettings.ActorPicturesPath, $"{actor.Id}.jpg");

          if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<Actor>();
    }
        public async Task EditMovieActorsAsync(Movie movie, EditMovieActors editMovieActors)
        {
            await _db.Entry(movie).Collection(m => m.Actors).LoadAsync();

            var actorsTask = editMovieActors.ActorsId.Select(async id => await GetActorAsync(id));
            var actors = await Task.WhenAll(actorsTask);
            movie.Actors = actors;
            if (await _db.SaveChangesAsync() == 0)
                throw new EditingException<Movie>();
        }

        public async Task AddAsync(RequestActor reqActor)
        {
            var actor = new Actor
            {
                DateOfBirth = reqActor.DateOfBirth,
                Name = reqActor.Name,
                Description = reqActor.Description,
                PhotoPath = _fileSettings.PlaceholderPicturePath
            };
            await _db.Actor.AddAsync(actor);
            if (await _db.SaveChangesAsync() == 0)
                throw new AddingException<Actor>();
        }

        public async Task DeleteAsync(int id)
        {
            _db.Actor.Remove(await GetActorAsync(id));
            if (await _db.SaveChangesAsync() == 0)
                throw new DeletingException<Actor>();
        }

        public async Task<List<DtoActor>> GetActorsDtoAsync(int? movieId = default)
        {
            IQueryable<Actor> actors = _db.Actor;
            if ((movieId ?? 0) > 0)
                actors = _db.Actor.Where(actor => actor.Movies.Any(movie => movie.Id == movieId));

            return await actors.Select(actor => new DtoActor(actor)).ToListAsync();
        }
        public async Task<DtoActor> GetActorDtoAsync(int id)
        {
            return new DtoActor(await GetActorAsync(id));
        }
        private async Task<Actor> GetActorAsync(int id)
        {
            var actor = await _db.Actor.FindAsync(id);
            if (actor == default)
                throw new NotFoundException<Actor>();

            return actor;
        }
    }
}
