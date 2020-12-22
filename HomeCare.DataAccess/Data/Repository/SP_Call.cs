using Dapper;
using HomeCare.DataAccess.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCare.DataAccess.Data.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString = "";
        public SP_Call(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void ExecuteNoReturn(string procedureName, DynamicParameters param = null)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
            {
                mySqlConnection.Open();

                mySqlConnection.Execute(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
            {
                mySqlConnection.Open();

                return (T)Convert.ChangeType(mySqlConnection.ExecuteScalar<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(ConnectionString))
            {
                mySqlConnection.Open();

                return mySqlConnection.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
