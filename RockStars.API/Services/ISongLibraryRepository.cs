using RockStars.API.Entities;
using RockStars.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongLibrary.API.Services
{
    /// <summary>
    /// Interface used to Get/Store/Update all the information related to Artists and Songs available in the Repository
    /// </summary>
    public interface ISongLibraryRepository
    {
        /// <summary>
        /// Adds an artist to the repository
        /// </summary>
        /// <param name="artist"></param>
        void AddArtist(Artist artist);
        
        /// <summary>
        /// Deletes an artist from the repository
        /// </summary>
        /// <param name="artist"></param>
        void DeleteArtist(Artist artist);

        /// <summary>
        /// Updates an artist  from the repository
        /// </summary>
        /// <param name="artist"></param>
        void UpdateArtist(Artist artist);

        /// <summary>
        /// Checks per Id if an artists exists
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns>true if exists false if not</returns>
        bool ArtistExists(int artistId);

        /// <summary>
        /// Checks per Id if a song exists
        /// </summary>
        /// <param name="songId"></param>
        /// <returns>true if exists false if not</returns>
        bool SongExists(int songId);
        
        /// <summary>
        /// Saves all changes inside the repository
        /// </summary>
        /// <returns>true if saved false if not</returns>
        bool Save();    
        
        /// <summary>
        /// Adds a song for an specific artist parsed by Id
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="song"></param>
        void AddSong(int artistId, Song song);

        /// <summary>
        /// Updates a specfic song
        /// </summary>
        /// <param name="song"></param>
        void UpdateSong(Song song);

        /// <summary>
        /// Deletes a specific song
        /// </summary>
        /// <param name="song"></param>
        void DeleteSong(Song song);

        /// <summary>
        /// Gets all available songs from an artist parsed by Id
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns>IEnumerable object of Songs</returns>
        IEnumerable<Song> GetSongs(int artistId);

        /// <summary>
        /// Gets all available songs obtained per Ids
        /// </summary>
        /// <param name="songIds"></param>
        /// <returns>IEnumerable object of Songs</returns>
        IEnumerable<Song> GetSongs(IEnumerable<int> songIds);

        /// <summary>
        /// Gets songs per genre with a Query/Genre/Year defined as key
        /// </summary>
        /// <param name="songsResourceParameters"></param>
        /// <returns>IEnumerable object of songs</returns>
        IEnumerable<Song> GetSongs(SongsResourceParameters songsResourceParameters);

        /// <summary>
        /// Gets all artists
        /// </summary>
        /// <returns>IEnumerable object of artists</returns>
        IEnumerable<Artist> GetArtists();
      
        /// <summary>
        /// Gets several artists per Id
        /// </summary>
        /// <param name="artistIds"></param>
        /// <returns>IEnumerable object of artists</returns>
        IEnumerable<Artist> GetArtists(IEnumerable<int> artistIds);

        /// <summary>
        /// Gets artists per genre with a Query or Genre defined as key
        /// </summary>
        /// <param name="artistsResourceParameters"></param>
        /// <returns>IEnumerable object of artists</returns>
        IEnumerable<Artist> GetArtists(ArtistsResourceParameters artistsResourceParameters);

        /// <summary>
        /// Gets a specific Song from an artist using the artist Id and song Id 
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="songId"></param>
        /// <returns>Entitie Song</returns>
        Song GetSong(int artistId, int songId);

        /// <summary>
        /// Gets a specific artist from its Id
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Artist GetArtist(int artistId);

        /// <summary>
        /// Gets the first Artist Id per Name
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns>nullable integer with artist Id found with name parsed</returns>
        int? GetArtistId(string artistName);

        /// <summary>
        /// Saves everything in the DbContext asynchronously
        /// </summary>
        /// <returns>true if saved false if not</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// Adds a Song to the Repository asynchronously
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="song"></param>
        /// <returns>void Task</returns>
        Task AddSongAsync(int artistId, Song song);

        /// <summary>
        /// Adds an Artist to the Repository asynchronously
        /// </summary>
        /// <param name="artist"></param>
        /// <returns>Void Task</returns>
        Task AddArtistAsync(Artist artist);
    }
}
