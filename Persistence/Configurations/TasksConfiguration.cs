using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class TasksConfiguration : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).HasMaxLength(100);
        builder.Property(x => x.Completed).HasDefaultValue(false);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.DueDate);
        builder.Property(x => x.TaskPriority).HasConversion<int>();
        
        builder.HasOne(x => x.User).WithMany(x => x.Tasks)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}