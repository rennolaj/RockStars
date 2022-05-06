using System.ComponentModel.DataAnnotations;

namespace RockStars.API.Models
{
    public class SongForUpdateDto : SongForManipulationDto
    {
        [Required(ErrorMessage = "The name is required.")]
        public override string Name { get => base.Name; set => base.Name = value; }
    }
}
