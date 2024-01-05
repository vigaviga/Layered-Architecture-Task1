using Carting.Carting.Domain.Entities;
using Carting.Carting.Domain.Models;
using Carting.Carting.Services.Exceptions;
using Carting.Carting.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Carting.Carting.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartsService;
        public CartsController(ICartService cartService)
        {
            _cartsService = cartService;
        }

        /// <summary>
        /// Created cart entity
        /// </summary>
        /// <param name="cart">Cart entity to be created.</param>
        /// <returns>Id of a cart that got created</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] Cart cart)
        {
            try
            {
                var id = await _cartsService.Create(cart);
                return Ok(id);
            }
            catch (EntityWithGivenIdExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrievs cart with given id
        /// </summary>
        /// <param name="cartId">Id of cart whose data should be retrieved</param>
        /// <returns>A cart entity</returns>
        [HttpGet("{cartId}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] string cartId)
        {
            var cart = await _cartsService.Get(cartId);
            return cart == null ? NotFound() : Ok(cart);
        }

        /// <summary>
        /// Gets items of a cart
        /// </summary>
        /// <param name="cartId">Id of cart whose items should be retrieved</param>
        /// <returns>List of carts items</returns>
        [HttpGet("{cartId}/items")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItems([FromRoute] string cartId)
        {
            var cart = await _cartsService.Get(cartId);
            return cart == null ? NotFound() : Ok(cart.Items);
        }
        
        /// <summary>
        /// Adds item to a cart
        /// </summary>
        /// <param name="cartId">Id of a cart into which item should be added</param>
        /// <param name="item">Item to be added</param>
        /// <returns>Carts id to which an item got added</returns>
        [HttpPost("{cartId}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemToCart([FromRoute] string cartId, [Required] Item item)
        {
            try
            {
                var resopnse = await _cartsService.AddItemToCart(cartId, item);
                return Ok(resopnse);
            }
            catch (EntityWithGivenIdExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes an item from a cart
        /// </summary>
        /// <param name="cartId">Id of a cart from which item should be removed</param>
        /// <param name="itemId">Id of an item to be removed</param>
        /// <returns>Carts id from which an item got removed</returns>
        [HttpDelete("{cartId}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] string cartId, [Required] int itemId)
        {
            try
            {
                var resopnse = await _cartsService.RemoveItemFromCart(cartId, itemId);
                return Ok(resopnse);

            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Update cart entity
        /// </summary>
        /// <param name="cart">Cart entity to be updated.</param>
        /// <returns>Id of a cart that got updated</returns>
        [HttpPut]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([Required] Cart cart)
        {
            try
            {
                var id = await _cartsService.Update(cart);
                return Ok(id);
            }
            catch (EntityWithGivenIdExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// Updates all carts with given item
        /// </summary>
        /// <param name="item">Item to update </param>
        /// <returns>Result of action </returns>
        [HttpPut("/item")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([Required] Item item)
        {
            try
            {
                await _cartsService.UpdateCartsGivenItem(item);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
