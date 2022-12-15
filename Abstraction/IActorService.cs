﻿using Data.Models;
using Models.Actor;

namespace Abstractions;
public interface IActorService
{
    Task EditAsync(RequestActor reqActor, int id);
    Task EditMovieActorsAsync(Movie movie, EditMovieActors editMovieActor);
    Task AddAsync(RequestActor actor);
    Task DeleteAsync(int id);
    Task<List<DtoActor>> GetActorsDtoAsync(int? movieId);
    Task<DtoActor> GetActorDtoAsync(int id);
}
