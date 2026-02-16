namespace DataSync.Models;

public class Well
{
    public int Id { get; set; }  // Identity (DB generated)
    public int ExternalId { get; set; }  // API ID
    public string UniqueName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}
