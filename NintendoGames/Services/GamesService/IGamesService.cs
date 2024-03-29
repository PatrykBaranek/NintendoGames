﻿using NintendoGames.Models.GamesModels;

namespace NintendoGames.Services.GamesService
{
    public interface IGamesService
    {
        Task<List<GameDto>> GetAllGames();
        Task<List<GameDto>> GetGamesByName(string gameName);
        Task DeleteGame(Guid gameId);

    }
}