using mslearn_ContosoPizza.Models;
using mslearn_ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    public PizzaController()
    {
    }

	// a method that returns the pizzas from our service
	[HttpGet]
	public ActionResult<List<Pizza>> GetAll() =>
		PizzaService.GetAll();

	// to request information about a specific pizza instead of the entire list
	[HttpGet("{id}")]
	public ActionResult<Pizza> Get(int id)
	{
		var pizza = PizzaService.Get(id);

		if(pizza == null)
			return NotFound();

		return pizza;
	}

    // POST action

    // PUT action

    // DELETE action
}