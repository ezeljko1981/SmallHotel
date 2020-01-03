using System;
using Microsoft.EntityFrameworkCore;
using SmallHotelEntitiesLib;

namespace SmallHotelContextLib
{
    public class SmallHotelDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source = (localdb)\mssqllocaldb; Initial Catalog = SmallHotelDB; Integrated Security = True; Pooling = False");          
        }
    }
}
