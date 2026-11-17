using Geometrie;

List<Forme> formes = [new Cercle(1), new Rectangle(2, 5), new Carre(3), new Cercle(2.5)];
formes.Sort();
foreach (var forme in formes)
    forme.Dessiner();

double aireTotale = 0;
Forme plusGrandPerimetre = formes[0];
foreach (var forme in formes)
{
    aireTotale += forme.Aire();
    if (forme.Perimetre() > plusGrandPerimetre.Perimetre())
        plusGrandPerimetre = forme;
}
Console.WriteLine($"Aire totale : {aireTotale:F2}");
Console.WriteLine($"Plus grand périmètre : {plusGrandPerimetre.Decrire()}");

Cercle c = new Cercle(1);
Forme f = c;
Console.WriteLine(c.Decrire());
Console.WriteLine(f.Decrire());