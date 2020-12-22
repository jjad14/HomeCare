using HomeCare.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCare.DataAccess.Data.Repository.IRepository
{
    public interface IWebImageRepository : IRepository<WebImages>
    {
        void Update(WebImages webImage, int flag);

    }
}
