using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public interface IInventoryService
    {
        IEnumerable<InventoryBalance> GetInventoryBalances();
        Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync();
        IEnumerable<AccountingBalance> GetAccountingBalances();
        Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync();
        Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccountingAsync(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances);
    }
}
