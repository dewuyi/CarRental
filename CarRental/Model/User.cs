using System.ComponentModel.DataAnnotations;

namespace CarRental.Model;

public class User
{
    [Key]
    public string Name { get; set; }
    public string Password { get; set; }
    
}