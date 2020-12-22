using HomeCare.DataAccess.Data.Repository.IRepository;
using HomeCare.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCare.DataAccess.Data.Repository
{
    public class WebImageRepository : Repository<WebImages>, IWebImageRepository
    {
        private readonly ApplicationDbContext _db;

        public WebImageRepository(ApplicationDbContext db)
            :base(db)
        {
            _db = db;
        }

        public void Update(WebImages webImage, int flag)
        {
            var objFromDb = _db.WebImages.FirstOrDefault(m => m.Id == webImage.Id);

            objFromDb.Name = webImage.Name;

            if (flag == 1)
            {
                objFromDb.Image = webImage.Image;
            }

            _db.SaveChanges();
        }

    }
}
