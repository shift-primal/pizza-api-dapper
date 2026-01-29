using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("pizzas/{pizzaId}/toppings")]
public class PizzaToppingsController(PizzaToppingsRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Topping>>> GetToppings(int pizzaId)
    {
        return await repo.GetToppings(pizzaId);
    }

    [HttpPost("{toppingId}")]
    public async Task<ActionResult> Add(int pizzaId, int toppingId)
    {
        await repo.Add(pizzaId, toppingId);
        return Created();
    }

    [HttpDelete("{toppingId}")]
    public async Task<ActionResult> Remove(int pizzaId, int toppingId)
    {
        await repo.Remove(pizzaId, toppingId);
        return Ok();
    }
}
