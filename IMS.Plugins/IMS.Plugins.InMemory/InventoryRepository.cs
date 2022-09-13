using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories = new List<Inventory>()
            {
                new Inventory { InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
                new Inventory { InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15 },
                new Inventory { InventoryId = 3, InventoryName = "Bike Wheel", Quantity = 20, Price = 8 },
                new Inventory { InventoryId = 4, InventoryName = "Bike Pedal", Quantity = 20, Price = 1 },
            };
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await Task.FromResult(_inventories);
            }

            return _inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(x => x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            // Give this new Inventory a valid new Id
            var maxId = _inventories.Max(x => x.InventoryId);
            inventory.InventoryId = maxId + 1;

            _inventories.Add(inventory);

            return Task.CompletedTask;
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            // Trying to rename to an existing inventory - cannot do this due to business logic
            if (_inventories.Any(x => x.InventoryId != inventory.InventoryId &&
                x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var inventoryToUpdate = _inventories.FirstOrDefault(x => x.InventoryId == inventory.InventoryId);

            if (inventoryToUpdate != null)
            {
                inventoryToUpdate.InventoryName = inventory.InventoryName;
                inventoryToUpdate.Price = inventory.Price;
                inventoryToUpdate.Quantity = inventory.Quantity;
            }

            return Task.CompletedTask;
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            // Separate variable is made to avoid editing original reference
            var inventory = _inventories.First(x => x.InventoryId == inventoryId);
            var newInventory = new Inventory
            {
                InventoryId = inventory.InventoryId,
                InventoryName = inventory.InventoryName,
                Price = inventory.Price,
                Quantity = inventory.Quantity
            };

            return await Task.FromResult(newInventory);
        }
    }
}