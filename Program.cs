using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseMySql("Server=localhost;Database=planning_app;User=root;Password=password;",
	new MySqlServerVersion(new Version(8, 0, 37))));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.AddControllers();

builder.Services.AddCors(options => {
	options.AddPolicy("AllowSpecificOrigin", policy => {
		policy.WithOrigins("http://localhost:3000")
			  .AllowAnyHeader()
			  .AllowAnyMethod()
			  .AllowCredentials(); 
	});
});


var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
