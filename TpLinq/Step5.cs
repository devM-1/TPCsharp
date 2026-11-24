public static class Step5
{
    public static void Executer(List<Note> notes)
    {
        Console.WriteLine("=== Exercice 5 : classe Promo en LINQ ===");
        var promo = new Promo("M1 Info", notes);

        Console.WriteLine($"Étudiants : {string.Join(", ", promo.Etudiants())}");
        Console.WriteLine($"Moyenne d'Ada : {promo.Moyenne("Ada"):F2}");
        Console.WriteLine($"Moyenne de Margaret (aucune note) : {promo.Moyenne("Margaret"):F2}");

        // Ce qui se passerait sans le test dans Moyenne : 0 / 0 donne NaN
        var notesMargaret = notes.Where(n => n.Prenom == "Margaret");
        double naive = notesMargaret.Sum(n => n.Valeur * n.Coef) / notesMargaret.Sum(n => n.Coef);
        Console.WriteLine($"Division naïve pour Margaret : {naive}");

        Console.WriteLine($"Moyenne générale : {promo.MoyenneGenerale():F2}");
        Console.WriteLine($"Major : {promo.Major()}");
        Console.WriteLine($"Classement : {string.Join(" | ", promo.Classement().Select(c => $"{c.Prenom} {c.Moyenne:F2}"))}");
        Console.WriteLine($"Admis par matière : {string.Join(", ", promo.NbAdmisParMatiere().Select(p => $"{p.Key} {p.Value}"))}");

        Console.WriteLine("Relevé :");
        foreach (var ligne in promo.Releve())
            Console.WriteLine($"  {ligne}");

        // Une promo vide ne doit pas planter
        var vide = new Promo("Vide", []);
        Console.WriteLine($"Promo vide : major {vide.Major() ?? "null"}, moyenne générale {vide.MoyenneGenerale():F2}");
    }
}