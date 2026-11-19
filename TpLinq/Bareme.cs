public static class Bareme
{
    public static string Mention(double moyenne) => moyenne switch
    {
        < 0 or > 20 => throw new ArgumentOutOfRangeException(nameof(moyenne), "La moyenne doit être entre 0 et 20."),
        >= 16 => "Très bien",
        >= 14 => "Bien",
        >= 12 => "Assez bien",
        >= 10 => "Passable",
        _ => "Ajourné"
    };
}