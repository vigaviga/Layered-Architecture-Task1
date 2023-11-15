using Carting.Carting.Domain.Entities;
using Carting.Carting.Domain.Models;
using Carting.Carting.Services.Exceptions;
using Carting.Carting.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Carting.Carting.API.Controllers
{
    [ApiController]
    [Route("Carts")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartsService;
        public CartsController(ICartService cartService)
        {
            _cartsService = cartService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([Required] Cart cart)
        {
            try
            {
                var id = _cartsService.Create(cart);
                return Ok(id);
            }
            catch (EntityWithGivenIdExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute] int id)
        {
            var cart = _cartsService.Get(id);
            return cart == null ? NotFound() : Ok(cart);
        }

        [HttpGet("id/{id}/items")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetItems([FromRoute] int id)
        {
            var cart = _cartsService.Get(id);
            return cart == null ? NotFound() : Ok(cart.Items);
        }

        [HttpPost("id/{cartId}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddItemToCart([FromRoute] int cartId, [Required] Item item)
        {
            try
            {
                var resopnse = _cartsService.AddItemToCart(cartId, item);
                return Ok(resopnse);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EntityWithGivenIdExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("id/{cartId}")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveItemFromCart([FromRoute] int cartId, [Required] int itemId) 
        {
            try
            {
                var resopnse = _cartsService.RemoveItemFromCart(cartId, itemId);
                return Ok(resopnse);

            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
