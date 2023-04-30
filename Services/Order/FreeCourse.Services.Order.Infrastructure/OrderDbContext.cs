using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        /*public override int SaveChanges()
        {
            //eventler burada fırlatılabilir.

            return base.SaveChanges();
        }*/
        public DbSet<Domain.OrderAggragate.Order> Orders { get; set;}
        public DbSet<Domain.OrderAggragate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggragate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggragate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

            modelBuilder.Entity<Domain.OrderAggragate.OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

            //Shadow property
            modelBuilder.Entity<Domain.OrderAggragate.Order>().OwnsOne(o => o.Address).WithOwner();


            base.OnModelCreating(modelBuilder);
        }
    }
}
