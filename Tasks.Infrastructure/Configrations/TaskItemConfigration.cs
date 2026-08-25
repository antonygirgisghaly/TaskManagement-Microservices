using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Domain.Entities;

namespace Tasks.Infrastructure.Configrations
{
    internal class TaskItemConfigration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(t => t.AssignedToUserId).IsRequired();
        }
    }
}
