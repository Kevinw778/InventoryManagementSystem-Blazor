using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Inventories
{
    public class ViewInventoryByIdUseCase : IViewInventoryByIdUseCase
    {
        private readonly IInventoryRepository inventoryRepository;

        public ViewInventoryByIdUseCase(IInventoryRepository inventory)
        {
            this.inventoryRepository = inventory;
        }

        public async Task<Inventory> ExecuteAsync(int inventoryId)
        {
            return await this.inventoryRepository.GetInventoryByIdAsync(inventoryId);
        }
    }
}
