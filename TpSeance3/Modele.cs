public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef)
{
    // La valeur est vérifiée dès la création : en dehors de 0 à 20, on lève une exception
    public double Valeur { get;} = Valeur is >=0 and <=20 
        ? Valeur
        : throw new ArgumentOutOfRangeException(nameof(Valeur),Valeur, "La valeur de la note doit être comprise entre 0 et 20.");

     public int Coef { get; init; } = VerifierCoef(Coef);

    private static int VerifierCoef(int coef)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(coef);
        return coef;
    }
}
public record Inscrit(string Nom, string Prenom, string Promo, string CodePostal);
public record Bulletin(string Prenom, List<double> Valeurs);