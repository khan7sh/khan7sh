using System;

//this one helps with permissions to give right access

namespace BasicVendorInventoryPlatform.Models
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        ViewVendors = 1 << 0,
        CreateVendor = 1 << 1,
        EditVendor = 1 << 2,
        DeleteVendor = 1 << 3,
        ViewProducts = 1 << 4,
        CreateProduct = 1 << 5,
        EditProduct = 1 << 6,
        DeleteProduct = 1 << 7,
        ViewReports = 1 << 8,
        ManageUsers = 1 << 9,
        ManageProducts = CreateProduct | EditProduct | DeleteProduct,
        ManageVendors = CreateVendor | EditVendor | DeleteVendor,
        GenerateReports = ViewReports
    }
}