using Server.Data;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddSqlite<GameDbContext>(connString);
    
    
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPlayerService,PlayerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.MigrateDb();

app.Run();