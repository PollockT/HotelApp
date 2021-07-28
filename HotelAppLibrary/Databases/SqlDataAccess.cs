using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public List<T> LoadData<T, U>(string sqlStatement,
                                      U parameters,
                                      string connectionStringName,
                                      dynamic options = null)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;

            if (options.IsStoredProcedure != null && options.IsStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection())
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                dynamic options = null)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;

            if (options.IsStoredProcedure != null && options.IsStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
