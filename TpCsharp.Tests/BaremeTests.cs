using Xunit;

namespace TpCsharp.Tests;

public class BaremeTests
{
    [Theory]
    [InlineData(18.0, "Très bien")]
    [InlineData(16.0, "Très bien")]
    [InlineData(15.0, "Bien")]
    [InlineData(12.5, "Assez bien")]
    [InlineData(10.0, "Passable")]
    [InlineData(9.99, "Ajourné")]
    public void Mention_RenvoieLaBonneMention(double moyenne, string attendu)
    {
        Assert.Equal(attendu, Bareme.Mention(moyenne));
    }

    [Theory]
    [InlineData(-0.5)]
    [InlineData(21.0)]
    public void Mention_HorsBornes_LeveUneException(double moyenne)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Bareme.Mention(moyenne));
    }
}