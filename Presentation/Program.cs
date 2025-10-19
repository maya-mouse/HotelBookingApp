using Application.Interfaces;
using Application.Mapper;
using Application.Services;
using AutoMapper;
using Domain;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.32"),
    mysqlOptions => 
        {
            // !!! ФІКС: ДОДАЄМО EnableRetryOnFailure !!!
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 15, 
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });

});
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
   options.Password.RequireDigit = true;
   options.Password.RequireUppercase = false;
   options.Password.RequireLowercase = false;
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
var mappingConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>(); 
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>(); 
builder.Services.AddScoped<IStatsRepository, StatsRepository>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IIdentityService, IdentityService>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        

        await DbInializer.InitializeAsync(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Помилка ініціалізації бази даних: {ex.Message}");
    }
}

app.Use(async (context, next) =>
{

    if (context.Request.Path == "/" && !context.Request.Path.Value.Contains('.'))
    {
        context.Response.Redirect("/Hotels/Index");
        return;
    }
    await next();
});

app.Run();