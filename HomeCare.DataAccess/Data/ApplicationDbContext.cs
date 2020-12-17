﻿using System;
using System.Collections.Generic;
using System.Text;
using HomeCare.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {   
        }

        public DbSet<Category> Category { get; set; }
    }
}