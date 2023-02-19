using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bargreen.Services
{
    public class InventoryService : IInventoryService
    {
        public Task<IEnumerable<InventoryBalance>> GetInventoryBalancesAsync()
        {
            return Task.Run(GetInventoryBalances);
        }

        public IEnumerable<InventoryBalance> GetInventoryBalances()
        {
            return new List<InventoryBalance>()
            {
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 312,
                     WarehouseLocation = "WLA1"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ABC123",
                     PricePerItem = 7.5M,
                     QuantityOnHand = 146,
                     WarehouseLocation = "WLA2"
                },
                new InventoryBalance()
                {
                     ItemNumber = "ZZZ99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 47,
                     WarehouseLocation = "WLA3"
                },
                new InventoryBalance()
                {
                     ItemNumber = "zzz99",
                     PricePerItem = 13.99M,
                     QuantityOnHand = 91,
                     WarehouseLocation = "WLA4"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxccM",
                     PricePerItem = 245.25M,
                     QuantityOnHand = 32,
                     WarehouseLocation = "WLA5"
                },
                new InventoryBalance()
                {
                     ItemNumber = "xxddM",
                     PricePerItem = 747.47M,
                     QuantityOnHand = 15,
                     WarehouseLocation = "WLA6"
                }
            };
        }

        public Task<IEnumerable<AccountingBalance>> GetAccountingBalancesAsync()
        {
            return Task.Run(GetAccountingBalances);
        }

        public IEnumerable<AccountingBalance> GetAccountingBalances()
        {
            return new List<AccountingBalance>()
            {
                new AccountingBalance()
                {
                     ItemNumber = "ABC123",
                     TotalInventoryValue = 3435M
                },
                new AccountingBalance()
                {
                     ItemNumber = "ZZZ99",
                     TotalInventoryValue = 1930.62M
                },
                new AccountingBalance()
                {
                     ItemNumber = "xxccM",
                     TotalInventoryValue = 7602.75M
                },
                new AccountingBalance()
                {
                     ItemNumber = "fbr77",
                     TotalInventoryValue = 17.99M
                }
            };
        }

        public Task<IEnumerable<InventoryReconciliationResult>> ReconcileInventoryToAccountingAsync(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            return Task.Run(() => ReconcileInventoryToAccounting(inventoryBalances, accountingBalances));
        }

        public static IEnumerable<InventoryReconciliationResult> ReconcileInventoryToAccounting(IEnumerable<InventoryBalance> inventoryBalances, IEnumerable<AccountingBalance> accountingBalances)
        {
            if (inventoryBalances == null) return GetAllAccountingBalances(accountingBalances);
            var query = inventoryBalances.GroupBy(x => x.ItemNumber);
            Dictionary<string, decimal> inventoryBalancesDict = GetInventoryDictionary(query);
            List<InventoryReconciliationResult> inventoryReconciliationResults = new List<InventoryReconciliationResult>();
            ReconcileInventoryToAccounting(accountingBalances, inventoryBalancesDict, inventoryReconciliationResults);
            ReconcileRemainingInventory(inventoryBalancesDict, inventoryReconciliationResults);
            return inventoryReconciliationResults;
        }

        private static Dictionary<string, decimal> GetInventoryDictionary(IEnumerable<IGrouping<string, InventoryBalance>> query)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>(query.Count());
            foreach (IGrouping<string, InventoryBalance> group in query)
            {
                result.Add(group.Key, group.Sum(x => x.TotalValue));
            }
            return result;
        }

        private static IEnumerable<InventoryReconciliationResult> GetAllAccountingBalances(IEnumerable<AccountingBalance> accountingBalances)
        {
#if ACCOUNTING_DUPLICATE_CHECKING
            HashSet<string> foundItemNumbers = new HashSet<string>();
#endif
            List<InventoryReconciliationResult> inventoryReconciliationResults = new List<InventoryReconciliationResult>();
            if (accountingBalances != null)
            {
                foreach (AccountingBalance accountingBalance in accountingBalances)
                {
#if ACCOUNTING_DUPLICATE_CHECKING
                    CheckForDuplicateAccountingItemNumber(foundItemNumbers, accountingBalance.ItemNumber);
#endif
                    inventoryReconciliationResults.Add(new InventoryReconciliationResult(accountingBalance.ItemNumber, 0, accountingBalance.TotalInventoryValue));
                }
            }
            return inventoryReconciliationResults;
        }

        private static void ReconcileInventoryToAccounting(IEnumerable<AccountingBalance> accountingBalances, Dictionary<string, decimal> inventoryBalancesDict, List<InventoryReconciliationResult> inventoryReconciliationResults)
        {
#if ACCOUNTING_DUPLICATE_CHECKING
            HashSet<string> foundItemNumbers = new HashSet<string>();
#endif
            foreach (AccountingBalance accountingBalance in accountingBalances)
            {
#if ACCOUNTING_DUPLICATE_CHECKING
                CheckForDuplicateAccountingItemNumber(foundItemNumbers, accountingBalance.ItemNumber);
#endif
                if (!inventoryBalancesDict.TryGetValue(accountingBalance.ItemNumber, out decimal inventoryTotalValue))
                {
                    inventoryReconciliationResults.Add(new InventoryReconciliationResult(accountingBalance.ItemNumber, 0, accountingBalance.TotalInventoryValue));
                }
                else
                {
                    if (accountingBalance.TotalInventoryValue != inventoryTotalValue)
                    {
                        inventoryReconciliationResults.Add(new InventoryReconciliationResult(accountingBalance.ItemNumber, inventoryTotalValue, accountingBalance.TotalInventoryValue));
                    }
                    inventoryBalancesDict.Remove(accountingBalance.ItemNumber);
                }
            }
        }

#if ACCOUNTING_DUPLICATE_CHECKING
        private static void CheckForDuplicateAccountingItemNumber(HashSet<string> foundItemNumbers, string itemNumber)
        {
            if (foundItemNumbers.Contains(itemNumber))
            {
                throw new System.Exception($"The item number {itemNumber} is duplicated in the AccountingBalances table.  Item numbers in this table must be unique.");
            }
            foundItemNumbers.Add(itemNumber);
        }
#endif

        private static void ReconcileRemainingInventory(Dictionary<string, decimal> inventoryBalancesDict, List<InventoryReconciliationResult> inventoryReconciliationResults)
        {
            foreach (KeyValuePair<string, decimal> keyValuePair in inventoryBalancesDict)
            {
                inventoryReconciliationResults.Add(new InventoryReconciliationResult(keyValuePair.Key, keyValuePair.Value, 0));
            }
        }
    }
}