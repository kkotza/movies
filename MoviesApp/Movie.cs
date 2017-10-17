using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Configuration;

namespace MoviesApp
{
    [DataContract()]
    public class Movie
    {
        [DataMember(Name = "id")]
        public int MovieId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "overview")]
        public string Description { get; set; }

        [DataMember(Name = "original_title")]
        public string OriginalTitle { get; set; }

        [IgnoreDataMember]
        public DateTime LastUpdate { get; set; }

        [DataMember(Name = "crew")]
        public IEnumerable<Person> CrewList { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movies"></param>
        public static void UpdateMany(IEnumerable<Movie> movies)
        {
            using (var connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString))
            {
                connection.Open();
                NpgsqlTransaction tran = connection.BeginTransaction();

                try
                {
                    foreach (var m in movies)
                    {
                        NpgsqlCommand command =
                            new NpgsqlCommand("INSERT INTO public.movie(movie_id, title, description, title_original)" +
                            " VALUES (:movie_id, :title, :description, :title_original) ON CONFLICT (movie_id) DO UPDATE SET last_update = NOW();",
                            connection);

                        command.Parameters.Add(new NpgsqlParameter("movie_id", NpgsqlTypes.NpgsqlDbType.Integer));
                        command.Parameters.Add(new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Text));
                        command.Parameters.Add(new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text));
                        command.Parameters.Add(new NpgsqlParameter("title_original", NpgsqlTypes.NpgsqlDbType.Text));

                        command.Parameters[0].Value = m.MovieId;
                        command.Parameters[1].Value = m.Title;
                        command.Parameters[2].Value = m.Description;
                        command.Parameters[3].Value = m.OriginalTitle;

                        command.ExecuteNonQuery();
                    }
                }
                catch(Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                
                tran.Commit();                
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCrew()
        {
            using (var connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString))
            {
                connection.Open();
                NpgsqlTransaction tran = connection.BeginTransaction();

                try
                {
                    foreach (var p in this.CrewList)
                    {
                        NpgsqlCommand command =
                            new NpgsqlCommand("INSERT INTO public.movie_crew(movie_id, crew_id, job)" +
                            " VALUES (:movie_id, :crew_id, :job) ON CONFLICT (movie_id, crew_id) do NOTHING RETURNING movie_id;",
                            connection);

                        command.Parameters.Add(new NpgsqlParameter("movie_id", NpgsqlTypes.NpgsqlDbType.Integer));
                        command.Parameters.Add(new NpgsqlParameter("crew_id", NpgsqlTypes.NpgsqlDbType.Integer));
                        command.Parameters.Add(new NpgsqlParameter("job", NpgsqlTypes.NpgsqlDbType.Text));

                        command.Parameters[0].Value = this.MovieId;
                        command.Parameters[1].Value = p.PersonId;
                        command.Parameters[2].Value = p.Job;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                
                tran.Commit();
                connection.Close();
            }
        }
    }

    [DataContract()]
    public class MoviesCollection
    {
        [DataMember(Name = "results")]
        public IEnumerable<Movie> Items { get; set; }
    }
}
