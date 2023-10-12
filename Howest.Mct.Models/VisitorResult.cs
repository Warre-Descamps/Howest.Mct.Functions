namespace Howest.Mct.Models;

public record VisitorResult
{
    public int Time { get; set; }
    public int Amount { get; set; }
    public string Day { get; set; } = null!;
}
