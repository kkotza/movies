using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static System.Console;

namespace MoviesApp
{
    class Program
    {        
        static void Main(string[] args)
        {
            try
            {
                WriteLine("getting now playing movies..");
                IEnumerable<Movie> taskMovies = ProcessMovies().Result;
                               
                WriteLine("update database..");
                ProcessDbUpdate(taskMovies);                
            }
            catch{}

            ReadKey();           
        }       
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static async Task<IEnumerable<Movie>> ProcessMovies()
        {
            IEnumerable<Movie> movies = null;

            try
            {
                var caller = new MovieDbClient();
                movies = await caller.GetMovies();

                WriteLine($"completed download, number of running movies:{movies?.ToList().Count ?? 0}");
                WriteLine("getting movies details..");                

                //get movies crew info         
                foreach (Movie m in movies)
                {
                    m.CrewList = caller.GetMovieCrew(m.MovieId, "Director").Result;

                    foreach (Person p in m.CrewList)
                    {                        
                        var personDetails = caller.GetPersonDetails(p.PersonId).Result;
                        p.ImdbLink = personDetails.ImdbLink;
                    }
                }
            }
            catch
            {
                WriteLine("error in download/process movies");
                throw;
            }
            
            return movies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movies"></param>
        private static void ProcessDbUpdate(IEnumerable<Movie> movies)
        {
            try
            {
                //update movies
                Movie.UpdateMany(movies);

                //get distinct crew from all movies
                var persons = movies.SelectMany(x => x.CrewList).Distinct(); 
                //update crew
                Person.UpdateMany(persons);
               
                //update movie-crew relation
                foreach(var m in movies)
                {
                    m.UpdateCrew();
                }

                WriteLine("completed database update");
            }
            catch
            {
                WriteLine("error in database update process");
                throw;
            }            
        }
    }
}
