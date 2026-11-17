# TP — Introduction à C# : réponses

## 1. Premier projet

### 1.2

Le projet vise .NET 10 (`<TargetFramework>net10.0</TargetFramework>`). Les options `Nullable` et `ImplicitUsings` sont toutes les deux activées (`enable`).

- `Nullable` active l'analyse de nullabilité : le compilateur distingue `string`, qui ne doit jamais valoir `null`, de `string?`, qui peut valoir `null`, et il avertit dès qu'une valeur potentiellement nulle est utilisée sans vérification.
- `ImplicitUsings` ajoute automatiquement des `global using` vers les espaces de noms les plus courants (`System`, `System.IO`, `System.Collections.Generic`, `System.Linq`, etc.). C'est pour cela qu'on peut utiliser `Console` sans écrire `using System;`, de la même manière que `java.lang` est importé automatiquement en Java.

### 1.4

Le fichier `TpCsharp.dll` contient le code compilé en langage intermédiaire (IL), et non en code machine. Il correspond au bytecode Java : à un `.class` ou plutôt, puisqu'il regroupe tout le projet, à un `.jar`. On l'exécute avec `dotnet TpCsharp.dll`, comme on lancerait `java -jar` : la CLR joue le rôle de la JVM et compile l'IL en code machine pendant l'exécution (compilation JIT). Le même fichier s'exécute donc sous Windows, Linux ou macOS dès que .NET est installé.

### 1.5

Avec `dotnet run hello.cs`, on n'a pas besoin de fichier `.csproj` : .NET 10 crée un projet virtuel à partir du fichier seul. Aucun dossier `bin/` ni `obj/` n'apparaît à côté du fichier, car le résultat de la compilation est placé dans un dossier temporaire. C'est pratique pour un script ou un test rapide ; si le programme grandit, on peut le transformer en vrai projet avec `dotnet project convert hello.cs`. C'est l'équivalent de `java Hello.java`, qui exécute directement un fichier source sans passer par `javac`.

## 2. Types, chaînes et null

### 2.1

- Pourquoi c1.X et s1.X n'ont pas la même valeur. 
On obtient c1.X = 9 et s1.X = 1. 
PointC est une classe, donc un type référence : la variable c1 ne contient pas l'objet mais une référence vers lui. L'affectation c2 = c1 copie cette référence,si bien que c1 et c2 désignent le même objet, et modifier c2.X modifie aussi ce qu'on lit avec c1.X.
PointS est une structure, donc un type valeur : l'affectation s2 = s1 copie toute la valeur, champ par champ. s2 est une copie indépendante, et la modifier ne touche pas s1.
En Java, tous les objets se comportent comme PointC ; seuls les types primitifs comme int se comportent comme PointS.

- a == b en Java et en C#. 
En Java, a == b donnerait false : sur deux String, l'opérateur == compare les références, et string.Concat construit une nouvelle chaîne à l'exécution, distincte du littéral "info". Il faudrait utiliser a.equals(b) pour comparer le contenu. En C#, on obtient True, car le type string redéfinit l'opérateur == pour comparer le contenu des chaînes. Les chaînes restent pourtant des types référence et immuables, comme en Java : seule la signification de == change.

- Le suffixe m et le choix entre double et decimal.
Le suffixe m indique un littéral de type decimal. Un double stocke les nombres en base 2, où 0.1 et 0.2 n'ont pas de représentation exacte : leur somme vaut 0.30000000000000004, d'où False. Un decimal stocke les nombres en base 10, donc 0.1m + 0.2m vaut exactement 0.3m, d'où True. Le choix est important dans les programmes financiers ou comptables (banque, facturation, paie), où les montants doivent être exacts au centime près : on utilise alors decimal, l'équivalent de BigDecimal en Java. Pour les calculs scientifiques, physiques ou graphiques, on garde double, plus rapide et capable de représenter des valeurs bien plus grandes, quitte à comparer avec une tolérance plutôt qu'avec ==.

### 2.2

**Avertissements du compilateur.** On obtient deux avertissements : CS8600 sur la première ligne, car on affecte le résultat de `Console.ReadLine()`, de type `string?`, à une variable de type `string` ; et CS8602 sur la deuxième, car on accède à `saisie.Length` alors que `saisie` peut valoir `null`.

**Pourquoi `Console.ReadLine()` peut renvoyer `null`.** `Console.ReadLine()` a pour type de retour `string?`. Elle renvoie `null` lorsqu'il n'y a plus rien à lire sur l'entrée standard : quand l'utilisateur ferme l'entrée avec Ctrl+D (Linux, macOS) ou Ctrl+Z puis Entrée (Windows), ou quand l'entrée est redirigée depuis un fichier dont on a atteint la fin. Il ne faut pas confondre ce cas avec une ligne vide : si l'utilisateur appuie seulement sur Entrée, on obtient la chaîne vide `""`, pas `null`. Sans vérification, l'accès à `saisie.Length` lève alors une `NullReferenceException` et le programme s'arrête.

**Effet de `TreatWarningsAsErrors`.** Avec ce réglage, chaque avertissement est traité comme une erreur. Si un avertissement de nullabilité réapparaît, par exemple en remettant `string` à la place de `string?`, la compilation échoue et le programme ne peut pas être lancé tant que le problème n'est pas corrigé. On ne peut donc plus ignorer un risque de `null` : on est obligé de le traiter au moment où on écrit le code. On garde ce réglage pour la suite du TP.

## 3. Contrôle et collections

### 3.4

**Lecture de `dico["Inconnu"]`.** La clé `"Inconnu"` n'existe pas dans le dictionnaire : l'indexeur lève donc une `KeyNotFoundException` à l'exécution, et le programme s'arrête si on ne l'attrape pas. C'est une différence avec Java, où `HashMap.get` renvoie simplement `null` pour une clé absente. Pour éviter l'exception, on utilise `TryGetValue` : la méthode renvoie `true` et place la liste dans sa variable `out` si la clé existe, et `false` sinon. On teste donc le résultat dans un `if`, sans exception et en une seule recherche dans le dictionnaire, alors qu'un `ContainsKey` suivi de l'indexeur en ferait deux.

## 4. Classe Etudiant et propriétés

### 4.1

**Redéfinition de `ToString`.** Pour redéfinir `ToString`, déclarée `virtual` dans `object`, on doit utiliser le mot-clé `override`. Si on l'oublie, le compilateur affiche l'avertissement CS0114 : la méthode de `Etudiant` masque la méthode héritée au lieu de la redéfinir. Avec `TreatWarningsAsErrors`, cet avertissement devient une erreur et le projet ne compile plus. Sans ce réglage, le programme compilerait, mais `Console.WriteLine(ada)`, qui appelle `ToString` à travers une référence de type `object`, exécuterait la version d'`object` et afficherait seulement `Ecole.Etudiant`.

**Accès à `ada._notes`.** Le compilateur refuse la ligne avec l'erreur CS0122 : le membre `Etudiant._notes` est inaccessible en raison de son niveau de protection. Un champ déclaré `private` n'est accessible que depuis l'intérieur de la classe `Etudiant`.

**Pourquoi ne pas exposer la liste directement.** Si on exposait `_notes` en propriété publique de type `List<double>`, n'importe quel code pourrait ajouter, supprimer ou vider les notes (`ada.Notes.Add(45)`, `ada.Notes.Clear()`) sans passer par `AjouterNote`. La vérification des bornes 0 à 20 serait contournée, et l'objet pourrait se retrouver dans un état incohérent. On propose donc une propriété `public IReadOnlyList<double> Notes => _notes.AsReadOnly();`. Elle permet de lire les notes (nombre, accès par indice, parcours avec `foreach`) sans pouvoir les modifier. On renvoie `_notes.AsReadOnly()` plutôt que `_notes` : avec `_notes`, un appelant pourrait reconvertir la référence en `List<double>` et modifier la liste quand même.

### 4.2

**Visibilité par défaut.** Après avoir retiré `public` devant `class Promo`, le programme compile toujours. Une classe déclarée sans modificateur est `internal` par défaut : elle est visible partout dans le même assembly, c'est-à-dire dans le même projet, ce qui est le cas de `Program.cs`. Cela poserait problème avec un projet de tests séparé comme `TpCsharp.Tests` : il produit un autre assembly, qui ne pourrait pas voir `Promo`, et les tests ne compileraient pas. Il faudrait alors remettre `public`, ou autoriser explicitement le projet de tests avec `InternalsVisibleTo`. C'est l'équivalent de la visibilité « package » par défaut en Java, mais à l'échelle du projet plutôt que du paquetage.

## 5. Héritage et interfaces : les formes

### 5.1

**Dérivation d'une classe `sealed`.** Quand on essaie de dériver une classe de `Carre`, le compilateur refuse avec l'erreur CS0509 : on ne peut pas dériver d'un type `sealed`. Le mot-clé `sealed` interdit tout héritage depuis la classe, comme `final` sur une classe en Java.

### 5.2

**Ordre d'affichage et rayon.** Les formes s'affichent bien par aire croissante : le cercle de rayon 1 (3,14), le carré (9,00), le rectangle (10,00), puis le cercle de rayon 2,5 (19,63). En effet, `formes.Sort()` utilise la méthode `CompareTo` de `IComparable<Forme>`, qui compare les aires. Le texte des cercles contient bien le rayon. `Dessiner()` est écrite une seule fois, dans `Forme`, et appelle `Decrire()`. Comme `Decrire` est `virtual` et redéfinie avec `override` dans `Cercle`, c'est la version de l'objet réel qui est exécutée.

**Aire totale et plus grand périmètre.** En parcourant la liste avec une boucle, on obtient une aire totale de 41,78. La forme de plus grand périmètre est le cercle de rayon 2,5, avec un périmètre de 15,71.

### 5.3

**Suppression de `virtual`.** Le compilateur refuse la méthode de `Cercle` avec l'erreur CS0506 : on ne peut pas redéfinir le membre hérité `Forme.Decrire()`, car il n'est marqué ni `virtual`, ni `abstract`, ni `override`. En C#, une méthode n'est pas redéfinissable par défaut : il faut l'autoriser explicitement avec `virtual`.

**Différence entre `new` et `override`.** Avec `new`, les deux lignes n'affichent pas la même chose : `c.Decrire()` affiche le rayon, mais pas `f.Decrire()`, alors que les deux variables désignent le même objet. Avec `new`, la méthode de `Cercle` masque celle de `Forme` sans la remplacer : le compilateur choisit la méthode d'après le type déclaré de la variable. Comme `c` est déclarée `Cercle`, on appelle la version de `Cercle` ; comme `f` est déclarée `Forme`, on appelle celle de `Forme`. Pour la même raison, les cercles affichés par `Dessiner()` perdent leur rayon. Avec `override`, la méthode est redéfinie : le choix se fait à l'exécution, d'après le type réel de l'objet, et les deux lignes affichent le rayon. En Java, toutes les méthodes d'instance sont virtuelles par défaut, et il n'existe pas de masquage de méthode d'instance : le code se comporterait comme avec `override`, et les deux lignes afficheraient la version de `Cercle`.