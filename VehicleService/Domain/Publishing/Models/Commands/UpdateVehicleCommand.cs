using System.ComponentModel.DataAnnotations;

namespace VehicleService.Domain.Publishing.Models.Commands;

public class UpdateVehicleCommand
{
    public int Id { get; set; }
    [Required] public string Brand { get; set; }
    [Required] public string Model { get; set; }
    [Required] public int MaximumSpeed { get; set; }
    [Required] public string Consumption { get; set; }
    [Required] public string Dimensions { get; set; }
    [Required] public string Weight { get; set; }
    [Required] public string CarClass { get; set; }
    [Required] public string Transmission { get; set; }
    [Required] public string TimeType { get; set; }
    [Required] public decimal RentalCost { get; set; }
    [Required] public string PickUpPlace { get; set; }
    [Required] public string UrlImage { get; set; }
    [Required] public string RentStatus { get; set; }
    [Required] public int OwnerId { get; set; }
    public bool IsActive { get; set; }
}