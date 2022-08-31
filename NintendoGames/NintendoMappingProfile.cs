using AutoMapper;
using NintendoGames.Entities;
using NintendoGames.Models.DataScraper;

namespace NintendoGames
{
    public class NintendoMappingProfile : Profile
    {
        public NintendoMappingProfile()
        {
            CreateMap<Game, GameDto>()
                .ForMember(g => g.GameTitle, c => c.MapFrom(g => g.Title));
        }
    }
}
