using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private List<InventoryTransaction> _inventoryTransactions = new();
        private readonly IInventoryRepository inventoryRepository;

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            this._inventoryTransactions.Add(new InventoryTransaction
            {
                PONumber = poNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });
        }

        public void ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            this._inventoryTransactions.Add(new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.InventoryId,
                QuantityBefore = inventory.Quantity,
                ActivityType = InventoryTransactionType.ProduceProduct,
                QuantityAfter = inventory.Quantity - quantityToConsume,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(
            string inventoryName,
            DateTime? dateFrom,
            DateTime? dateTo,
            InventoryTransactionType? transactionType)
        {
            var inventories = (await inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

            var query = from transaction in this._inventoryTransactions
                        join inventory in inventories
                            on transaction.InventoryId equals inventory.InventoryId
                        where
                            (string.IsNullOrWhiteSpace(inventoryName) || inventory.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0)
                            &&
                            (!dateFrom.HasValue || transaction.TransactionDate >= dateFrom.Value.Date) &&
                            (!dateTo.HasValue || transaction.TransactionDate <= dateTo.Value.Date) &&
                            (!transactionType.HasValue || transaction.ActivityType == transactionType)
                        select new InventoryTransaction
                        {
                            Inventory = inventory,
                            InventoryTransactionId = transaction.InventoryTransactionId,
                            PONumber = transaction.PONumber,
                            InventoryId = transaction.InventoryId,
                            QuantityBefore = transaction.QuantityBefore,
                            QuantityAfter = transaction.QuantityAfter,
                            ActivityType = transaction.ActivityType,
                            TransactionDate = transaction.TransactionDate,
                            DoneBy = transaction.DoneBy,
                            UnitPrice = transaction.UnitPrice
                        };

            return query;
        }
    }
}
