using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos
{
    public class CommandCreateDto
    {
        [Required]
        public string Howto { get; set; } = null!;
        [Required]
        public string CommandLine { get; set; } = null!;
    }
}
