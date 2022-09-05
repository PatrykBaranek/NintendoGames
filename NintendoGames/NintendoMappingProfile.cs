using AutoMapper;
using NintendoGames.Entities;
using NintendoGames.Models.DataScraperModels;
using NintendoGames.Models.DevelopersModels;
using NintendoGames.Models.GamesModels;
using NintendoGames.Models.RatingModels;

namespace NintendoGames
{
    public class NintendoMappingProfile : Profile
    {
        public NintendoMappingProfile()
        {
            CreateMap<Game, ScrapedGameDto>()
                .ForMember(g => g.GameTitle, c => c.MapFrom(g => g.Title));

            CreateMap<Game, GameDto>()
                .ForMember(g => g.CriticRating, c => c.MapFrom(g => g.Rating.CriticRating))
                .ForMember(g => g.UserScore, c => c.MapFrom(g => g.Rating.UserScore))
                .ForMember(g => g.IsMustPlay, c => c.MapFrom(g => g.Rating.IsMustPlay))
                .ForMember(g => g.GenreName, c => c.MapFrom(g => g.Genres.Select(g => g.Name).ToList()))
                .ForMember(g => g.DeveloperName, c => c.MapFrom(g => g.Developers.Select(d => d.Name).ToList()));

            CreateMap<UpdateUserScoreDto, Rating>();

            CreateMap<AddDeveloperDto, Developers>()
                .ForMember(d => d.Name, c => c.MapFrom(d => d.DeveloperName));
        }
    }
}
