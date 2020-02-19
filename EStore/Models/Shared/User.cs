using Microsoft.AspNetCore.Identity;

namespace EStore.Models.Shared
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
