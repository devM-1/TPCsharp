using Ecole;

var ada = new Etudiant("Lovelace", "Ada", 4)
{
    Naissance = new DateOnly(1815, 12, 10)
};
ada.AjouterNote(15);
ada.AjouterNote(16);
Console.WriteLine(ada);
ada.Annee = 5;
// ada.Nom = "Byron";
// ada.Naissance = new DateOnly(1816, 1, 1);

try
{
    ada.AjouterNote(25);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine($"Erreur : {e.Message}");
}

var alan = new Etudiant("Turing", "Alan", 4);
alan.AjouterNote(9);
alan.AjouterNote(12);

var grace = new Etudiant("Hopper", "Grace", 4);
grace.AjouterNote(17);
grace.AjouterNote(18.5);

var promo = new Promo("M1 Info");
promo.Inscrire(ada);
promo.Inscrire(alan);
promo.Inscrire(grace);

Console.WriteLine($"Recherche de Turing : {promo.Chercher("Turing")?.NomComplet ?? "introuvable"}");
Console.WriteLine($"Major : {promo.Major()}");
Console.WriteLine($"Moyenne générale : {promo.MoyenneGenerale():F2}");
promo.AfficherReleve();