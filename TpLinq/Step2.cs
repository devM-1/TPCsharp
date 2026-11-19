public static class Step2
{
    public static void Executer(List<Note> notes)
    {
        Console.WriteLine("=== Premiere requetes===");

        // 2.1.1 On garde les notes d'Algo, puis on les trie de la plus haute à la plus basse
        var algo = notes
            .Where(n => n.Matiere == "Algo")
            .OrderByDescending(n => n.Valeur);
        Afficher("Notes d'Algo par valeur décroissante", algo);

        // 2.1.2 On ne garde que le nom des matières, on retire les doublons, puis on trie
        var matieres = notes
            .Select(n => n.Matiere)
            .Distinct()
            .Order();
        Console.WriteLine($"Matières : {string.Join(", ", matieres)}");

        // 2.1.3 Pour chaque note sous 10, on crée un petit objet sans nom avec le prénom et la valeur
        var sous10Anonyme = notes
            .Where(n => n.Valeur < 10)
            .Select(n => new { n.Prenom, n.Valeur });
        Console.WriteLine($"Sous 10 (type anonyme) : {string.Join(", ", sous10Anonyme)}");

        // Même chose, mais avec un tuple
        var sous10Tuple = notes
            .Where(n => n.Valeur < 10)
            .Select(n => (n.Prenom, n.Valeur));
        Console.WriteLine($"Sous 10 (tuple) : {string.Join(", ", sous10Tuple)}");

        // 2.1.4 Avec deux OrderBy, le second refait tout le tri, et l'ordre par matière est perdu
        var classementFaux = notes
            .OrderBy(n => n.Matiere)
            .OrderByDescending(n => n.Valeur);
        Afficher("Classement avec deux OrderBy", classementFaux);

        // ThenBy ajoute un critère secondaire sans casser le premier tri
        var classement = notes
            .OrderBy(n => n.Matiere)
            .ThenByDescending(n => n.Valeur)
            .ThenBy(n => n.Prenom);
        Afficher("Classement avec ThenBy", classement);

        // 2.1.5 On trie de la meilleure à la moins bonne, puis on prend les 3 premières
        var top3 = notes
            .OrderByDescending(n => n.Valeur)
            .Take(3);
        Afficher("Top 3", top3);

        // On saute les 3 premières et on prend les 3 suivantes, soit les places 4 à 6
        var places4a6 = notes
            .OrderByDescending(n => n.Valeur)
            .Skip(3)
            .Take(3);
        Afficher("Places 4 à 6", places4a6);

        // 2.2.1 Le nombre de notes, le nombre de notes sous 10, la moyenne simple
        int nbNotes = notes.Count;
        int nbSous10 = notes.Count(n => n.Valeur < 10);
        double moyenne = notes.Average(n => n.Valeur);
        Console.WriteLine($"{nbNotes} notes, dont {nbSous10} sous 10, moyenne simple {moyenne:F2}");

        // 2.2.2 Max renvoie seulement la meilleure valeur, MaxBy renvoie la note entière
        double meilleure = notes.Max(n => n.Valeur);
        Note? meilleureNote = notes.MaxBy(n => n.Valeur);
        Console.WriteLine($"Max : {meilleure}");
        Console.WriteLine($"MaxBy : {meilleureNote}");

        // 2.2.3 Moyenne pondérée : la somme des valeurs multipliées par leur coefficient, divisée par la somme des coefficients
        double moyennePonderee = notes.Sum(n => n.Valeur * n.Coef) / notes.Sum(n => n.Coef);
        Console.WriteLine($"Moyenne pondérée : {moyennePonderee:F2}");

        // 2.2.4 Les prénoms sans doublon, dans l'ordre où ils apparaissent dans le fichier
        var prenoms = notes
            .Select(n => n.Prenom)
            .Distinct();

        // Aggregate colle les prénoms un par un, en ajoutant une virgule entre chaque
        string avecAggregate = prenoms.Aggregate((texte, prenom) => texte + ", " + prenom);
        Console.WriteLine($"Avec Aggregate : {avecAggregate}");

        // string.Join fait la même chose en une seule fois
        string avecJoin = string.Join(", ", prenoms);
        Console.WriteLine($"Avec string.Join : {avecJoin}");

        // 2.3.1 FirstOrDefault renvoie null si aucune note ne correspond
        Note? linusBD = notes.FirstOrDefault(n => n.Prenom == "Linus" && n.Matiere == "BD");
        Console.WriteLine($"Linus en BD avec FirstOrDefault : {linusBD?.ToString() ?? "null"}");

        // First lève une exception si aucune note ne correspond
        try
        {
            _ = notes.First(n => n.Prenom == "Linus" && n.Matiere == "BD");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Linus en BD avec First : {e.GetType().Name} ({e.Message})");
        }

        // 2.3.2 Single renvoie l'unique note qui correspond
        Note adaAlgo = notes.Single(n => n.Prenom == "Ada" && n.Matiere == "Algo");
        Console.WriteLine($"Ada en Algo avec Single : {adaAlgo}");

        // Single lève une exception si plusieurs notes correspondent
        try
        {
            _ = notes.Single(n => n.Prenom == "Ada");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Single sur les notes d'Ada : {e.GetType().Name} ({e.Message})");
        }

        // 2.3.3 Any : existe-t-il au moins une note sous 10 ?
        bool uneSous10 = notes.Any(n => n.Valeur < 10);
        Console.WriteLine($"Une note sous 10 : {uneSous10}");

        // All : toutes les notes de BD valent-elles au moins 12 ?
        bool tousAuMoins12EnBD = notes
            .Where(n => n.Matiere == "BD")
            .All(n => n.Valeur >= 12);
        Console.WriteLine($"Tous au moins 12 en BD : {tousAuMoins12EnBD}");

        // All sur une liste vide renvoie true, car aucun élément ne contredit la condition
        bool allVide = notes
            .Where(n => n.Matiere == "Physique")
            .All(n => n.Valeur >= 12);
        Console.WriteLine($"All sur une séquence vide : {allVide}");
    }

    // Affiche un titre, puis chaque note sur une ligne (une boucle est permise pour afficher)
    static void Afficher(string titre, IEnumerable<Note> notes)
    {
        Console.WriteLine($"{titre} :");
        foreach (var n in notes)
            Console.WriteLine($"  {n.Prenom} · {n.Matiere} · {n.Valeur}");
    }
}