namespace mslearn_ContosoPizza.Models;

// This class defines a pizza
public class Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsGlutenFree { get; set; }
}