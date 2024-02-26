using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.RedisServices;
using RedisExampleApp.API.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductRepository>(sp =>
{
    var appDbContext=sp.GetRequiredService<AppDbContext>();
    var productRepository=new ProductRepository(appDbContext);
    //var redisService= sp.GetRequiredService(typeof(RedisService));
    var redisService = sp.GetRequiredService<RedisService>();
    var dataBase = sp.GetRequiredService<IDatabase>();
    return new ProductRepositoryWithCacheDecorator(productRepository, redisService, dataBase);

});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("myDataBase");
});
builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

builder.Services.AddSingleton<IDatabase>(sp =>
{
    var redisService=sp.GetService<RedisService>();
    return redisService.GetDatabase(1);
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext=scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
