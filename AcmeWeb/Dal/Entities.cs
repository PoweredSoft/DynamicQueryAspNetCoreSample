using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeWeb.Dal
{
    public class Order
    {
        public long Id { get; set; }
        public long OrderNum { get; set; }
        public DateTime Date { get; set; }
        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }

    public class Item
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class OrderItem
    {
        public long Id { get; set; }
        public long Quantity { get; set; }
        public decimal PriceAtTheTime { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }

    public class AcmeContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public AcmeContext()
        {

        }

        public AcmeContext(DbContextOptions<AcmeContext> options)
            : base(options)
        {

        }

        public void Seed()
        {
            Customers.AddRange(
                new Customer() { Id = 1, FirstName = "David", LastName = "Lebee" },
                new Customer() { Id = 2, FirstName = "John", LastName = "Doe" },
                new Customer() { Id = 3, FirstName = "Chuck", LastName = "Norris" },
                new Customer() { Id = 4, FirstName = "Nelson", LastName = "Mendela" },
                new Customer() { Id = 5, FirstName = "Jimi", LastName = "Hendrix" },
                new Customer() { Id = 6, FirstName = "Axel", LastName = "Rose" },
                new Customer() { Id = 7, FirstName = "John", LastName = "Frusciante" },
                new Customer() { Id = 8, FirstName = "Michael", LastName = "Jackson" },
                new Customer() { Id = 9, FirstName = "Anita", LastName = "Franklin" }
            );

            Items.AddRange(
                new Item { Id = 1, Name = "Computer", Price = 1000M },
                new Item { Id = 2, Name = "Mice", Price = 25.99M },
                new Item { Id = 3, Name = "Keyboard", Price = 100M },
                new Item { Id = 4, Name = "Screen", Price = 499.98M },
                new Item { Id = 5, Name = "Printer", Price = 230.95M },
                new Item { Id = 6, Name = "HDMI Cables", Price = 20M },
                new Item { Id = 7, Name = "Power Cables", Price = 5.99M }
            );

            SaveChanges();

      
            Orders.AddRange(
                new Order()
                {
                    Id = 1,
                    OrderNum = 1000,
                    CustomerId = 1,
                    Date = new DateTime(2018, 1, 1)
                },
                new Order()
                {
                    Id = 2,
                    OrderNum = 1001,
                    CustomerId = 2,
                    Date = new DateTime(2018, 2, 1)
                },
                new Order()
                {
                    Id = 3,
                    OrderNum = 1002,
                    CustomerId = 3,
                    Date = new DateTime(2018, 2, 1)
                },
                new Order()
                {
                    Id = 4,
                    OrderNum = 1003,
                    CustomerId = 1,
                    Date = new DateTime(2018, 3, 1)
                }
            );

            SaveChanges();

            OrderItems.AddRange(
                  new OrderItem() { Id = 1, OrderId = 1, ItemId = 1, PriceAtTheTime = 1000M, Quantity = 1 },
                  new OrderItem() { Id = 2, OrderId = 1, ItemId = 2, PriceAtTheTime = 30M, Quantity = 1 },
                  new OrderItem() { Id = 3, OrderId = 1, ItemId = 4, PriceAtTheTime = 399.99M, Quantity = 2 },
                  new OrderItem() { Id = 4, OrderId = 1, ItemId = 6, PriceAtTheTime = 20, Quantity = 2 },
                  new OrderItem() { Id = 8, OrderId = 1, ItemId = 6, PriceAtTheTime = 3.99M, Quantity = 3 },

                  new OrderItem() { Id = 9, OrderId = 2, ItemId = 6, PriceAtTheTime = 20, Quantity = 2 },
                  new OrderItem() { Id = 10, OrderId = 2, ItemId = 6, PriceAtTheTime = 3.99M, Quantity = 3 },

                  new OrderItem() { Id = 11, OrderId = 3, ItemId = 5, PriceAtTheTime = 499.99M, Quantity = 1 },
                  new OrderItem() { Id = 12, OrderId = 3, ItemId = 6, PriceAtTheTime = 20, Quantity = 1 },
                  new OrderItem() { Id = 13, OrderId = 3, ItemId = 7, PriceAtTheTime = 3.99M, Quantity = 1 },

                  new OrderItem() { Id = 14, OrderId = 4, ItemId = 2, PriceAtTheTime = 50M, Quantity = 1 },
                  new OrderItem() { Id = 15, OrderId = 4, ItemId = 3, PriceAtTheTime = 75.50M, Quantity = 1 }
              );

            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

        }
    }
}
