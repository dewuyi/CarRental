using System.ComponentModel.DataAnnotations;
using CarRental.Model;

namespace CarRental.RavenModel;

public class CarRavenDb
{
    public string Id { get; set; }
    public string Status { get; set; }
    public int Stock { get; set; }
   
    [MaxLength(100)]
    public string Name { get; set; }
    public string Category { get; set; }
    public string Manufacturer { get; set; }
    public Decimal Price { get; set; }
    
    public ICollection<RentalRavenDb> Rentals { get; set; }
}