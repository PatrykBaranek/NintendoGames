using AutoMapper;
using NintendoGames.Entities;
using NintendoGames.Models.DataScraperModels;
using NintendoGames.Models.DevelopersModels;
using NintendoGames.Models.GamesModels;
using NintendoGames.Models.GenresModels;
using NintendoGames.Models.RatingModels;
using NintendoGames.Models.UserRequestModels;
using NintendoGames.Models.WishListModels;

namespace NintendoGames
{
    public class NintendoMappingProfile : Profile
    {
        public NintendoMappingProfile()
        {
            // DataScraper
            CreateMap<Game, ScrapedGameDto>()
                .ForMember(g => g.GameTitle, c => c.MapFrom(g => g.Title));

            CreateMap<Game, ScrapedGameFromDbDto>()
                .ForMember(g => g.CriticRating, c => c.MapFrom(g => g.Rating.CriticRating))
                .ForMember(g => g.UserScore, c => c.MapFrom(g => g.Rating.UserScore))
                .ForMember(g => g.IsMustPlay, c => c.MapFrom(g => g.Rating.IsMustPlay))
                .ForMember(g => g.GenreName, c => c.MapFrom(g => g.Genres.Select(g => g.Name).ToList()))
                .ForMember(g => g.DeveloperName, c => c.MapFrom(g => g.Developers.Select(d => d.Name).ToList()));


            // Games
            CreateMap<Game, GameDto>()
                .ForMember(g => g.CriticRating, c => c.MapFrom(g => g.Rating.CriticRating))
                .ForMember(g => g.UserScore, c => c.MapFrom(g => g.Rating.UserScore))
                .ForMember(g => g.IsMustPlay, c => c.MapFrom(g => g.Rating.IsMustPlay))
                .ForMember(g => g.GenreName, c => c.MapFrom(g => g.Genres.Select(g => g.Name).ToList()))
                .ForMember(g => g.DeveloperName, c => c.MapFrom(g => g.Developers.Select(d => d.Name).ToList()));

            // Ratings
            CreateMap<UpdateUserScoreDto, Rating>();


            // Developers
            CreateMap<Developers, DevelopersDto>();
            CreateMap<AddDeveloperDto, Developers>()
                .ForMember(d => d.Name, c => c.MapFrom(d => d.DeveloperName));


            // Genres
            CreateMap<Genres, GenresDto>();
            CreateMap<AddGenreDto, Genres>()
                .ForMember(ge => ge.Name, c => c.MapFrom(ge => ge.GenreName));


            // WishList
            CreateMap<Game, WishListDto>()
                .ForMember(g => g.CriticRating, c => c.MapFrom(g => g.Rating.CriticRating))
                .ForMember(g => g.UserScore, c => c.MapFrom(g => g.Rating.UserScore))
                .ForMember(g => g.IsMustPlay, c => c.MapFrom(g => g.Rating.IsMustPlay))
                .ForMember(g => g.GenreName, c => c.MapFrom(g => g.Genres.Select(g => g.Name).ToList()))
                .ForMember(g => g.DeveloperName, c => c.MapFrom(g => g.Developers.Select(d => d.Name).ToList()));

            // UserRequest 
            CreateMap<UserRequest, UserRequestDto>();
        }
    }
}
