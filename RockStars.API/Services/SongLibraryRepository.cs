using RockStars.API.DbContexts;
using RockStars.API.Entities;
using RockStars.API.ResourceParameters;
using SongLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockStars.API.Services
{
    public class SongLibraryRepository : ISongLibraryRepository, IDisposable
    {
        private const int BEGIN_OF_INVALID_ID_VALUE = 0;
        private readonly SongLibraryContext dbContext;

        public SongLibraryRepository(SongLibraryContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public void AddArtist(Artist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist));
            }

            dbContext.Artists.Add(artist);
        }

        /// <inheritdoc />
        public async Task AddArtistAsync(Artist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist));
            }

            await dbContext.Artists.AddAsync(artist);
        }

        /// <inheritdoc />
        public void AddSong(int artistId, Song song)
        {
            if (artistId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(artistId));
            }

            if (song == null)
            {
                throw new ArgumentNullException(nameof(song));
            }
            song.ArtistId = artistId;
            dbContext.Songs.Add(song);
        }

        /// <inheritdoc />
        public async Task AddSongAsync(int artistId, Song song)
        {
            if (artistId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(artistId));
            }

            if (song == null)
            {
                throw new ArgumentNullException(nameof(song));
            }
            song.ArtistId = artistId;
            await dbContext.Songs.AddAsync(song);
        }

        /// <inheritdoc />
        public bool ArtistExists(int artistId)
        {
            if (artistId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(artistId));
            }

            return dbContext.Artists.Any(a => a.Id == artistId) || dbContext.Artists.Local.Any(a => a.Id == artistId);
        }

        /// <inheritdoc />
        public bool SongExists(int songId)
        {
            if (songId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(songId));
            }

            return dbContext.Songs.Any(a =>a.Id == songId) || dbContext.Songs.Local.Any(a => a.Id == songId);
        }

        /// <inheritdoc />
        public void DeleteArtist(Artist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist));
            }

            dbContext.Artists.Remove(artist);
        }

        /// <inheritdoc />
        public void DeleteSong(Song song)
        {
            if (song == null)
            {
                throw new ArgumentNullException(nameof(song));
            }

            dbContext.Songs.Remove(song);
        }

        /// <inheritdoc />
        public Artist GetArtist(int artistId)
        {
            if (artistId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(artistId));
            }

            return dbContext.Artists.FirstOrDefault(a => a.Id == artistId);
        }

        /// <inheritdoc />
        public int? GetArtistId(string artistName)
        {
            if (artistName == string.Empty)
            {
                throw new ArgumentNullException(nameof(artistName));
            }

            int? artistId;
            artistId = dbContext.Artists.FirstOrDefault(a => a.Name == artistName)?.Id;

            if(artistId == null) 
            {
                artistId = dbContext.Artists.Local.FirstOrDefault(a => a.Name == artistName)?.Id;
            }

            return artistId;
        }

        /// <inheritdoc />
        public IEnumerable<Artist> GetArtists()
        {
            return dbContext.Artists.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<Artist> GetArtists(ArtistsResourceParameters artistsResourceParameters)
        {
            if (artistsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(artistsResourceParameters));
            }

            if (string.IsNullOrWhiteSpace(artistsResourceParameters.Genre)
                 && string.IsNullOrWhiteSpace(artistsResourceParameters.SearchQuery))
            {
                return GetArtists();
            }

            var collection = dbContext.Artists as IQueryable<Artist>;

            if (!string.IsNullOrWhiteSpace(artistsResourceParameters.Genre))
            {
                var genre = artistsResourceParameters.Genre.Trim();
                collection = collection.Where(a => a.Songs.Any(s => s.Genre.Contains(genre)));
            }

            if (!string.IsNullOrWhiteSpace(artistsResourceParameters.SearchQuery))
            {

                var searchQuery = artistsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Songs.Any(s => s.Genre == searchQuery)
                    || a.Name.Contains(searchQuery));
            }

            return collection.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<Artist> GetArtists(IEnumerable<int> artistIds)
        {
            if (artistIds == null)
            {
                throw new ArgumentNullException(nameof(artistIds));
            }

            return dbContext.Artists.Where(a => artistIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        /// <inheritdoc />
        public Song GetSong(int artistId, int songId)
        {
            if (artistId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(artistId));
            }

            if (songId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(songId));
            }

            return dbContext.Songs
              .Where(c => c.ArtistId == artistId && c.Id == songId).FirstOrDefault();

        }

        /// <inheritdoc />
        public IEnumerable<Song> GetSongs(int artistId)
        {
            if (artistId < BEGIN_OF_INVALID_ID_VALUE)
            {
                throw new ArgumentException(nameof(artistId));
            }

            return dbContext.Songs
                        .Where(c => c.ArtistId == artistId)
                        .OrderBy(c => c.ArtistObject).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<Song> GetSongs(IEnumerable<int> songIds)
        {
            if (songIds == null)
            {
                throw new ArgumentNullException(nameof(songIds));
            }

            return dbContext.Songs.Where(a => songIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        /// <inheritdoc />
        public IEnumerable<Song> GetSongs(SongsResourceParameters songsResourceParameters)
        {
            if (songsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(songsResourceParameters));
            }

            if (string.IsNullOrWhiteSpace(songsResourceParameters.Genre)
                 && string.IsNullOrWhiteSpace(songsResourceParameters.SearchQuery)
                 && songsResourceParameters.Year == null)
            {
                return dbContext.Songs
                    .OrderBy(a => a.Name)
                    .ToList();
            }

            var collection = dbContext.Songs as IQueryable<Song>;

            if (songsResourceParameters.Year != null)
            {
                collection = GetSongsPerYear((uint)songsResourceParameters.Year);
            }

            if (!string.IsNullOrWhiteSpace(songsResourceParameters.Genre))
            {
                var genre = songsResourceParameters.Genre.Trim();
                collection = GetSongsPerGenre(genre);
            }

            if(songsResourceParameters.Year != null && !string.IsNullOrWhiteSpace(songsResourceParameters.Genre))
            {
                collection = collection.Where(c =>
                        c.Genre == songsResourceParameters.Genre &&
                        c.Year == songsResourceParameters.Year
                    );
            }

            if (!string.IsNullOrWhiteSpace(songsResourceParameters.SearchQuery))
            {
                var searchQuery = songsResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Genre == searchQuery
                    || a.Name.Contains(searchQuery)
                    || a.Album.Contains(searchQuery)
                    || a.Artist.Contains(searchQuery));
            }

            return collection.ToList();
        }

        /// <inheritdoc />
        public bool Save()
        {
            return (dbContext.SaveChanges() >= 0);
        }

        /// <inheritdoc />
        public async Task<bool> SaveAsync()
        {
            return (await dbContext.SaveChangesAsync() >= 0);
        }

        /// <inheritdoc />
        public void UpdateArtist(Artist artist)
        {
            dbContext.Artists.Update(artist);
        }

        /// <inheritdoc />
        public void UpdateSong(Song song)
        {
            dbContext.Songs.Update(song);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        private IQueryable<Song> GetSongsPerGenre(string genre)
        {
            if (genre == string.Empty)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            return dbContext.Songs
                        .Where(c => c.Genre.Contains(genre))
                        .OrderBy(c => c.Artist);
        }

        private IQueryable<Song> GetSongsPerYear(uint year)
        {
            if (year < 0)
            {
                throw new ArgumentException(nameof(year));
            }

            return dbContext.Songs
                        .Where(c => c.Year == year)
                        .OrderBy(c => c.Artist);
        }
    }
}
