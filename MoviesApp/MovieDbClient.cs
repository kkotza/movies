using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MoviesApp
{
    public class MovieDbClient: IApiClient
    {
        static readonly string apiKey;
        static readonly string apiUrl;
        static readonly string apiGetRunningMethod;
        static readonly string apiGetCrewMethod;
        static readonly string apiGetPersonMethod;

        HttpClient client = new HttpClient();

        /// <summary>
        /// 
        /// </summary>
        static MovieDbClient()
        {
            var appSettings = ConfigurationManager.AppSettings;

            apiKey = appSettings["MovieDbApiKey"];
            apiUrl = appSettings["MovieDbApiUrl"];
            apiGetRunningMethod = appSettings["MovieDbApiGetRunning"];
            apiGetCrewMethod = appSettings["MovieDbApiGetCrew"];
            apiGetPersonMethod = appSettings["MovieDbApiGetPerson"];        
        }       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> GetMovies()
        {                           
            //get running movies
            var serializer = new DataContractJsonSerializer(typeof(MoviesCollection));
            var streamTask = client.GetStreamAsync(string.Concat(apiUrl, string.Format(apiGetRunningMethod, apiKey)));
            MoviesCollection movies = serializer.ReadObject(await streamTask) as MoviesCollection;           

            return movies?.Items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Person>> GetMovieCrew(int movieId, string job = null)
        {
            var serializer = new DataContractJsonSerializer(typeof(Movie));
            var streamTask = client.GetStreamAsync(string.Concat(apiUrl, string.Format(apiGetCrewMethod, movieId, apiKey)));

            var result = (serializer.ReadObject(await streamTask) as Movie).CrewList;

            if (job != string.Empty) result=result.Where(x => x.Job.Equals(job, StringComparison.CurrentCultureIgnoreCase)).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<Person> GetPersonDetails(int personId)
        {
            var serializer = new DataContractJsonSerializer(typeof(Person));
            var streamTask = client.GetStreamAsync(string.Concat(apiUrl, string.Format(apiGetPersonMethod, personId, apiKey)));

            return serializer.ReadObject(await streamTask) as Person;           
        }
    }
}
