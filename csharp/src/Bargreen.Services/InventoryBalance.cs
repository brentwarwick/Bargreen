namespace Bargreen.Services
{
    public class InventoryBalance
    {
        public string ItemNumber { get; set; }
        public string WarehouseLocation { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal PricePerItem { get; set; }
        public decimal TotalValue => PricePerItem * QuantityOnHand;
    }
}
