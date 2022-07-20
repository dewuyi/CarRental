using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Model;

public class Rental
{
    public int Id { get; set; }
    public DateTime RentalStart { get; set; }
    public DateTime RentalEnd { get; set; }
    [ForeignKey("Car")]
    public int CarId { get; set; }
    public Car Car { get; set; }
}