// Une note du fichier notes.csv
public record Note(string Nom, string Prenom, string Matiere, double Valeur, int Coef);

// Un étudiant du fichier etudiants.csv
public record Inscrit(string Nom, string Prenom, string Promo);

// Le bulletin d'un étudiant : son prénom et toutes ses valeurs (exercice 3.3)
public record Bulletin(string Prenom, List<double> Valeurs);
