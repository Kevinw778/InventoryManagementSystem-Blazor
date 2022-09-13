﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater or equal to 0")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater or equal to 0")]
        public double Price { get; set; }

        public List<ProductInventory> ProductInventories { get; set; } = new();

        public void AddInventory(Inventory inventory)
        {
            if (!this.ProductInventories.Any(x => x.Inventory != null &&
            x.Inventory.InventoryName.Equals(inventory.InventoryName)))
            {
                var productInventoryToAdd = new ProductInventory
                {
                    InventoryId = inventory.InventoryId,
                    Inventory = inventory,
                    ProductId = this.ProductId,
                    Product = this,
                    InventoryQuantity = 1
                };

                this.ProductInventories.Add(productInventoryToAdd);
            }
        }
    }
}
