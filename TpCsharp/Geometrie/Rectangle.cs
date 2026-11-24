namespace Geometrie;

public class Rectangle : Forme
{
    // Un rectangle créé directement porte le nom « Rectangle »
    public Rectangle(double largeur, double hauteur) : this("Rectangle", largeur, hauteur)
    {
    }

    // Réservé aux classes filles, pour qu'un carré garde son propre nom
    protected Rectangle(string nom, double largeur, double hauteur) : base(nom)
    {
        if (largeur <= 0)
            throw new ArgumentOutOfRangeException(nameof(largeur), "La largeur doit être strictement positive.");
        if (hauteur <= 0)
            throw new ArgumentOutOfRangeException(nameof(hauteur), "La hauteur doit être strictement positive.");
        Largeur = largeur;
        Hauteur = hauteur;
    }

    public double Largeur { get; }
    public double Hauteur { get; }

    public override double Aire() => Largeur * Hauteur;
    public override double Perimetre() => 2 * (Largeur + Hauteur);
}
