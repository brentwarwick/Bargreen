using Bargreen.API.Controllers;
using Bargreen.Services;
using System.Linq;
using Xunit;

namespace Bargreen.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public void Inventory_Reconciliation_Performs_As_Expected()
        {
            var controller = new InventoryController(new InventoryService());
            var result = controller.GetReconciliation();
            var results = result.Result;
            // Make sure values are not identical
            Assert.Empty(results.Where(x => x.TotalValueInAccountingBalance == x.TotalValueOnHandInInventory));
            // Make sure ItemNumbers are not duplicated
            Assert.Empty(results.GroupBy(x => x.ItemNumber).Where(y => y.Count() > 1));
        }
    }
}
