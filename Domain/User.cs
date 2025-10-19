
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}