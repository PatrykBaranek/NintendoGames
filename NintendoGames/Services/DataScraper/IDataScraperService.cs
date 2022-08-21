﻿using NintendoGames.Models.DataScraper;

namespace NintendoGames.Services.DataScraper
{
    public interface IDataScraperService
    {
        List<GameDto> GetList();
        Task<List<GameDto>> GetNintendoGames();

    }
}