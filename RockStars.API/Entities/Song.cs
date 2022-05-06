using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockStars.API.Entities
{
    public class Song
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Name { get; set; }

        public uint? Year { get; set; }

        [ForeignKey("ArtistId")]
        public Artist ArtistObject { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [MaxLength(150)]
        public string Artist { get; set; }

        public uint? Bpm { get; set; }

        public uint Duration { get; set; }

        [MaxLength(1500)]
        public string Genre { get; set; }

        [MaxLength(1500)]
        public string? SpotifyId { get; set; }

        [MaxLength(1500)]
        public string? Album { get; set; }

        [MaxLength(1500)]
        public string ShortName { get; set; }
    }
}
