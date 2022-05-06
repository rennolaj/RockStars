using System.ComponentModel.DataAnnotations;

namespace RockStars.API.Models
{
    public abstract class SongForManipulationDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the song is required.")]
        [MaxLength(1000, ErrorMessage = "The name shouldn't have more than 1000 characters.")]
        public virtual string Name { get; set; }

        [MaxLength(1500, ErrorMessage = "The Gender shouldn't have more than 1500 characters.")]
        public virtual string Genre { get; set; }

        [MaxLength(1500, ErrorMessage = "The SpotifyId shouldn't have more than 1500 characters.")]
        public virtual string? SpotifyId { get; set; }

        [MaxLength(1500, ErrorMessage = "The Album shouldn't have more than 1500 characters.")]
        public virtual string? Album { get; set; }

        public virtual uint? Year { get; set; }

        [MaxLength(1500, ErrorMessage = "The Shortname shouldn't have more than 1500 characters.")]
        public virtual string Shortname { get; set; }

        public virtual uint? Bpm { get; set; }

        public virtual uint Duration { get; set; }

        [MaxLength(1500, ErrorMessage = "The Artist shouldn't have more than 1500 characters.")]
        public virtual string Artist { get; set; }
    }
}
