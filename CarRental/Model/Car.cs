using System.ComponentModel.DataAnnotations;

namespace CarRental.Model;

public class Car
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int Stock { get; set; }
   
    [MaxLength(100)]
    public string Name { get; set; }
    public string Category { get; set; }
    public string Manufacturer { get; set; }
    public Decimal Price { get; set; }
    
    public ICollection<Rental> Rentals { get; set; }
 }