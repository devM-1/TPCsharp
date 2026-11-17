namespace Geometrie;

public readonly struct Point2D
{
    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }
    public double Y { get; }

    public Point2D Deplacer(double dx, double dy) => new(X + dx, Y + dy);

    public override string ToString() => $"({X}, {Y})";
}