using AutoMapper;
using NintendoGames.Entities;
using NintendoGames.Models.DataScraper;

namespace NintendoGames
{
    public class NintendoMappingProfile : Profile
    {
        public NintendoMappingProfile()
        {
            CreateMap<Game, GameDto>();
        }
    }
}
