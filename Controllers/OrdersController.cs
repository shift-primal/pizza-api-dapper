using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class OrdersController(OrdersRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAll()
    {
        return await repo.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var order = await repo.GetById(id);
        if (order == null)
            return NotFound();
        return order;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Order order)
    {
        await repo.Create(order);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Order order)
    {
        order.Id = id;
        await repo.Update(order);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await repo.Delete(id);
        return Ok();
    }
}
