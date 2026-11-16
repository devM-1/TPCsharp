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