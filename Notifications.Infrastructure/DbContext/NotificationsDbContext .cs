using Microsoft.EntityFrameworkCore;
using Notifications.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Infrastructure
{
    public class NotificationsDbContext : DbContext         
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : base(options)
        {
        }

        public DbSet<Notfication> Notifications => Set<Notfication>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notfication>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message).IsRequired().HasMaxLength(500);
                entity.Property(n => n.UserId).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
