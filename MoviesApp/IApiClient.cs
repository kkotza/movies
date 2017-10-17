using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApp
{
    public interface IApiClient
    {
        Task<IEnumerable<Movie>> GetMovies();

        Task<IEnumerable<Person>> GetMovieCrew(int movieId, string job = null);

        Task<Person> GetPersonDetails(int personId);
    }
}
