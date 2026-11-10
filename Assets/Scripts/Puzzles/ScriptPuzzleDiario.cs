
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptDiario : MonoBehaviour
{
    [SerializeField] private TMP_Text dataPagina;
    [SerializeField] private TMP_Text textoPagina;
    [SerializeField] private TMP_Text contadorPagina;
    [SerializeField] private Button botaoLacuna;
    [SerializeField] private Button[] botoesLivros = new Button[5];
    [SerializeField] private Button botaoAnterior;
    [SerializeField] private Button botaoProximo;
    [SerializeField] private Button botaoVerificar;
    [SerializeField] private Button botaoVoltar;
    [SerializeField] private TMP_Text mensagem;

    public int[] respostas = { -1, -1, -1, -1, -1 };

    public int paginaAtual = 0;
    public bool lacunaSelecionada = false;
    public bool concluido = false;

    public TMP_Text textoLacuna;
    public Image imagemLacuna;

    private void Start()
    {
        textoLacuna = botaoLacuna.GetComponentInChildren<TMP_Text>();

        imagemLacuna = botaoLacuna.GetComponent<Image>();

        botaoLacuna.onClick.AddListener(SelecionarLacuna);

        botaoAnterior.onClick.AddListener(PaginaAnterior);
        botaoProximo.onClick.AddListener(ProximaPagina);
        botaoVerificar.onClick.AddListener(VerificarRespostas);
        botaoVoltar.onClick.AddListener(Voltar);

        for (int i = 0; i < botoesLivros.Length; i++)
        {
            int indice = i;

            TMP_Text texto =
                botoesLivros[i].GetComponentInChildren<TMP_Text>();

            if (texto != null)
                texto.text = DadosPuzzleLivros.Titulos[i];

            botoesLivros[i].onClick.AddListener(
                () => EscolherLivro(indice)
            );
        }

        CarregarRespostas();

        concluido = PlayerPrefs.GetInt("PuzzleLivrosConcluido", 0) == 1;

        mensagem.text = "";

        AtualizarPagina();
    }

    private void CarregarRespostas()
    {
        respostas[0] = PlayerPrefs.GetInt("DiarioLivro_0", -1);
        respostas[1] = PlayerPrefs.GetInt("DiarioLivro_1", -1);
        respostas[2] = PlayerPrefs.GetInt("DiarioLivro_2", -1);
        respostas[3] = PlayerPrefs.GetInt("DiarioLivro_3", -1);
        respostas[4] = PlayerPrefs.GetInt("DiarioLivro_4", -1);
    }

    private void SalvarRespostas()
    {
        PlayerPrefs.SetInt("DiarioLivro_0", respostas[0]);
        PlayerPrefs.SetInt("DiarioLivro_1", respostas[1]);
        PlayerPrefs.SetInt("DiarioLivro_2", respostas[2]);
        PlayerPrefs.SetInt("DiarioLivro_3", respostas[3]);
        PlayerPrefs.SetInt("DiarioLivro_4", respostas[4]);
        PlayerPrefs.Save();
    }

    private void SelecionarLacuna()
    {
        if (concluido){
            return;
        }
            
        lacunaSelecionada = true;
        mensagem.text = "Escolha um dos livro.";

        AtualizarLacuna();
    }

    private void EscolherLivro(int indiceLivro)
    {
        if (concluido)
            return;

        if (!lacunaSelecionada){
            mensagem.text = "";
            return;
        }

        for (int i = 0; i < respostas.Length; i++)
        {
            if (i != paginaAtual && respostas[i] == indiceLivro){
                respostas[i] = -1;
            }
        }

        respostas[paginaAtual] = indiceLivro;

        lacunaSelecionada = false;
        mensagem.text = "Livro selecionado.";

        SalvarRespostas();
        AtualizarPagina();
    }

    private void PaginaAnterior()
    {
        paginaAtual--;

        if (paginaAtual < 0) {
            paginaAtual = respostas.Length - 1;
        }
            

        lacunaSelecionada = false;
        mensagem.text = "";

        AtualizarPagina();
    }

    private void ProximaPagina()
    {
        paginaAtual++;

        if (paginaAtual >= respostas.Length) {
            paginaAtual = 0;
        }
            
        lacunaSelecionada = false;
        mensagem.text = "";

        AtualizarPagina();
    }

    private void AtualizarPagina()
    {
        dataPagina.text = DadosPuzzleLivros.Datas[paginaAtual];

        textoPagina.text = DadosPuzzleLivros.Paginas[paginaAtual];

        contadorPagina.text = "Pagina " + (paginaAtual + 1) + " / 5";

        botaoAnterior.interactable = !concluido;
        botaoProximo.interactable = !concluido;

        botaoLacuna.interactable = !concluido;

        botaoVerificar.gameObject.SetActive(!concluido);

        AtualizarLacuna();
    }

    private void AtualizarLacuna()
    {
        int indiceLivro = respostas[paginaAtual];

        if (textoLacuna != null)
        {
            textoLacuna.text = indiceLivro == -1 ? "Selecionar livro..." : DadosPuzzleLivros.Titulos[indiceLivro];
        }

        if (imagemLacuna != null)
        {
            imagemLacuna.color = lacunaSelecionada ? new Color(0.8f, 0.65f, 0.4f) : new Color(0.95f, 0.9f, 0.8f);
        }
    }

    private void VerificarRespostas()
    {
        for (int i = 0; i < respostas.Length; i++)
        {
            if (respostas[i] == -1)
            {
                mensagem.text = "Ainda existem paginas incompletas."; 
                return;
            }

            if (respostas[i] != DadosPuzzleLivros.Respostas[i])
            {
                mensagem.text = "Alguma lembranca nao corresponde ao diario."; 
                return;
            }
        }

        concluido = true;

        PlayerPrefs.SetInt("PuzzleLivrosConcluido", 1);
        PlayerPrefs.Save();

        paginaAtual = 3;

        mensagem.text = "Seu livro favorito, o que a inspirou.";

        AtualizarPagina();
    }

    private void Voltar()
    {
        SceneManager.LoadScene("Game");
    }

}
