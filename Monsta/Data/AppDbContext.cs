﻿using Microsoft.EntityFrameworkCore;
using Monsta.Models;

namespace Monsta.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }
}
