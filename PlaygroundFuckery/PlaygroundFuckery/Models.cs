using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyderSoft.POS.Models
{
    public record Location(Guid Id, string Description, string Name, string Address, string City, string Region, string PostalCode, string Country, bool IsRetailStore, int TotalUsersLicensed, bool IsLicenseValid);
    public record Item(Guid Id, string sku, DateOnly LastRestock, DateOnly LastOrder, IDictionary<string, int> InventoryCount, IDictionary<ItemTransaction, UserTransaction> InventoryTransaction);
    public record ItemTransaction(Guid Id, Guid ItemId, DateTime TransactionDateTime, string Action, InventoryDisposition StartingDisposition, InventoryDisposition EndingDisposition);
    //This is the equivelent of Circuit City Inventory Buckets.
    public record InventoryDisposition(Guid Id, string Description, bool IsPhysical, bool Count, bool CanSellFrom, bool CanUpdate, bool IsShrink);
    //This is used to locate the inventory in a location, example would be like pacer cats inventory locations for a different parts of the theater
    public record InventoryHub(Guid Id, string Description, Location PhysicalLocation, string ItemLocation);
    public record User(Guid Id, string UserName, UserPassword Password, IDictionary<SecurityPermission, UserTransaction> Permissions);
    public record UserPassword(Guid Id, string Password, int EncryptionType);
    public record SecurityPermission(Guid Id, string PermissionName, string Description, int MaxAssignablePerLocation, bool MustExpire, bool Expires, int ExpiresOn);
    public record UserTransaction(Guid Id, User PerformedBy, User PerformedTo, bool IsOverriden, DateTime PerformedOn, string ActionTaken);
}
