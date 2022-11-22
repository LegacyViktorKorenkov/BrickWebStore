using Microsoft.AspNetCore.Identity;

namespace BrickWebStore.Models
{
    public class ShopUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
