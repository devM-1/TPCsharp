using Ecole;
using Geometrie;

// Un point est une struct : p2 est une copie, la déplacer ne change pas p1
var p1 = new Point2D(1, 2);
var p2 = p1;
p2 = p2.Deplacer(5, 5);
Console.WriteLine($"p1 = {p1}, p2 = {p2}");

// Lire le centre d'une forme en donne une copie : le cercle garde son centre d'origine
var cercle = new Cercle(1) { Centre = new Point2D(2, 3) };
Point2D centre = cercle.Centre;
centre = centre.Deplacer(10, 10);
Console.WriteLine($"Centre du cercle : {cercle.Centre}, copie déplacée : {centre}");

// ada1 et ada2 ont le même nom, le même prénom et la même date de naissance, mais pas la même année
var ada1 = new Etudiant("Lovelace", "Ada", 4) { Naissance = new DateOnly(1815, 12, 10) };
var ada2 = new Etudiant("Lovelace", "Ada", 5) { Naissance = new DateOnly(1815, 12, 10) };
var alan = new Etudiant("Turing", "Alan", 4) { Naissance = new DateOnly(1912, 6, 23) };

// Equals compare le contenu, alors que == compare toujours les références
Console.WriteLine(ada1.Equals(ada2));
Console.WriteLine(ada1 == ada2);

// Le HashSet refuse ada2, qu'il reconnaît comme un doublon d'ada1
var ensemble = new HashSet<Etudiant>();
Console.WriteLine(ensemble.Add(ada1));
Console.WriteLine(ensemble.Add(ada2));
Console.WriteLine(ensemble.Add(alan));
Console.WriteLine($"Nombre d'étudiants dans l'ensemble : {ensemble.Count}");
