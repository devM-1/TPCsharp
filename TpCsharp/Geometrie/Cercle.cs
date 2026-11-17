namespace Geometrie;

public class Cercle : Forme
{
    public Cercle(double rayon) : base("Cercle")
    {
        if (rayon <= 0)
            throw new ArgumentOutOfRangeException(nameof(rayon), "Le rayon doit être strictement positif.");
        Rayon = rayon;
    }

    public double Rayon { get; }

    public override double Aire() => Math.PI * Rayon * Rayon;
    public override double Perimetre() => 2 * Math.PI * Rayon;

    public override string Decrire() => $"{base.Decrire()}, rayon {Rayon:F2}";
}