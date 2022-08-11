using CarRental.Model;

namespace CarRental.RavenModel;

using System.ComponentModel.DataAnnotations;
public class CarCategoryRavenDb
{
    private CarCategoryRavenDb(CarCategoryEnum categoryEnum)
    {
        Name = categoryEnum.ToString();
    }
    public CarCategoryRavenDb() {}
    
    public string? Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
    
    public static implicit operator CarCategoryRavenDb(CarCategoryEnum carEnum) => new CarCategoryRavenDb(carEnum);

    public static implicit operator CarCategoryEnum(CarCategoryRavenDb carCategory) => (CarCategoryEnum) int.Parse(carCategory.Id);
    
}

