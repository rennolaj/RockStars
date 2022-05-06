
using AutoMapper;

namespace RockStars.API.Profiles
{
    public class SongsProfile : Profile
    {
        public SongsProfile()
        {
            CreateMap<Entities.Song, Models.SongDto>();
            CreateMap<Models.SongForCreationDto, Entities.Song>();
            CreateMap<Models.SongForUpdateDto, Entities.Song>();
            CreateMap<Entities.Song, Models.SongForUpdateDto>();
        }
    }
}
