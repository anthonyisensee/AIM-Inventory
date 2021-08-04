// Anthony Isensee
// 7/30/21
// Much useful information pulled from https://www.youtube.com/watch?v=_JxC6EUxbDo&t=649s.
// This class acts as the middleman between our database and web server.

using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataHandler
{
    public class DataAccessAsync
    {

        // This helper class allows us to easily access data from our database.
        // ** Parameters **
        // * sql - A sql query to send to the database.
        // * parameters - Any unique parameters we would like to insert into the sql query string.
        // * connectionString - A connection string used to connect to the database.
        // ** Returns **
        // * A task whose job is to fetch a list of unique (T) objects.
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            // Creates a connection to our database, and will do its best to properly close the connection even if the service is interrupted.
            using (IDbConnection connection = new MySqlConnection(connectionString))    // Note that the connection string is used to open the MySQL connection.
            {
                // We get a list of a unique object (T) by querying the database with our sql and parameters.
                // Note that we await the succesful connection so that we can return the data in a list.
                var rows = await connection.QueryAsync<T>(sql, parameters);

                // Adds the data the task has found to a list and then returns it.
                return rows.ToList();
            }
        }

        // This helper class allows us to easily save data to our database.
        // ** Parameters **
        // * sql - A sql script to send to the database.
        // * parameters - Any unique parameters we would like to provide into the sql script.
        // * connectionString - A connection string used to connect to the database.
        // ** Returns **
        // * This returns an Async task. Basically, this allows the caller (the end point user) to wait for the data while our web server begin the task and move on.
        public Task SaveData<T>(string sql, T parameters, string connectionString)
        {
            // Creates a connection to our database, and will do its best to properly close the connection even if the service is interrupted.
            using (IDbConnection connection = new MySqlConnection(connectionString))    // Note that the connection string is used to open the MySQL connection.
            {
                // Executes the sql script with unique parameters in our database.
                
                return connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
