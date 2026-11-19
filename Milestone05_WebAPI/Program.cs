using System.Text.Json.Serialization;
using Milestone05_WebAPI.Data;
using Milestone05_WebAPI.Repositories;
using Milestone05_WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new DatabaseContext("library.db"));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<GenreRepository>();
builder.Services.AddScoped<BookRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<BookService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<DatabaseContext>().Initialize();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
