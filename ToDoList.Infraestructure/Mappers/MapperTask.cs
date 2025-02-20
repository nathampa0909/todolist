using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Task = ToDoList.Core.Entities.Task;

namespace ToDoList.Infraestructure.Mappers;

public class MapperTask : IEntityTypeConfiguration<Task>
{
	public void Configure(EntityTypeBuilder<Task> builder)
	{
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Title).HasMaxLength(50).IsRequired();
		builder.Property(c => c.Description).HasMaxLength(500).IsRequired();
		builder.Property(c => c.CreatedDate).IsRequired();
		builder.Property(c => c.FinishDate);
		builder.Property(c => c.Status).IsRequired();
		builder.Property(c => c.UserId).IsRequired();
		builder.HasOne(u => u.User)
			   .WithMany(t => t.Tasks)
			   .HasForeignKey(t => t.UserId);
	}
}
