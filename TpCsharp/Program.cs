Console.Write("Tape un texte : ");
string? saisie = Console.ReadLine();
Console.WriteLine($"Vous avez tapé {saisie?.Length ?? 0} caractères");

Console.Write("Tape un autre texte : ");
string? saisie2 = Console.ReadLine();
if (saisie2 is null)
    Console.WriteLine("Aucune saisie : l'entrée est fermée.");
else
    Console.WriteLine($"Vous avez tapé {saisie2.Length} caractères");

int age;
while (true)
{
    Console.Write("Ton âge (entre 0 et 120) : ");
    string? texte = Console.ReadLine();
    if (texte is null)
        return;
    if (int.TryParse(texte, out age) && age is >= 0 and <= 120)
        break;
    Console.WriteLine("Saisie invalide, recommence.");
}
Console.WriteLine($"Tu as {age} ans.");