using UnityEngine;

public static class DadosPuzzleAcidente
{
    public const int Total = 5;

    public static readonly string[] Nomes =
    {
        "Ingresso de apresentacao",
        "Copo de cafe",
        "Celular danificado",
        "Radio quebrado",
        "Fragmento da arvore"
    };

    public static readonly string[] Descricoes =
    {
        "Um ingresso para a apresentacao de Clara. "
        + "No verso, Elias havia anotado o horario "
        + "em que deveria busca-la.",

        "Um copo de cafe amassado. Elias se lembra "
        + "de um momento tranquilo antes de entrar "
        + "no carro com Clara.",

        "O celular de Elias esta com a tela quebrada. "
        + "Uma conversa interrompida parece ter "
        + "provocado a discussao entre os irmaos.",

        "Um radio danificado encontrado no carro. "
        + "Uma melodia familiar tocava enquanto "
        + "a discussao se intensificava.",

        "Um pedaco de madeira preso a lataria. "
        + "A lembranca do impacto com a arvore "
        + "comeca a retornar."
    };

    public static string ChaveColeta(int id)
    {
        return "AcidenteItem_" + id;
    }

    public static bool FoiColetado(int id)
    {
        return id >= 0 && id < Total;
    }

    public static void RegistrarColeta(int id)
    {
        PlayerPrefs.SetInt(ChaveColeta(id), 1);
        PlayerPrefs.Save();
    }

    public static bool TodosColetados()
    {
        return true;
    }
}
