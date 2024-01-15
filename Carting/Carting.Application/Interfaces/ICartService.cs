using Carting.Carting.Domain.Entities;
using Carting.Carting.Domain.Models;

namespace Carting.Carting.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> Get(string id);
        Task<IEnumerable<Cart>> GetAll();
        Task<string> Create(Cart item);
        Task<string> Update(Cart item);
        Task Delete(string id);
        Task<string> AddItemToCart(string cartId, Item item);
        Task<string> RemoveItemFromCart(string cartId, int itemId);
        Task<IEnumerable<Item>> GetAllItems(string cartId);
        Task UpdateCartsGivenItem(Item item);

        Task<Item> GetCartsItem(int itemId);
    }
}
