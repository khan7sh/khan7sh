using System;

namespace BasicVendorInventoryPlatform.Models
{
    

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Permissions UserPermissions { get; set; }

        public bool HasPermission(Permissions permission)
        {
            return (UserPermissions & permission) == permission;
        }

        public bool IsAdmin()
        {
            return HasPermission(Permissions.ManageUsers);
        }
    }
}