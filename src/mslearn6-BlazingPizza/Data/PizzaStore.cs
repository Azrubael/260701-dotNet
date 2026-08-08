namespace BlazingPizza.Data;

public class PizzaStore
{
    public Pizza[] Pizzas { get; } =
    [
        new Pizza { PizzaId = 1, Name = "Basic Cheese Pizza", Description = "It's cheesy and delicious. Why wouldn't you want one?", Price = 9.99M, Vegan = false, Vegetarian = false},
        new Pizza { PizzaId = 2, Name = "Margherita", Description = "Tomato, mozzarella, basil", Price = 9.99M, Vegan = false, Vegetarian = false},
        new Pizza { PizzaId = 3, Name = "Pepperoni", Description = "Tomato, mozzarella, pepperoni", Price = 10.5M, Vegan = false, Vegetarian = false },
        new Pizza { PizzaId = 4, Name = "Classic pepperoni", Description = "It's the pizza you grew up with, but Blazing hot!", Price = 10.5M, Vegan = false, Vegetarian = false },
    ];
}