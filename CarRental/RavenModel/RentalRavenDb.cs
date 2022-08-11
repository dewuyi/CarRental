using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.RavenModel;

public class RentalRavenDb
{
    public string Id { get; set; }
    public DateTime RentalStart { get; set; }
    public DateTime RentalEnd { get; set; }
    [ForeignKey("Car")]
    public int CarId { get; set; }
    
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }
}