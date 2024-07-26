using BasicVendorInventoryPlatform.Models;
using MessageBox = System.Windows.MessageBox;

namespace BasicVendorInventoryPlatform.Utilities
{
    public static class AuthorizationUtil
    {
        public static bool IsAdmin(string role)
        {
            return role == UserRoles.Admin;
        }

        public static bool IsEditor(string role)
        {
            return role == UserRoles.Editor || IsAdmin(role);
        }

        public static bool IsRegularUser(string role)
        {
            return role == UserRoles.Regular;
        }

        public static bool CanEditVendors(string role)
        {
            return IsEditor(role) || IsAdmin(role);
        }
    }
}