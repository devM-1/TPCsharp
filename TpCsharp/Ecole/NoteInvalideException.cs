namespace Ecole;

// Levée pour une ligne invalide du fichier de notes ; elle garde le numéro de la ligne fautive
public class NoteInvalideException : Exception
{
    public NoteInvalideException(int ligne, string message) : base(message)
    {
        Ligne = ligne;
    }

    public int Ligne { get; }
}
