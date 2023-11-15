using Carting.Carting.Domain.Entities;
using Carting.Carting.Domain.Models;

namespace Carting.Carting.Services.Interfaces
{
    public interface ICartService
    {
        Cart Get(int id);
        IEnumerable<Cart> GetAll();
        int Create(Cart item);
        void Update(Cart item);
        void Delete(int id);
        int AddItemToCart(int cartId, Item item);
        int RemoveItemFromCart(int cartId, int itemId);
        IEnumerable<Item> GetAllItems(int cartId);
    }
}
