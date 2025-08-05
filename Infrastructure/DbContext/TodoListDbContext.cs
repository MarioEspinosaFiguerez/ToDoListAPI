namespace Infrastructure.AppDbContext;

public class TodoListDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ToDoTask> Tasks { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<State> States { get; set; }

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedNever();

            entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
            entity.Property(u => u.FirstSurname).IsRequired().HasMaxLength(100);
            entity.Property(u => u.SecondSurname).HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(u => u.BirthDate).HasColumnType("date").IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(u => u.UpdatedAt).HasDefaultValueSql("GETDATE()");

            entity.Property(u => u.JwtTokenVersion).IsRequired().HasDefaultValue(1);
            entity.Property(u => u.RefreshTokenHashed).HasMaxLength(512);
            entity.Property(u => u.RefreshTokenExpiresAt);
            entity.Property(u => u.PasswordChangedAt);
        });

        // Task
        modelBuilder.Entity<ToDoTask>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedNever();

            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(2000);
            entity.Property(t => t.Deadline).IsRequired();
            entity.Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(t => t.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // Priority
        modelBuilder.Entity<Priority>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
        });

        // State
        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(50);
        });

        // Relations
        modelBuilder.Entity<ToDoTask>().HasOne(task => task.AssignedTo).WithMany(user => user.TasksAssigned).HasForeignKey(task => task.UserAssignedId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ToDoTask>().HasOne(task => task.Priority).WithMany(prio => prio.ToDoTasks).HasForeignKey(task => task.PriorityId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ToDoTask>().HasOne(task => task.State).WithMany(stat => stat.ToDoTasks).HasForeignKey(task => task.StateId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Seed Data for Lookup Tables
        modelBuilder.Entity<Priority>().HasData(
            new Priority { Id = 1, Name = "Low" },
            new Priority { Id = 2, Name = "Medium" },
            new Priority { Id = 3, Name = "High" }
        );

        modelBuilder.Entity<State>().HasData(
            new State { Id = 1, Name = "Pending" },
            new State { Id = 2, Name = "InProgress" },
            new State { Id = 3, Name = "Completed" }
        );

        base.OnModelCreating(modelBuilder);
    }
}