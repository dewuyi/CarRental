using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Model;

public enum CarCategoryEnum
{
    Small = 1,
    Medium,
    Large
}

public class CarCategory
{
    private CarCategory(CarCategoryEnum categoryEnum)
    {
        Id = (int) categoryEnum;
        Name = categoryEnum.ToString();
    }
    public CarCategory() {}
    
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
    
    public static implicit operator CarCategory(CarCategoryEnum carEnum) => new CarCategory(carEnum);

    public static implicit operator CarCategoryEnum(CarCategory carCategory) => (CarCategoryEnum)carCategory.Id;
    
}

