using EFCore.Multitenant.Domain;
using EFCore.Multitenant.Provider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Multitenant.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Product> Products { get; set; }
        public readonly TenantData TenantData;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, TenantData tenant) : base(options)
        {
            TenantData = tenant;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema(TenantData.TenantId);

            modelBuilder.Entity<Person>()
                        .HasData(
                new Person { Id = 1, Name = "Person 1", TenantId = "tenant-1" },
                new Person { Id = 2, Name = "Person 2", TenantId = "tenant-2" },
                new Person { Id = 3, Name = "Person 3", TenantId = "tenant-2" });

            modelBuilder.Entity<Product>()
                        .HasData(
                new Product { Id = 1, Description = "Description 1", TenantId = "tenant-1" },
                new Product { Id = 2, Description = "Description 2", TenantId = "tenant-2" },
                new Product { Id = 3, Description = "Description 3", TenantId = "tenant-2" });

            //modelBuilder.Entity<Person>()
            //            .HasQueryFilter(person => person.TenantId.Equals(TenantData.TenantId));
            
            //modelBuilder.Entity<Product>()
            //            .HasQueryFilter(product => product.TenantId.Equals(TenantData.TenantId));
        }
    }
}
