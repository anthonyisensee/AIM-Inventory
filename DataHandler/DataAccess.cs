using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace DataHandler
{
    public class DataAccess
    {
        // Summary:
        //      Allows us to quickly retrieve a list of unique objects designed to be mapped to from our database.
        // Parameters:
        //  T:
        //      A generic object to be stored in the list.
        //  sql:
        //      A SQL query string, generally a select statement.
        //  connectionString:
        //      A connection string containing the information needed to connect to a database.
        // Returns:
        //      A list of T generic objects each containing data pulled from the database.
        public List<T> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sql, parameters).ToList();

                return rows;
            }
        }

        // Summary:
        //      Allows us to quickly execute a MySQl query to our database with certain unique parameters.
        // Parameters:
        //  T:
        //      A generic object whose contents should be stored back to the database.
        //  sql:
        //      A string containing a SQL statement, generally an insert statement.
        //  parameters:
        //      Unique parameters to be used with the SQL string.
        //  connectionString:
        //      A connection string containing the information needed to connect to a database.
        public void SaveData<T>(string sql, T parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sql, parameters);
            }
        }
    }
}
