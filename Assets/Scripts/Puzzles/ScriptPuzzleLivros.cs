using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPuzzleLivros : MonoBehaviour
{
    [SerializeField] private Image capaLivro;
    [SerializeField] private Sprite[] capas = new Sprite[5];
    [SerializeField] private TMP_Text nomeLivro;
    [SerializeField] private TMP_Text descricaoLivro;
    [SerializeField] private TMP_Text contadorLivro;
    [SerializeField] private Button botaoAnterior;
    [SerializeField] private Button botaoProximo;
    [SerializeField] private Button botaoDiario;
    [SerializeField] private Button botaoVoltar;
    [SerializeField] private string pontoRetorno = "PontoEntrada_Rotina";


    private int livroAtual = 0;

    private void Start()
    {
        botaoAnterior.onClick.AddListener(Anterior);
        botaoProximo.onClick.AddListener(Proximo);
        botaoDiario.onClick.AddListener(AbrirDiario);
        botaoVoltar.onClick.AddListener(Voltar);

        AtualizarLivro();
    }

    private void Anterior()
    {
        livroAtual--;

        if (livroAtual < 0)
        {
            livroAtual = DadosPuzzleLivros.Titulos.Length - 1;
        }
        AtualizarLivro();
    }

    private void Proximo()
    {
        livroAtual++;

        if (livroAtual >= DadosPuzzleLivros.Titulos.Length){
            livroAtual = 0;
        }
        AtualizarLivro();
    }

    private void AtualizarLivro()
    {
        nomeLivro.text =
            DadosPuzzleLivros.Titulos[livroAtual];

        descricaoLivro.text =
            DadosPuzzleLivros.Descricoes[livroAtual];

        contadorLivro.text = (livroAtual + 1) + " / " + DadosPuzzleLivros.Titulos.Length;

        if (livroAtual < capas.Length &&
            capas[livroAtual] != null)
        {
            capaLivro.sprite = capas[livroAtual];
            capaLivro.enabled = true;
        }
        else
        {
            capaLivro.enabled = false;
        }
    }

    private void AbrirDiario()
    {
        SceneManager.LoadScene("PuzzleDiario");
    }
    private void Voltar()
    {
        PlayerPrefs.SetString(
            "NomePontoEntrada", pontoRetorno
        );

        PlayerPrefs.SetInt("VeioDeOutraCena", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }
}
