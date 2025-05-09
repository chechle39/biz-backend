﻿using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos
{
    public class PlatformCreateDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public required string Publisher { get; set; }
        [Required]
        public string? Cost { get; set; }
    }
}
