using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext {
	public DbSet<User> Users { get; set; }
	public DbSet<Goal> Goals { get; set; }
	public DbSet<Task> Tasks { get; set; }
	public DbSet<Notes> Notes { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) {
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<User>().ToTable("user");
		modelBuilder.Entity<Goal>().ToTable("goal");
		modelBuilder.Entity<Task>().ToTable("task");
		modelBuilder.Entity<Notes>().ToTable("notes");
	}
}
