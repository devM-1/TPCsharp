// Barème des mentions, utilisé pour le relevé de la promo
public static class Bareme
{
    // Les cas sont testés dans l'ordre : le premier qui correspond donne la mention
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
