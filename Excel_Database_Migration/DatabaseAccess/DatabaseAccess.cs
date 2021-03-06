﻿using Excel_Database_Migration.SQLGeneration;
using System.Data;
using System.Data.SqlClient;

namespace Excel_Database_Migration.DatabaseAccess
{
    internal class DatabaseAccess
    {

        private readonly string _connectionString;

        /// <summary>
        /// Constructor overload in case user want's to connect to another server.
        /// </summary>
        /// <param name="connectionString"></param>
        public DatabaseAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Establish and open connection with SQL server.
        /// </summary>
        /// <returns></returns>
        private SqlConnection ConnectToSql() {

            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;

        }

        /// <summary>
        /// Responsible for Select queries. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Return a datatable of queries results</returns>
        public DataTable GetQuery(string query) {
            DataTable queryResult = null;
            using (SqlConnection connection = ConnectToSql()) {
                using (SqlDataAdapter adapter = new SqlDataAdapter()) {

                    try {
                        SqlCommand command = new SqlCommand(query, connection);
                        queryResult= new DataTable();

                        adapter.SelectCommand = command;
                        adapter.Fill(queryResult);
                    }
                    catch(System.Exception e)
                    {
                        System.Console.WriteLine(e.ToString());
                    }
                    finally {
                    }
                }
            }
            return queryResult;
        }

        /// <summary>
        /// Responsible for Delete, Insert, and Update queries.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Return the number of rows affected</returns>
        public int GetNonQuery(string query) {

            int nonQuery = -1;

            using (SqlConnection connection = ConnectToSql()) {

                SqlCommand command = new SqlCommand(query, connection);

                try {
                    nonQuery = command.ExecuteNonQuery();
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                }
                finally {  
                }

                return nonQuery;
            }
        }
    }
}