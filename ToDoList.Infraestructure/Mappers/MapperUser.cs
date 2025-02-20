using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Entities;

namespace ToDoList.Infraestructure.Mappers;

public class MapperUser : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
		builder.Property(c => c.Username).HasMaxLength(30).IsRequired();
		builder.Property(c => c.PasswordHash).IsRequired();
	}
}
