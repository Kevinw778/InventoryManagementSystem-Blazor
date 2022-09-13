using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product { ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150 },
                new Product { ProductId = 2, ProductName = "Car", Quantity = 5, Price = 25000 }
            };
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await Task.FromResult(_products);
            }

            return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            // Give this new Product a valid new Id
            var maxId = _products.Max(x => x.ProductId);
            product.ProductId = maxId + 1;

            _products.Add(product);

            return Task.CompletedTask;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var product = _products.FirstOrDefault(x => x.ProductId == productId);
            var newProduct = new Product();
            if (product != null)
            {
                newProduct.ProductId = product.ProductId;
                newProduct.ProductName = product.ProductName;
                newProduct.Price = product.Price;
                newProduct.Quantity = product.Quantity;

                newProduct.ProductInventories = new();
                if (product.ProductInventories != null && product.ProductInventories.Count > 0)
                {
                    foreach (var loopProductInventory in product.ProductInventories)
                    {
                        var newLoopProductInventory = new ProductInventory
                        {
                            InventoryId = loopProductInventory.InventoryId,
                            Inventory = new Inventory(),
                            ProductId = loopProductInventory.ProductId,
                            Product = product,
                            InventoryQuantity = loopProductInventory.InventoryQuantity
                        };
                        if (loopProductInventory.Inventory != null)
                        {
                            newLoopProductInventory.Inventory.InventoryId = loopProductInventory.Inventory.InventoryId;
                            newLoopProductInventory.Inventory.InventoryName = loopProductInventory.Inventory.InventoryName;
                            newLoopProductInventory.Inventory.Price = loopProductInventory.Inventory.Price;
                            newLoopProductInventory.Inventory.Quantity = loopProductInventory.Inventory.Quantity;
                        }

                        newProduct.ProductInventories.Add(newLoopProductInventory);
                    }
                }
            }

            return await Task.FromResult(newProduct);
        }
    }
}
