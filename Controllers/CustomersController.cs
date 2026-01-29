using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CustomersController(CustomersRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        return await repo.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await repo.GetById(id);
        if (customer == null)
            return NotFound();
        return customer;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Customer customer)
    {
        await repo.Create(customer);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Customer customer)
    {
        customer.Id = id;
        await repo.Update(customer);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await repo.Delete(id);
        return Ok();
    }
}
