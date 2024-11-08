﻿using System.ComponentModel.DataAnnotations;

namespace RentService.Domain.Publishing.Models.Commands;

public class UpdateRentCommand
{
    [Required] public string Status { get; set; }
    [Required] public DateOnly StartDate { get; set; }
    [Required] public DateOnly EndDate { get; set; }
    [Required] public int VehicleId { get; set; }
    [Required] public int OwnerId { get; set; }
    [Required] public int TenantId { get; set; }
    [Required] public string PickUpPlace { get; set; }
}