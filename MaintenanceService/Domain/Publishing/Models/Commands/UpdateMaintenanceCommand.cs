using System.ComponentModel.DataAnnotations;

namespace MaintenanceService.Domain.Publishing.Models.Commands
{
    public class UpdateMaintenanceCommand
    {
        public int Id { get; set; }
        [Required] public int VehicleId { get; set; }
        [Required] public string Description { get; set; }
        [Required] public DateTime MaintenanceDate { get; set; }
        [Required] public decimal Cost { get; set; }
        [Required] public string Status { get; set; }
    }
} 