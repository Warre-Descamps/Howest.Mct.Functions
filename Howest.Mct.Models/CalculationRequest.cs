namespace Howest.Mct.Models;

public class CalculationRequest
{
    public int A { get; set; }
    public int B { get; set; }
    public string Operator { get; set; } = null!;
}