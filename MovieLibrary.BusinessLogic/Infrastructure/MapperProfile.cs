using AutoMapper;
using MovieLibrary.BusinessLogic.Models;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.BusinessLogic.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Movie, MovieDTO>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.Name}"))
                .ForMember(
                    dest => dest.Year,
                    opt => opt.MapFrom(src => src.Year))
                .ForMember(
                    dest => dest.Rating,
                    opt => opt.MapFrom(src => src.Rating))
                .ForMember(
                    dest => dest.DirectorId,
                    opt => opt.MapFrom(src => src!.DirectorId))
                .ForPath(
                    dest => dest.DirectorFirstName,
                    opt => opt.MapFrom(src => $"{src!.Director!.FirstName}"))
                .ForPath(
                    dest => dest.DirectorLastName,
                    opt => opt.MapFrom(src => $"{src!.Director!.LastName}"));

            CreateMap<MovieDTO, Movie>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.Name}"))
                .ForMember(
                    dest => dest.Year,
                    opt => opt.MapFrom(src => src.Year))
                .ForMember(
                    dest => dest.Rating,
                    opt => opt.MapFrom(src => src.Rating))
                .ForMember(
                    dest => dest.DirectorId,
                    opt => opt.MapFrom(src => src!.DirectorId))
                .ForPath(
                    dest => dest.Director!.FirstName,
                    opt => opt.MapFrom(src => $"{src!.DirectorFirstName}"))
                .ForPath(
                    dest => dest.Director!.LastName,
                    opt => opt.MapFrom(src => $"{src!.DirectorLastName}"));


            CreateMap<Director, DirectorDTO>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(
                    dest => dest.Movies,
                    opt => opt.MapFrom(src => src.Movies));

            CreateMap<DirectorDTO, Director>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(
                    dest => dest.Movies,
                    opt => opt.MapFrom(src => src.Movies));

            CreateMap<Rating, RatingDTO>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.MovieId,
                    opt => opt.MapFrom(src => src.MovieId))
                .ForMember(
                    dest => dest.UserNickname,
                    opt => opt.MapFrom(src => src.UserNickname))
                .ForMember(
                    dest => dest.Value,
                    opt => opt.MapFrom(src => src.Value));

            CreateMap<RatingDTO, Rating>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.MovieId,
                    opt => opt.MapFrom(src => src.MovieId))
                .ForMember(
                    dest => dest.UserNickname,
                    opt => opt.MapFrom(src => src.UserNickname))
                .ForMember(
                    dest => dest.Value,
                    opt => opt.MapFrom(src => src.Value));

            CreateMap<Comment, CommentDTO>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.MovieId,
                    opt => opt.MapFrom(src => src.MovieId))
                .ForMember(
                    dest => dest.UserNickname,
                    opt => opt.MapFrom(src => src.UserNickname))
                .ForMember(
                    dest => dest.Text,
                    opt => opt.MapFrom(src => src.Text));

            CreateMap<CommentDTO, Comment>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.MovieId,
                    opt => opt.MapFrom(src => src.MovieId))
                .ForMember(
                    dest => dest.UserNickname,
                    opt => opt.MapFrom(src => src.UserNickname))
                .ForMember(
                    dest => dest.Text,
                    opt => opt.MapFrom(src => src.Text));
        }
    }
}
