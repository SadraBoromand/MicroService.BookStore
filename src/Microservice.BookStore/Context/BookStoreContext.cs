
using Microservice.BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microservice.BookStore.Context
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Seed user
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    FullName = "محمد صدرا برومند",
                    Phone = "09140286763",
                    Role = "admin",
                    Password = "sadra123",
                    Serial = Guid.NewGuid().ToString()
                }
                );
            #endregion
        }
    }
}
