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
                .ForMember(g => g.GameTitle, c => c.MapFrom(gm => gm.Title))
                .ForMember(g => g.MoreDetails.Ratings, c => c.MapFrom(gm => gm.Rating))
                .ForMember(g => g.MoreDetails.Developers, c => c.MapFrom(gm => gm.Developers))
                .ForMember(g => g.MoreDetails.Genres, c => c.MapFrom(gm => gm.Genres));
        }
    }
}
