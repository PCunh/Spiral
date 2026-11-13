
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptGradeLogica : MonoBehaviour
{
    [SerializeField] private Transform grade;
    [SerializeField] private Button modeloCelula;

    [SerializeField] private Button[] botoesCausas = new Button[3];

    [SerializeField] private Button botaoVerificar;
    [SerializeField] private Button botaoVoltar;
    [SerializeField] private Button botaoJornal;
    [SerializeField] private TMP_Text mensagem;

    [SerializeField] private string cenaRetorno = "Game";

    private readonly string[] pessoas = {"Antonio", "Beatriz", "Carlos"};

    private readonly string[] datas = { "05/10", "12/10", "19/10"};

    private readonly string[] causas = {"Causas naturais", "Queda acidental", "Acidente automobilistico"};

    private readonly int[,] solucao ={{ 0, -1, -1 },{ -1, 1, -1 },{ -1, -1, 2 }};

    private int[,] respostas = new int[3, 3];
    private Button[,] celulas = new Button[3, 3];

    private int linhaSelecionada = -1;
    private int colunaSelecionada = -1;

    private bool concluido = false;

    private void Start(){
        modeloCelula.gameObject.SetActive(false);

        concluido = PlayerPrefs.GetInt("PuzzleGradeLogica", 0) == 1;

        for (int l = 0; l < 3; l++){
            for (int c = 0; c < 3; c++){
                respostas[l, c] = PlayerPrefs.GetInt(ChaveCelula(l, c), -1);
            }
        }

        CriarGrade();

        for (int i = 0; i < botoesCausas.Length; i++){
            int indice = i;

            TMP_Text texto = botoesCausas[i].GetComponentInChildren<TMP_Text>();
            texto.text = causas[i];
            botoesCausas[i].onClick.AddListener(() => EscolherCausa(indice));
        }

        botaoVerificar.onClick.AddListener(Verificar);
        botaoVoltar.onClick.AddListener(Voltar);
        botaoJornal.onClick.AddListener(Jornal);

        botaoVerificar.interactable = !concluido;

        AtualizarGrade();
    }

    private void CriarGrade(){
        CriarCabecalho("");

        foreach (string data in datas){
            CriarCabecalho(data);
        }
            

        for (int linha = 0; linha < 3; linha++){
            CriarCabecalho(pessoas[linha]);
            for (int coluna = 0; coluna < 3; coluna++){
                int l = linha;
                int c = coluna;

                Button novo = Instantiate(modeloCelula, grade);

                novo.gameObject.name = "Celula_" + l + "_" + c;

                novo.onClick.RemoveAllListeners();
                novo.onClick.AddListener(() => SelecionarCelula(l, c));

                novo.gameObject.SetActive(true);
                celulas[l, c] = novo;
            }
        }
    }

    private void CriarCabecalho(string valor){
        Button novo = Instantiate(modeloCelula, grade);

        novo.gameObject.name = "Cabecalho_" + valor;
        novo.gameObject.SetActive(true);

        TMP_Text texto = novo.GetComponentInChildren<TMP_Text>();

        texto.text = valor;
        texto.fontSize = 20;
        texto.color = Color.black;

        novo.image.color = new Color(0.75f, 0.70f, 0.60f);

        novo.interactable = false;
    }

    private void SelecionarCelula(int linha, int coluna)
    {
        if (concluido){
            return;
        }
            

        linhaSelecionada = linha;
        colunaSelecionada = coluna;

        mensagem.text = "Selecione a causa da morte.";

        AtualizarGrade();
    }

    private void EscolherCausa(int causa)
    {
        if (concluido){
            return;
        }

        if (linhaSelecionada == -1){
            mensagem.text = "Selecione primeiro uma celula.";
            return;
        }

        for (int c = 0; c < 3; c++)
        {
            respostas[linhaSelecionada, c] = -1;
        }

        for (int l = 0; l < 3; l++)
        {
            respostas[l, colunaSelecionada] = -1;
        }

        for (int l = 0; l < 3; l++){
            for (int c = 0; c < 3; c++){
                if (respostas[l, c] == causa){
                    respostas[l, c] = -1;
                }   
            }
        }

        respostas[linhaSelecionada, colunaSelecionada] = causa;

        linhaSelecionada = -1;
        colunaSelecionada = -1;

        SalvarRespostas();
        AtualizarGrade();

        mensagem.text = "Resposta registrada.";
    }

    private void SalvarRespostas(){
        for (int l = 0; l < 3; l++) {
            for (int c = 0; c < 3; c++){
                PlayerPrefs.SetInt(ChaveCelula(l, c),respostas[l, c]);
            }
        }

        PlayerPrefs.Save();
    }

    private void AtualizarGrade(){
        for (int l = 0; l < 3; l++){
            for (int c = 0; c < 3; c++){
                Button botao = celulas[l, c];

                TMP_Text texto = botao.GetComponentInChildren<TMP_Text>();

                int resposta = respostas[l, c];

                texto.text = resposta == -1 ? "..." : causas[resposta];
                texto.fontSize = resposta == -1 ? 22 : 15;
                texto.color = Color.black;

                bool selecionada = linhaSelecionada == l && colunaSelecionada == c;

                botao.image.color = selecionada ? new Color(0.9f, 0.7f, 0.4f) : new Color(0.96f, 0.94f, 0.87f);

                botao.interactable = !concluido;
            }
        }
    }

    private void Verificar(){
        for (int l = 0; l < 3; l++){
            for (int c = 0; c < 3; c++){
                if (respostas[l, c] != solucao[l, c]){
                    mensagem.text = "As informacoes nao correspondem.";
                    return;
                }
            }
        }

        concluido = true;

        PlayerPrefs.SetInt("PuzzleGradeLogica", 1);
        PlayerPrefs.Save();

        mensagem.text = "Então ela...";

        botaoVerificar.interactable = false;
        AtualizarGrade();
    }

    private string ChaveCelula(int linha, int coluna){
        return "GradeCausa_" + linha + "_" + coluna;
    }

    private void Voltar(){
        SceneManager.LoadScene(cenaRetorno);
    }
    private void Jornal()
    {
        SceneManager.LoadScene("PuzzleJornal");
    }
}
