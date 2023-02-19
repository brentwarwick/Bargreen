namespace Bargreen.Services
{
    public class InventoryReconciliationResult
    {
        public string ItemNumber { get; set; }
        public decimal TotalValueOnHandInInventory { get; set; }
        public decimal TotalValueInAccountingBalance { get; set; }

        public InventoryReconciliationResult(string itemNumber, decimal totalValueOnHandInInventory, decimal totalValueInAccountingBalance)
        {
            ItemNumber = itemNumber;
            TotalValueOnHandInInventory = totalValueOnHandInInventory;
            TotalValueInAccountingBalance = totalValueInAccountingBalance;
        }
    }
}
