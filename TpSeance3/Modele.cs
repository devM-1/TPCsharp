public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef);
public record Inscrit(string Nom, string Prenom, string Promo, string CodePostal);
public record Bulletin(string Prenom, List<double> Valeurs);