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
                    dest => dest.Rating,
                    opt => opt.MapFrom(src => src.Rating))
                .ForMember(
                    dest => dest.Year,
                    opt => opt.MapFrom(src => src.Year))
                .ForMember(
                    dest => dest.DirectorId,
                    opt => opt.MapFrom(src => src!.Director!.Id))
                .ForMember(
                    dest => dest.DirectorFirstName,
                    opt => opt.MapFrom(src => $"{src!.Director!.FirstName}"))
                .ForMember(
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
                    dest => dest.Rating,
                    opt => opt.MapFrom(src => src.Rating))
                .ForMember(
                    dest => dest.Year,
                    opt => opt.MapFrom(src => src.Year))
                .ForMember(
                    dest => dest.DirectorId,
                    opt => opt.MapFrom(src => src!.DirectorId))
                .ForPath(
                    dest => dest.Director!.FirstName,
                    opt => opt.MapFrom(src => $"{src!.DirectorFirstName}"))
                .ForPath(
                    dest => dest.Director!.LastName,
                    opt => opt.MapFrom(src => $"{src!.DirectorLastName}"));
        }
    }
}
