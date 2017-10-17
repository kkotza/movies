using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Serialization;
using System.Text;

namespace MoviesApp
{
    [DataContract()]
    public class Person
    {
        [DataMember(Name = "id")]
        public int PersonId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "job")]
        public string Job { get; set; }

        [DataMember(Name = "imdb_id")]
        public string ImdbLink { get; set; }

        public static void UpdateMany(IEnumerable<Person> persons)
        {
            using (var connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString))
            {
                connection.Open();
                NpgsqlTransaction tran = connection.BeginTransaction();

                try
                {
                    foreach (var p in persons)
                    {
                        NpgsqlCommand command =
                            new NpgsqlCommand("INSERT INTO public.crew(crew_id, name, imdb_profile)" +
                            " VALUES (:crew_id, :name, :imdb_profile) ON CONFLICT (crew_id) DO UPDATE SET imdb_profile = :imdb_profile;",
                            connection);

                        command.Parameters.Add(new NpgsqlParameter("crew_id", NpgsqlTypes.NpgsqlDbType.Integer));
                        command.Parameters.Add(new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Text));
                        command.Parameters.Add(new NpgsqlParameter("imdb_profile", NpgsqlTypes.NpgsqlDbType.Text));

                        command.Parameters[0].Value = p.PersonId;
                        command.Parameters[1].Value = p.Name;
                        command.Parameters[2].Value = p.ImdbLink;

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
}
