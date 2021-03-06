﻿using HomeCare.DataAccess.Data.Repository.IRepository;
using HomeCare.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCare.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db)
            :base(db)
        {
            _db = db;
        }

        public void ChangeOrderStatus(int orderId, string status)
        {
            var objFromDb = _db.OrderHeader.FirstOrDefault(o => o.Id == orderId);

            objFromDb.Status = status;

            _db.SaveChanges();
        }

    }
}
