Console.Write("Quel est ton prénom ? ");
string prenom = Console.ReadLine();

Console.Write("Quelle est ton année de naissance ? ");
int annee = int.Parse(Console.ReadLine());

int age = DateTime.Now.Year - annee;

Console.WriteLine($"Bonjour {prenom}, tu as {age} ans.");