namespace CommandsService.Dtos
{
    public class CommandReadDto
    {
        public int Id { get; set; }
        public string Howto { get; set; } = null!;
        public string CommandLine { get; set; } = null!;
        public int PlatformId { get; set; }
    }
}
