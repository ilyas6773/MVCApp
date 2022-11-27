using Microsoft.EntityFrameworkCore;
using MVCApp.Services;
using MVCAppData;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins, policy => {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddDbContext<HouseDataContext>(
    o=>o.UseNpgsql(builder.Configuration.GetConnectionString("HousesDb")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<HouseService>();
builder.Services.AddMemoryCache();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);


app.Run();
