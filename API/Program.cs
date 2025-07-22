var builder = WebApplication.CreateBuilder(args);

// To get custom configuration. If you dont want, just quit the options in AddSwaggerGen and this variables
var swaggerConfiguration = builder.Configuration.GetSection("SwaggerConfig");
(string swaggerTitle, string swaggerVersion) = (swaggerConfiguration.GetValue<string>("Title") ?? "API", swaggerConfiguration.GetValue<string>("Version") ?? "v1");

// Serilog Config
Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(swaggerVersion, new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = swaggerTitle,
        Version = swaggerVersion,
    });
});

builder.Services.AddDbContext<TodoListDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// Dependency Injection

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Fluent validation
//builder.Services.AddValidatorsFromAssemblyContaining<>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();