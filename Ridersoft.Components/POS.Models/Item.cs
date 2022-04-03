namespace RyderSoft.POS.Models
{
    public record Location(Guid Id, string Description, string Name, string Address, string City, string Region, string PostalCode, string Country, bool IsRetailStore, int TotalUsersLicensed);
    public record Item(Guid Id, string sku, DateOnly LastRestock, DateOnly LastOrder, IDictionary<string, int> InventoryCount, IDictionary<ItemTransactionEntry, UserTransaction> InventoryTransaction);
    public record ItemTransactionEntry(Guid Id, string sku, DateTime TransactionDateTime, int Action, InventoryDisposition StartingDisposition, InventoryDisposition EndingDisposition);
    public record InventoryDisposition(Guid Id, string Description, bool IsPhysical, bool Count, bool CanSellFrom, bool CanUpdate, bool IsShrink);
    public record InventoryLocation(Guid Id, string Description, Location PhysicalLocation, string ItemLocation);
    public record User(Guid Id, string UserName, UserPassword Password, IDictionary<SecurityPermission, UserTransaction> Permissions);
    public record UserPassword(Guid Id, string Password, int EncryptionType);
    public record SecurityPermission(Guid Id, string PermissionName, string Description, int MaxAssignablePerLocation, bool MustExpire, bool Expires, int ExpiresOn);
    public record UserTransaction(Guid Id, User PerformedBy, User PerformedTo, bool IsOverriden, DateTime PerformedOn, string ActionTaken);
}