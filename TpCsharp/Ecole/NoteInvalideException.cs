namespace Ecole;

public class NoteInvalideException : Exception
{
    public NoteInvalideException(int ligne, string message) : base(message)
    {
        Ligne = ligne;
    }

    public int Ligne { get; }
}