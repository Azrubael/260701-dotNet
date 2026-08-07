namespace BlazingPizza.Data;

public class PizzaService
{
  public Task<Pizza[]> GetPizzasAsync()
  {
    // Call your data access technology here
    return Task.FromResult(Array.Empty<Pizza>());

  }


}