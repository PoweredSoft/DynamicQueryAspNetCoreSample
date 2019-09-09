using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AcmeWeb.Dal
{
    public interface IHasLongId
    {
        long Id { get; set; }
    }

    public class MaxPlusOneValueGenerator<TEntity> : ValueGenerator<long>
        where TEntity : class, IHasLongId
    {

        private static MethodInfo generic;

        //statically set the MethodInfo for GetMaxKeyValue
        static MaxPlusOneValueGenerator()
        {
            var method = typeof(DbContext).GetMethod("Set");
            generic = method.MakeGenericMethod(typeof(TEntity));
        }

        //determines if values are overrided by the database 
        //(since this class is use for testing, values are not 
        //written to the database)
        public override bool GeneratesTemporaryValues => false;

        /// <summary>
        /// Gets the next value for the ID of the entity. The
        /// next value is always 1 + current maximum
        /// </summary>
        /// <param name="entry">The entry that will be written to the test database</param>
        /// <returns>max plus 1</returns>
        public override long Next(EntityEntry entry)
        {
            var context = entry.Context;
            var qry = generic.Invoke(context, null) as DbSet<TEntity>;

            var max = 0L;

            if (qry.AsNoTracking().Count() != 0)
                max = qry.Select(x => x.Id).Max();

            //return max plus one
            return max + 1;
        }

    }

    public class Order : IHasLongId
    {
        public long Id { get; set; }
        public long OrderNum { get; set; }
        public DateTime Date { get; set; }
        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class Customer : IHasLongId
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }

    public class Item : IHasLongId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class OrderItem : IHasLongId
    {
        public long Id { get; set; }
        public long Quantity { get; set; }
        public decimal PriceAtTheTime { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }

    public class Ticket : IHasLongId
    {
        public long Id { get; set; }
        public string TicketType { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool IsHtml { get; set; }
        public string TagList { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Owner { get; set; }
        public string AssignedTo { get; set; }
        public int TicketStatus { get; set; }
        public DateTimeOffset CurrentStatusDate { get; set; }
        public string CurrentStatusSetBy { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTimeOffset LastUpdateDate { get; set; }
        public string Priority { get; set; }
        public bool AffectedCustomer { get; set; }
        public string Version { get; set; }
        public int ProjectId { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public decimal EstimatedDuration { get; set; }
        public decimal ActualDuration { get; set; }
        public DateTimeOffset TargetDate { get; set; }
        public DateTimeOffset ResolutionDate { get; set; }
        public int Type { get; set; }
        public int ParentId { get; set; }
        public string PreferredLanguage { get; set; }
    }

    public class Task : IHasLongId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public int AssignedLogId { get; set; }
        public int IsAssigned { get; set; }
        public int OTSTaskTypeId { get; set; }
        public string Comment { get; set; }
        public DateTime LogComplete { get; set; }
        public bool IsLogComplete { get; set; }
        public bool ActualComplete { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedCompleteStartDate { get; set; }
        public bool IsComplete { get; set; }
        public bool Unplanned { get; set; }
        public int UnplannedCodeId { get; set; }
        public string UnplannedComments { get; set; }
        public bool IsImport { get; set; }
        public bool IsArchived { get; set; }
        public int OTSProjectId { get; set; }
        public int BarrierTypeId { get; set; }
        public bool IsBarrier { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class AcmeContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        public AcmeContext()
        {

        }

        public AcmeContext(DbContextOptions<AcmeContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            
            modelBuilder.Entity<Customer>().Property(t => t.Id).HasValueGenerator<MaxPlusOneValueGenerator<Customer>>();
            modelBuilder.Entity<Item>().Property(t => t.Id).HasValueGenerator<MaxPlusOneValueGenerator<Item>>();
            modelBuilder.Entity<Order>().Property(t => t.Id).HasValueGenerator<MaxPlusOneValueGenerator<Order>>();
            modelBuilder.Entity<OrderItem>().Property(t => t.Id).HasValueGenerator<MaxPlusOneValueGenerator<OrderItem>>();
            modelBuilder.Entity<Ticket>().Property(p => p.Id).HasValueGenerator<MaxPlusOneValueGenerator<Ticket>>();
            modelBuilder.Entity<Task>().Property(t => t.Id).HasValueGenerator<MaxPlusOneValueGenerator<Task>>();


            modelBuilder.Entity<Customer>().HasData(this.GetCustomerSeed());
            modelBuilder.Entity<Item>().HasData(this.GetItemsSeed());
            modelBuilder.Entity<Order>().HasData(this.GetOrdersSeed());
            modelBuilder.Entity<OrderItem>().HasData(this.GetOrderItemsSeed());
            modelBuilder.Entity<Ticket>().HasData(this.GetTicketsSeed());
            modelBuilder.Entity<Task>().HasData(this.GetTaskSeed());
        }

        private Task[] GetTaskSeed()
        {
            var faker = new Bogus.Faker<Task>()
                    .RuleFor(t => t.Name, (f, u) => f.Lorem.Sentence())
                    .RuleFor(t => t.OrderNumber, (f, u) => f.Random.Number(10000000))
                    .RuleFor(t => t.AssignedLogId, (f, u) => f.Random.Number(10000000))
                    .RuleFor(t => t.IsAssigned, (f, u) => f.PickRandom(0, 1))
                    .RuleFor(t => t.OTSTaskTypeId, (f, u) => f.PickRandom(1, 2, 3, 4))
                    .RuleFor(t => t.Comment, (f) => f.Lorem.Paragraph())
                    .RuleFor(t => t.LogComplete, f => f.Date.Soon(5))
                    .RuleFor(t => t.IsLogComplete, f => f.PickRandom(true, false))
                    .RuleFor(t => t.ActualComplete, f => f.PickRandom(true, false))
                    .RuleFor(t => t.PlannedStartDate, f => f.Date.Soon(5))
                    .RuleFor(t => t.PlannedCompleteStartDate, f => f.Date.Soon(5))
                    .RuleFor(t => t.IsComplete, f => f.PickRandom(true, false))
                    .RuleFor(t => t.Unplanned, f => f.PickRandom(true, false))
                    .RuleFor(t => t.UnplannedCodeId, f => f.Random.Number(10000000))
                    .RuleFor(t => t.UnplannedComments, f => f.Lorem.Paragraph())
                    .RuleFor(t => t.IsImport, f => f.PickRandom(true, false))
                    .RuleFor(t => t.IsArchived, f => f.PickRandom(true, false))
                    .RuleFor(t => t.OTSProjectId, f => f.Random.Number(10000000))
                    .RuleFor(t => t.BarrierTypeId, f => f.Random.Number(10000000))
                    .RuleFor(t => t.IsBarrier, f => f.PickRandom(true, false))
                    .RuleFor(t => t.IsActive, f => f.PickRandom(true, false))
                    .RuleFor(t => t.IsDeleted, f => f.PickRandom(true, false))
                    .RuleFor(t => t.CreatedBy, f => f.Person.FullName)
                    .RuleFor(t => t.CreatedDate, f => f.Date.Soon(5))
                    .RuleFor(t => t.LastModifiedDate, f => f.Date.Soon(5))
                    .RuleFor(t => t.LastModifiedBy, f => f.Person.FullName)
                ;

            var fakeModels = new List<Task>();
            for (var i = 0; i < 500; i++)
            {
                var t = faker.Generate();
                t.Id = i + 1;
                fakeModels.Add(t);
            }

            return fakeModels.ToArray();
        }

        private Ticket[] GetTicketsSeed()
        {
            var owners = new List<string>();

            for (var i = 0; i < 20; i++)
            {
                var faker2 = new Faker("en");
                var owner = faker2.Person.FullName;//.Generate();
                owners.Add(owner);
            }

            var faker = new Bogus.Faker<Ticket>()
                    .RuleFor(t => t.TicketType, (f, u) => f.PickRandom("new", "open", "refused", "closed"))
                    .RuleFor(t => t.Title, (f, u) => f.Lorem.Sentence())
                    .RuleFor(t => t.Details, (f, u) => f.Lorem.Paragraph())
                    .RuleFor(t => t.IsHtml, (f, u) => false)
                    .RuleFor(t => t.TagList, (f, u) => string.Join(",", f.Commerce.Categories(3)))
                    .RuleFor(t => t.CreatedDate, (f, u) => f.Date.Recent(100))
                    .RuleFor(t => t.Owner, (f, u) => f.PickRandom(owners))
                    .RuleFor(t => t.AssignedTo, (f, u) => f.Person.FullName)
                    .RuleFor(t => t.TicketStatus, (f, u) => f.PickRandom(1, 2, 3))
                    .RuleFor(t => t.LastUpdateBy, (f, u) => f.Person.FullName)
                    .RuleFor(t => t.LastUpdateDate, (f, u) => f.Date.Soon(5))
                    .RuleFor(t => t.Priority, (f, u) => f.PickRandom("low", "medium", "high", "critical"))
                    .RuleFor(t => t.AffectedCustomer, (f, u) => f.PickRandom(true, false))
                    .RuleFor(t => t.Version, (f, u) => f.PickRandom("1.0.0", "1.1.0", "2.0.0"))
                    .RuleFor(t => t.ProjectId, (f, u) => f.Random.Number(100))
                    .RuleFor(t => t.DueDate, (f, u) => f.Date.Soon(5))
                    .RuleFor(t => t.EstimatedDuration, (f, u) => f.Random.Number(20))
                    .RuleFor(t => t.ActualDuration, (f, u) => f.Random.Number(20))
                    .RuleFor(t => t.TargetDate, (f, u) => f.Date.Soon(5))
                    .RuleFor(t => t.ResolutionDate, (f, u) => f.Date.Soon(5))
                    .RuleFor(t => t.Type, (f, u) => f.PickRandom(1, 2, 3))
                    .RuleFor(t => t.ParentId, () => 0)
                    .RuleFor(t => t.PreferredLanguage, (f, u) => f.PickRandom("fr", "en", "es"))
                ;

            var fakeModels = new List<Ticket>();
            for (var i = 0; i < 500; i++)
            {
                var t = faker.Generate();
                t.Id = i + 1;
                fakeModels.Add(t);
            }

            return fakeModels.ToArray();
        }

        private OrderItem[] GetOrderItemsSeed()
        {
            return new[]
            {
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
            };
        }

        private Order[] GetOrdersSeed()
        {
            return new[]
            {
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
            };
        }

        private Item[] GetItemsSeed()
        {
            return new[]
            {
                new Item { Id = 1, Name = "Computer", Price = 1000M },
                new Item { Id = 2, Name = "Mice", Price = 25.99M },
                new Item { Id = 3, Name = "Keyboard", Price = 100M },
                new Item { Id = 4, Name = "Screen", Price = 499.98M },
                new Item { Id = 5, Name = "Printer", Price = 230.95M },
                new Item { Id = 6, Name = "HDMI Cables", Price = 20M },
                new Item { Id = 7, Name = "Power Cables", Price = 5.99M }
            };
        }

        private Customer[] GetCustomerSeed()
        {
            return new[] {
                new Customer() { Id = 1, FirstName = "David", LastName = "Lebee" },
                new Customer() { Id = 2, FirstName = "John", LastName = "Doe" },
                new Customer() { Id = 3, FirstName = "Chuck", LastName = "Norris" },
                new Customer() { Id = 4, FirstName = "Nelson", LastName = "Mendela" },
                new Customer() { Id = 5, FirstName = "Jimi", LastName = "Hendrix" },
                new Customer() { Id = 6, FirstName = "Axel", LastName = "Rose" },
                new Customer() { Id = 7, FirstName = "John", LastName = "Frusciante" },
                new Customer() { Id = 8, FirstName = "Michael", LastName = "Jackson" },
                new Customer() { Id = 9, FirstName = "Anita", LastName = "Franklin" }
            };
        }
    }
}
