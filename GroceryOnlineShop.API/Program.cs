using GroceryOnlineShop.API.Database;
using GroceryOnlineShop.API.EntityAutomappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var iMapper = MappingProfile.RegisterMapping().CreateMapper();
builder.Services.AddSingleton(iMapper);

builder.Services.AddSingleton<GroceryShopDatabase>();
builder.Services.AddSingleton<GroceryShopPurchaseOrderProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var gdb = app.Services.GetRequiredService(typeof(GroceryShopDatabase)) as GroceryShopDatabase;
if (gdb != null) await gdb.Seed();

app.Run();
