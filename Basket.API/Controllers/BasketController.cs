using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _basketRepository.GetBasket(username);   

            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            return Ok(await _basketRepository.UpdateBasket(shoppingCart));
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        public async Task<IActionResult> DeleteBasket(string username) 
        {
            await _basketRepository.DeleteBasket(username);

            return Ok();
        }
    }
}
