namespace Infrastructure.AppDbContext;

public class TodoListDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ToDoTask> Tasks { get; set; }

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedNever();

        // Task
        modelBuilder.Entity<ToDoTask>().Property(task => task.Id).ValueGeneratedNever();

        // Relations
        modelBuilder.Entity<ToDoTask>().HasOne(task => task.AssignedTo).WithMany().HasForeignKey(task => task.UserAssignedId).IsRequired();


        base.OnModelCreating(modelBuilder);
    }
}