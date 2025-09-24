using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RestaurantOrderingSystem.Models
{
    public class OrderingDbContext: DbContext   
    {

        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
        {
        }

        public DbSet<OrderDb> Orders { get; set; }
    }
}
