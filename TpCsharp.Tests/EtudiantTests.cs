using Ecole;
using Xunit;

namespace TpCsharp.Tests;

public class EtudiantTests
{
    // Sans aucune note, la moyenne vaut 0
    [Fact]
    public void Moyenne_SansNote_VautZero()
    {
        var e = new Etudiant("Lovelace", "Ada", 4);
        Assert.Equal(0.0, e.Moyenne());
    }

    // (12 + 15 + 16) / 3 = 14,33…, comparé à 2 décimales près
    [Fact]
    public void Moyenne_PlusieursNotes_EstCorrecte()
    {
        var e = new Etudiant("Lovelace", "Ada", 4);
        e.AjouterNote(12);
        e.AjouterNote(15);
        e.AjouterNote(16);
        Assert.Equal(14.33, e.Moyenne(), 2);
    }

    // Une note hors de 0 à 20 est refusée
    [Theory]
    [InlineData(-1.0)]
    [InlineData(20.5)]
    public void AjouterNote_HorsBornes_LeveUneException(double note)
    {
        var e = new Etudiant("Lovelace", "Ada", 4);
        Assert.Throws<ArgumentOutOfRangeException>(() => e.AjouterNote(note));
    }

    // Le prénom tel quel, puis le nom en majuscules
    [Fact]
    public void NomComplet_PrenomPuisNomEnMajuscules()
    {
        var e = new Etudiant("Lovelace", "Ada", 4);
        Assert.Equal("Ada LOVELACE", e.NomComplet);
    }
}
