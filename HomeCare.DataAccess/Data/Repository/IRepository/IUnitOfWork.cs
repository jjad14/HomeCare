using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCare.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }

        void Save();
    }
}
