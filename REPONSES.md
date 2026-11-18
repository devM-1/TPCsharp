# TP — Introduction à C# : réponses

## 1. Premier projet

### 1.2
Le projet vise .NET 10 (net10.0), et les options Nullable et ImplicitUsings sont activées. Avec Nullable, le compilateur signale les valeurs qui peuvent être null. ImplicitUsings ajoute automatiquement les using courants : c'est pour cela qu'on peut utiliser Console sans écrire using System;.

### 1.4
Le .dll contient du code intermédiaire (IL), pas du code machine. C'est l'équivalent d'un .jar en Java : la CLR l'exécute comme la JVM exécute le bytecode, en le compilant pendant l'exécution (JIT).

### 1.5
Un fichier seul s'exécute sans .csproj : .NET crée un projet virtuel et compile dans un dossier temporaire, donc aucun dossier bin/ ni obj/ n'apparaît. C'est l'équivalent de java Hello.java, pratique pour un petit script.

## 2. Types, chaînes et null

### 2.1
**c1.X et s1.X.** On obtient c1.X = 9 et s1.X = 1. Une classe est un type référence : c2 = c1 copie la référence, donc les deux variables désignent le même objet. Une structure est un type valeur : s2 = s1 copie les données, donc s2 est indépendante de s1.

**a == b.** En Java, == compare les références et donnerait false, car string.Concat crée un nouvel objet. En C#, string redéfinit == pour comparer le contenu, d'où True.

**Suffixe m.** Il désigne un decimal, stocké en base 10 : 0.1m + 0.2m vaut exactement 0.3m. Un double est stocké en base 2, ce qui provoque une erreur d'arrondi, d'où False. On choisit decimal pour les montants (banque, facturation) et double pour les calculs scientifiques.

### 2.2
**Avertissements.** On obtient CS8600, car on range un string? dans un string, et CS8602, car on lit saisie.Length alors que saisie peut valoir null.

**ReadLine() et null.** ReadLine() renvoie null quand l'entrée standard est fermée, c'est-à-dire quand il n'y a plus rien à lire : Ctrl+D sous Linux et macOS, Ctrl+Z sous Windows, ou fin d'un fichier redirigé. Une ligne vide donne "", pas null.

**TreatWarningsAsErrors.** Les avertissements deviennent des erreurs : si un problème de nullabilité réapparaît, le projet ne compile plus.

## 3. Contrôle et collections

### 3.4
dico["Inconnu"] lève une KeyNotFoundException, alors qu'en Java, get renverrait null. Avec TryGetValue, on obtient simplement false, sans exception, si la clé n'existe pas.

## 4. Classe Etudiant et propriétés

### 4.1
**override.** On redéfinit ToString avec override. Sans ce mot-clé, la méthode masque celle d'object au lieu de la redéfinir : on obtient l'avertissement CS0114, transformé en erreur ici. Sans TreatWarningsAsErrors, le programme compilerait, mais Console.WriteLine(ada) afficherait Ecole.Etudiant.

**ada._notes.** On obtient l'erreur CS0122 : le champ est private, donc inaccessible en dehors de la classe.

**Liste publique.** Exposer directement la List<double> permettrait d'ajouter ou de supprimer des notes sans passer par AjouterNote, donc sans contrôle des bornes. On propose public IReadOnlyList<double> Notes => _notes.AsReadOnly();, qui permet de lire les notes sans pouvoir les modifier.

### 4.2
Sans public, la classe est internal par défaut. Le programme compile toujours, car Program.cs est dans le même projet, mais un projet de tests séparé ne pourrait pas utiliser Promo.

## 5. Héritage et interfaces : les formes

### 5.1
Dériver une classe de Carre provoque l'erreur CS0509 : une classe sealed ne peut pas être héritée. C'est l'équivalent de final en Java.

### 5.2
Les formes s'affichent bien par aire croissante, car Sort() utilise CompareTo, qui compare les aires. Le texte des cercles contient le rayon, car Decrire est virtual et redéfinie dans Cercle. On obtient une aire totale de 41,78, et la forme de plus grand périmètre est le cercle de rayon 2,5 (15,71).

### 5.3
**Sans virtual.** On obtient l'erreur CS0506 : on ne peut redéfinir qu'une méthode virtual, abstract ou override.

**new ou override.** Avec new, c.Decrire() affiche le rayon mais pas f.Decrire(), alors que les deux variables désignent le même objet : la méthode est choisie selon le type déclaré de la variable (masquage). Avec override, elle est choisie selon le type réel de l'objet (redéfinition), et les deux lignes affichent le rayon. En Java, les méthodes sont virtuelles par défaut : les deux lignes afficheraient aussi le rayon.

## 6. Records et exceptions : un fichier de notes

### 6.1
Deux records identiques sont égaux (True), car un record compare ses valeurs. Deux objets Etudiant avec les mêmes données ne le sont pas (False), car pour une classe, == compare les références. with crée une copie modifiée sans changer l'original.

### 6.6
On place le calcul dans Etudiant, puisque c'est cette classe qui possède les notes. Il faut y conserver le coefficient de chaque note : on ajoute un paramètre coef à AjouterNote, et une méthode MoyennePonderee() qui fait le calcul.


# TP — C# séance 2 : LINQ — réponses

## 0. Mise en place

### 0.2
Avec Where(n => n is not null), le résultat reste de type IEnumerable<Note?> : Where retire des éléments mais ne change pas leur type, donc le compilateur considère toujours que chaque note peut être null. Avec OfType<Note>(), on obtient un IEnumerable<Note>. Les null sont écartés et le type n'est plus nullable, ce qui permet d'utiliser les notes sans avertissement ni vérification supplémentaire.