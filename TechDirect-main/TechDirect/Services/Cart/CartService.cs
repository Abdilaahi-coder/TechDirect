using TechDirect.Models;

namespace TechDirect.Services.Cart
{
    public class CartService
    {
        public event Action? CartChanged;

        public List<Product> Items { get; private set; } = new List<Product>();

        public void AddToCart(Product product)
        {
            Items.Add(product);
            NotifyCartChanged();
        }

        public void RemoveFromCart(Product product)
        {
            Items.Remove(product);
            NotifyCartChanged();
        }

        public void ClearCart()
        {
            Items.Clear();
            NotifyCartChanged();
        }

        public decimal GetTotal()
        {
            return Items.Sum(p => p.Price);
        }

        private void NotifyCartChanged()
        {
            CartChanged?.Invoke();
        }
    }
}
