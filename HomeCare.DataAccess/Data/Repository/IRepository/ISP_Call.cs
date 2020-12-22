using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCare.DataAccess.Data.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        // For operations where you just want to execute something to the database and you do not want to retrieve anything
        void ExecuteNoReturn(string procedureName, DynamicParameters param = null);

        // Retrieve one complete row or one complete record
        T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null);

        // Retrieve all rows
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);
    }
}
