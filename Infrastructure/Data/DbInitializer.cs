using Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public static class DbInializer
    {
        public static async Task InitializeAsync(
        AppDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
        {
        
            await context.Database.MigrateAsync();

        
            var roles = new[] { "Admin", "Client" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to create role '{roleName}': {errors}");
                    }
                }
            }

          
            if (!userManager.Users.Any())
            {
                var admin = new User
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    EmailConfirmed = true
                };

                var r1 = await userManager.CreateAsync(admin, "Admin123!");
                if (!r1.Succeeded)
                {
                    var errors = string.Join("; ", r1.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }

                var rr = await userManager.AddToRoleAsync(admin, "Admin");
                if (!rr.Succeeded)
                {
                    var errors = string.Join("; ", rr.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to add admin to role: {errors}");
                }

                var client = new User
                {
                    UserName = "client@test.com",
                    Email = "client@test.com"
                };

                var r2 = await userManager.CreateAsync(client, "Client123!");
                if (!r2.Succeeded)
                {
                    var errors = string.Join("; ", r2.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create client user: {errors}");
                }

                var r3 = await userManager.AddToRoleAsync(client, "Client");
                if (!r3.Succeeded)
                {
                    var errors = string.Join("; ", r3.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to add client to role: {errors}");
                }
            }

          
            if (!context.Hotels.Any())
            {
            
                var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Name = "Grand Hotel",
                    City = "Kyiv",
                    Address = "Main St. 12",
                    Description = "Luxurious hotel in the center of Kyiv"
                },
                new Hotel
                {
                    Name = "Seaside Resort",
                    City = "Odesa",
                    Address = "Beach Ave. 45",
                    Description = "Cozy resort with sea view"
                },
                new Hotel
                {
                    Name = "Mountain Inn",
                    City = "Lviv",
                    Address = "Hill Rd. 7",
                    Description = "Charming inn near the mountains"
                },
                new Hotel
                {
                    Name = "Metro Palace",
                    City = "Kharkiv",
                    Address = "Central Blvd. 3",
                    Description = "Modern hotel near metro and business center"
                },
                new Hotel
                {
                    Name = "River View",
                    City = "Dnipro",
                    Address = "Riverbank St. 21",
                    Description = "Comfortable hotel with a view on the river"
                }
            };

                await context.Hotels.AddRangeAsync(hotels);
                await context.SaveChangesAsync(); // отримуємо Id для кожного

                // Тепер для кожного готелю додамо 3-4 номера
                var rooms = new List<Room>();

                // Grand Hotel (3 номера)
                var gh = hotels[0];
                rooms.AddRange(new[]
                {
                new Room { PricePerNight = 150, RoomNumber = "101", Capacity = 2, HotelId = gh.Id, Hotel = gh },
                new Room { PricePerNight = 200, RoomNumber = "102", Capacity = 3, HotelId = gh.Id, Hotel = gh },
                new Room { PricePerNight = 300, RoomNumber = "201", Capacity = 4, HotelId = gh.Id, Hotel = gh },
            });

                
                var sr = hotels[1];
                rooms.AddRange(new[]
                {
                new Room { PricePerNight = 100, RoomNumber = "1", Capacity = 2, HotelId = sr.Id, Hotel = sr },
                new Room { PricePerNight = 130, RoomNumber = "2", Capacity = 2, HotelId = sr.Id, Hotel = sr },
                new Room { PricePerNight = 180, RoomNumber = "3", Capacity = 3, HotelId = sr.Id, Hotel = sr },
                new Room { PricePerNight = 250, RoomNumber = "4", Capacity = 4, HotelId = sr.Id, Hotel = sr },
            });

               
                var mi = hotels[2];
                rooms.AddRange(new[]
                {
                new Room { PricePerNight = 90, RoomNumber = "101", Capacity = 2, HotelId = mi.Id, Hotel = mi },
                new Room { PricePerNight = 120, RoomNumber = "102", Capacity = 3, HotelId = mi.Id, Hotel = mi },
                new Room { PricePerNight = 160, RoomNumber = "201", Capacity = 4, HotelId = mi.Id, Hotel = mi },
            });

                
                var mp = hotels[3];
                rooms.AddRange(new[]
                {
                new Room { PricePerNight = 110, RoomNumber = "201", Capacity = 2, HotelId = mp.Id, Hotel = mp },
                new Room { PricePerNight = 140, RoomNumber = "202", Capacity = 2, HotelId = mp.Id, Hotel = mp },
                new Room { PricePerNight = 170, RoomNumber = "301", Capacity = 3, HotelId = mp.Id, Hotel = mp },
                new Room { PricePerNight = 220, RoomNumber = "302", Capacity = 4, HotelId = mp.Id, Hotel = mp },
            });

               
                var rv = hotels[4];
                rooms.AddRange(new[]
                {
                new Room { PricePerNight = 95, RoomNumber = "10", Capacity = 1, HotelId = rv.Id, Hotel = rv },
                new Room { PricePerNight = 125, RoomNumber = "11", Capacity = 2, HotelId = rv.Id, Hotel = rv },
                new Room { PricePerNight = 180, RoomNumber = "12", Capacity = 4, HotelId = rv.Id, Hotel = rv },
            });

                await context.Rooms.AddRangeAsync(rooms);
                await context.SaveChangesAsync();
            }
        }
    }
}
