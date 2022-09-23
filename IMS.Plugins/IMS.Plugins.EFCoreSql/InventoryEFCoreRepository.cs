using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSql
{
    public class InventoryEFCoreRepository : IInventoryRepository
    {
        private readonly IMSContext db;

        public InventoryEFCoreRepository(IMSContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            return await this.db.Inventories.Where(x =>
                x.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            this.db.Inventories.Add(inventory);

            await db.SaveChangesAsync();
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
            var inv = await this.db.Inventories.FindAsync(inventoryId);

            if (inv != null)
            {
                return inv;
            }

            return new Inventory();
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            var inv = await this.db.Inventories.FindAsync(inventory.InventoryId);

            if (inv != null)
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Price = inventory.Price;
                inv.Quantity = inventory.Quantity;

                await db.SaveChangesAsync();
            }

        }

    }
}