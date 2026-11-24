namespace Geometrie;

// Un point du plan ; readonly struct : il ne peut plus être modifié après sa création
public readonly struct Point2D
{
    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }
    public double Y { get; }

    // Ne modifie pas le point : renvoie un nouveau point déplacé
    public Point2D Deplacer(double dx, double dy) => new(X + dx, Y + dy);

    public override string ToString() => $"({X}, {Y})";
}
