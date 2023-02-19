using Bargreen.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bargreen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private IInventoryService inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        [Route("InventoryBalances")]
        [HttpGet]
        public async Task<IEnumerable<InventoryBalance>> GetInventoryBalances()
        {
            return await inventoryService.GetInventoryBalancesAsync();
        }

        [Route("AccountingBalances")]
        [HttpGet]
        public async Task<IEnumerable<AccountingBalance>> GetAccountingBalances()
        {
            return await inventoryService.GetAccountingBalancesAsync();
        }

        [Route("InventoryReconciliation")]
        [HttpGet]
        public async Task<IEnumerable<InventoryReconciliationResult>> GetReconciliation()
        {
            Task<IEnumerable<InventoryBalance>> inventoryBalances = inventoryService.GetInventoryBalancesAsync();
            Task<IEnumerable<AccountingBalance>> accountingBalances = inventoryService.GetAccountingBalancesAsync();
            await Task.WhenAll(inventoryBalances, accountingBalances);
            return await inventoryService.ReconcileInventoryToAccountingAsync(inventoryBalances.Result, accountingBalances.Result);
        }
    }
}