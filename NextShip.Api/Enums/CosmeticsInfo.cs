namespace NextShip.Api.Enums;

public record CosmeticsInfo
{
    public CosmeticType CosmeticType { get; set; }
    public string Name { get; init; }
    public string Author { get; set; }


    public string Id { get; set; }
    public string Package { get; set; }
    public string Condition { get; set; }

    public string Resource { get; init; }
    public string FlipResource { get; set; }
    public string BackFlipResource { get; set; }
    public string BackResource { get; set; }
    public string ClimbResource { get; set; }

    public string ResHash { get; set; }
    public string ResHashBack { get; set; }
    public string ResHashClimb { get; set; }
    public string ResHashFlip { get; set; }
    public string ResHashBackFlip { get; set; }

    public bool Bounce { get; set; }
    public bool Adaptive { get; set; }
    public bool Behind { get; set; }
}