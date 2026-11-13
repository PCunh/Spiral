
using UnityEngine;

public class BloqueioPortaPuzzle : MonoBehaviour
{
    public enum Requisito
    {
        Nenhum,
        Fotografia,
        Livros,
        Rotina,
        Canto,
        Acidente,
        GradeLogica,
        Todos
    }

    [SerializeField] private Requisito requisito = Requisito.Nenhum;

    public bool PodeEntrar()
    {
        if (requisito == Requisito.Nenhum)
            return true;

        if (requisito == Requisito.Todos)
            return TodosConcluidos();

        string chave = ObterChave(requisito);

        return PlayerPrefs.GetInt(chave, 0) == 1;
    }

    private string ObterChave(Requisito puzzle)
    {
        switch (puzzle)
        {
            case Requisito.Fotografia:
                return "PuzzleFoto";

            case Requisito.Livros:
                return "PuzzleLivrosConcluido";

            case Requisito.Rotina:
                return "PuzzleRotinaConcluido";

            case Requisito.Canto:
                return "PuzzleCantoConcluido";

            case Requisito.Acidente:
                return "PuzzleAcidenteConcluido";

            case Requisito.GradeLogica:
                return "PuzzleGradeLogica";

            default:
                return "";
        }
    }

    private bool TodosConcluidos()
    {
        return
            PlayerPrefs.GetInt("PuzzleFoto", 0) == 1 &&
            PlayerPrefs.GetInt("PuzzleLivrosConcluido", 0) == 1 &&
            PlayerPrefs.GetInt("PuzzleRotinaConcluido", 0) == 1 &&
            PlayerPrefs.GetInt("PuzzleCantoConcluido", 0) == 1 &&
            PlayerPrefs.GetInt("PuzzleAcidenteConcluido", 0) == 1 &&
            PlayerPrefs.GetInt("PuzzleGradeLogica", 0) == 1;
    }
}
