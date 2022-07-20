using System.ComponentModel.DataAnnotations;

namespace CarRental.Model;

public class Car
{
    public int Id { get; set; }
    public RentalStatus Status { get; set; }
   
    [MaxLength(100)]
    public string Name { get; set; }
    public virtual CarCategoryEnum Category { get; set; }
    public CarManufacturer Manufacturer { get; set; }
    
    public ICollection<Rental> Rentals { get; set; }
 }