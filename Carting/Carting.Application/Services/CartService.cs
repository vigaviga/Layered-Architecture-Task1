using Carting.Carting.Domain.Entities;
using Carting.Carting.Domain.Models;
using Carting.Carting.Services.Exceptions;
using Carting.Carting.Services.Interfaces;
using Carting.Carting.Shared;
using Carting.DataAccessLayer.Interfaces;

namespace Carting.Carting.Services.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;

        public CartService(IRepository<Cart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public int AddItemToCart(int cartId, Item itemToAdd)
        {
            var cart = _cartRepository.GetById(cartId);

            if (cart == null)
            {
                throw new EntityNotFoundException(Constants.CartNotFound);
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

        public int Create(Cart item)
        {
            bool cartExists = _cartRepository.GetById(item.Id) != null;
            if (cartExists)
            {
                throw new EntityWithGivenIdExistsException(Constants.CartWithGivenIdExists);
            }

            return _cartRepository.Add(item);
        }

        public void Delete(int id)
        {
            _cartRepository.Remove(id);
        }

        public Cart Get(int id)
        {
            return _cartRepository.GetById(id);
        }

        public IEnumerable<Item> GetAllItems(int id)
        {
            return _cartRepository.GetById(id).Items;
        }

        public IEnumerable<Cart> GetAll()
        {
            return _cartRepository.GetAll();
        }

        public int RemoveItemFromCart(int cartId, int itemId)
        {
            var cart = _cartRepository.GetById(cartId);

            if(cart == null)
            {
                throw new EntityNotFoundException(Constants.CartNotFound);
            }
            
            cart.Items.RemoveAll(i => i.Id == itemId);

            _cartRepository.Update(cart);

            return cart.Id;
        }

        public void Update(Cart cartToUpdate)
        {
            var cart = _cartRepository.GetById(cartToUpdate.Id);

            if (cart == null)
            {
                throw new EntityNotFoundException(Constants.CartNotFound);
            }
            _cartRepository.Update(cartToUpdate);
        }
    }
}
