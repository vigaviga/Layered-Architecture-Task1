using Carting.Carting.Application.Interfaces;
using Carting.Carting.Domain.Entities;
using Carting.Carting.Domain.Models;
using Carting.Carting.Services.Exceptions;
using Carting.Carting.Services.Interfaces;
using Carting.Carting.Shared;
using Microsoft.OpenApi.Validations;

namespace Carting.Carting.Services.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;

        public CartService(IRepository<Cart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<string> AddItemToCart(string cartId, Item itemToAdd)
        {
            var cart = await _cartRepository.GetById(cartId);

            if (cart == null)
            {
                cartId = Guid.NewGuid().ToString();
                var cartToAdd = new Cart(cartId);
                cartToAdd.Items.Add(itemToAdd);
                await Create(cartToAdd);
                return cartId;
            }

            var item = cart.Items.Find(i => i.Id == itemToAdd.Id);
            
            if(item != null)
            {
                throw new EntityWithGivenIdExistsException(Constants.ItemWithGivenIdExists);
            }

            cart.Items.Add(itemToAdd);
            _cartRepository.Update(cart);
            return cart.Id;
        }

        public async Task<string> Create(Cart cart)
        {
            bool cartExists = await _cartRepository.GetById(cart.Id) != null;
            if (cartExists)
            {
                throw new EntityWithGivenIdExistsException(Constants.CartWithGivenIdExists);
            }

            return await _cartRepository.Add(cart);
        }

        public async Task Delete(string id)
        {
            await _cartRepository.Remove(id);
        }

        public async Task<Cart> Get(string id)
        {
            return await _cartRepository.GetById(id);
        }

        public async Task<IEnumerable<Item>> GetAllItems(string id)
        {
            var cart = await _cartRepository.GetById(id);
            return cart.Items;
        }

        public async Task<IEnumerable<Cart>> GetAll()
        {
            return await _cartRepository.GetAll();
        }

        public async Task<string> RemoveItemFromCart(string cartId, int itemId)
        {
            var cart = await _cartRepository.GetById(cartId);

            if(cart == null)
            {
                throw new EntityNotFoundException(Constants.CartNotFound);
            }
            
            cart.Items.RemoveAll(i => i.Id == itemId);

            _cartRepository.Update(cart);

            return cart.Id;
        }

        public async Task<string> Update(Cart cartToUpdate)
        {
            var cart = await _cartRepository.GetById(cartToUpdate.Id);

            if (cart == null)
            {
                throw new EntityNotFoundException(Constants.CartNotFound);
            }
            await _cartRepository.Update(cartToUpdate);
            return cart.Id;
        }

        public async Task UpdateCartsGivenItem(Item item)
        {
            var carts = await _cartRepository.GetAll();

            foreach (var cart in carts)
            {
                for(int i = 0; i < cart.Items.Count; i++)
                {
                    if (cart.Items[i].Id == item.Id)
                    {
                        cart.Items[i] = item;
                    }
                }
                await _cartRepository.Update(cart);
            }
        }

        public async Task<Item> GetCartsItem(int itemId)
        {
            var carts = await _cartRepository.GetAll();

            foreach (var cart in carts)
            {
                for (int i = 0; i < cart.Items.Count; i++)
                {
                    if (cart.Items[i].Id == itemId)
                    {
                        return await Task.FromResult(cart.Items[i]);
                    }
                }
            }
            return null;
        }
    }
}
