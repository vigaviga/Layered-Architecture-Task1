using Carting.Carting.Domain.Models;
using Carting.Carting.Services.Interfaces;
using Carting.Carting.Services.Services;
using Carting.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Carting.Carting.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private ICartService _cartsService;
        public ItemsController(ICartService cartsService)
        {
            _cartsService = cartsService;
        }

        /// <summary>
        /// Updates all carts with given item
        /// </summary>
        /// <param name="itemUpdateInfo">Item to update </param>
        /// <returns>Result of action </returns>
        [HttpPut]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] ItemUpdateInfo itemInfo)
        {
            try
            {
                var item = await _cartsService.GetCartsItem(itemInfo.Id);
                if (item == null) return NotFound();

                item.Name = itemInfo.Name;
                item.Price = itemInfo.Price;
                await _cartsService.UpdateCartsGivenItem(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
