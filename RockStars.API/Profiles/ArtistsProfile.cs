using AutoMapper;

namespace RockStars.API.Profiles
{
    public class ArtistsProfile : Profile
    {
        public ArtistsProfile()
        {
            CreateMap<Entities.Artist, Models.ArtistDto>();
            CreateMap<Models.ArtistForCreationDto, Entities.Artist>();
        }
    }
}
