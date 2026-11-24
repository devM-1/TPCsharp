// Une note dont la valeur et le coefficient sont vérifiés dès la création, même quand elle vient du JSON
public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef)
{
    // En dehors de 0 à 20, on lève une exception qui garde la valeur reçue
    public double Valeur { get; init; } = Valeur is >= 0 and <= 20
        ? Valeur
        : throw new ArgumentOutOfRangeException(nameof(Valeur), Valeur, "La valeur de la note doit être comprise entre 0 et 20.");

    // Le coefficient doit être strictement positif
    public int Coef { get; init; } = VerifierCoef(Coef);

    // ThrowIfNegativeOrZero ne renvoie rien : cette méthode vérifie, puis renvoie le coefficient
    private static int VerifierCoef(int coef)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(coef);
        return coef;
    }
}

// Le code postal est du texte : on ne calcule pas avec, et il peut commencer par 0
public record Inscrit(string Nom, string Prenom, string Promo, string CodePostal);

// Seulement les champs utiles de la réponse de l'API geo.api.gouv.fr
public record Commune(string Nom, string Code, int Population);
