using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repositories.Data
{
    public static class StoreDBContextSeed
    {
        public static async Task SeedAsync(StoreDBContext _context)
        {
           

            //ProductCategory/////////////////////
            if (_context.Types.Count() == 0)
            {
                var productCategoryData = File.ReadAllText("../Talabat.Repositories/Data/DataSeeding/categories.json");
                //Console.WriteLine(productData);
                var productsCategory = JsonSerializer.Deserialize<List<ProductType>>(productCategoryData);


                if (productsCategory?.Count() > 0)
                {
                    foreach (var product in productsCategory)
                    {
                        _context.Set<ProductType>().Add(product);
                    }
                    await _context.SaveChangesAsync();

                }
            }

            ////Product/////////////////////
            if (_context.Products.Count()==0)
            {
                var productData = File.ReadAllText("../Talabat.Repositories/Data/DataSeeding/products.json");
                //Console.WriteLine(productData);
                var products = JsonSerializer.Deserialize<List<Product>>(productData);


                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        _context.Set<Product>().Add(product);
                    }
                    await _context.SaveChangesAsync();

                }
            }

   


        }
    }
}
