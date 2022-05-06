using System.Collections.Generic;

namespace RockStars.API.Models
{
    public class ArtistForCreationDto : ArtistDto
    {
        public ICollection<SongForCreationDto> Songs { get; set; }
          = new List<SongForCreationDto>();
    }
}
