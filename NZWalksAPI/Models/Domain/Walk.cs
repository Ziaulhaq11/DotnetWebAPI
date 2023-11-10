namespace NZWalksAPI.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        //Navigation properties 
        // So this tells entity framework core that a Walk will have a difficulty and this is the difficulty ID
        public Difficulty Difficulty { get; set; }
        public Region Regions { get; set; }
    }
}
