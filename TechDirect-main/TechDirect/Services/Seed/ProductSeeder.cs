using TechDirect.Data;
using TechDirect.Models;

namespace TechDirect.Services.Seed
{
    public static class ProductSeeder
    {
        public static void SeedProducts(ApplicationDbContext context)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "SmartDevice 1",
                    Price = 499,
                    ImageUrl = "images/devices/smartdevice1.svg",
                    Description = "A powerful smart gadget."
                },
                new Product
                {
                    Name = "SmartDevice 2",
                    Price = 649,
                    ImageUrl = "images/devices/smartdevice2.svg",
                    Description = "A sleek device with great features."
                },
                new Product
                {
                    Name = "SmartDevice 3",
                    Price = 799,
                    ImageUrl = "images/devices/smartdevice3.svg",
                    Description = "Premium performance and design."
                },
                new Product
                {
                    Name = "SmartDevice 4",
                    Price = 949,
                    ImageUrl = "images/devices/smartdevice4.svg",
                    Description = "Top-tier device for tech lovers."
                }
            };

            foreach (var product in products)
            {
                var existing = context.Products.FirstOrDefault(p => p.Name == product.Name);
                if (existing is null)
                {
                    context.Products.Add(product);
                }
                else
                {
                    existing.Price = product.Price;
                    existing.ImageUrl = product.ImageUrl;
                    existing.Description = product.Description;
                }
            }

            context.SaveChanges();
        }
    }
}
