using Microsoft.EntityFrameworkCore;
using LocalTreeData.ApplicationInterfaces;
using LocalTreeData.Application;
using LocalTreeData.EfCoreInterfaces;
using LocalTreeData.EfCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
        });

});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<LocalTreeData.EfCore.AppContext>(
    options => options.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=local"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<ITreeRepository, TreeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseCors();
app.Run();
