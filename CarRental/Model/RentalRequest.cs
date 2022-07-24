using System.ComponentModel.DataAnnotations;

namespace CarRental.Model;

public class RentalRequest
{
    [Required]
    public string VehicleName { get; set; }
    [Required]
    public DateTime RentalStartDate { get; set; }
    [Required]
    [Range(1,7)]
    public int RentalDurationInDays { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string CustomerEmail { get; set; }
}