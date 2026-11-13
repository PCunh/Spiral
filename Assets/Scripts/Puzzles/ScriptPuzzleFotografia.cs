using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPuzzleFotografia : MonoBehaviour
{
    [SerializeField] private Texture2D fotografia;
    [SerializeField] private RectTransform Tabuleiro4x4;
    [SerializeField] private Button ModeloPeca;

    [SerializeField] private Button botaoEmbaralhar;
    [SerializeField] private Button botaoDica;
    [SerializeField] private Button botaoVoltar;
    [SerializeField] private string cenaRetorno = "Game";
    [SerializeField] private string pontoRetorno = "PontoEntrada_Rotina";


    [SerializeField, Min(20)]

    public int lado = 4;
    public int total = 16;
    public int[] pecas = new int[16];
    public Button[] botoes = new Button[16];
    public TMP_Text[] numeros = new TMP_Text[16];
    public List<Sprite> sprites = new List<Sprite>();
    public bool mostrarDica = false;
    public bool concluido = false;
    public TMP_Text textoBotaoDica;

    private void Awake()
    {
        GridLayoutGroup grid = Tabuleiro4x4.GetComponent<GridLayoutGroup>();


        grid.constraint =
            GridLayoutGroup.Constraint.FixedColumnCount;

        grid.constraintCount = lado;
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;

        CriarSprites();
        CriarBotoes();

        if (botaoEmbaralhar != null)
            botaoEmbaralhar.onClick.AddListener(Reiniciar);

        if (botaoVoltar != null)
            botaoVoltar.onClick.AddListener(Voltar);

        if (botaoDica != null) {
            botaoDica.onClick.AddListener(AcaoBotaoDica);
            textoBotaoDica = botaoDica.GetComponentInChildren<TMP_Text>();
        }

        Reiniciar();
    }

    private void CriarSprites()
    {
        float largura = fotografia.width / (float)lado;
        float altura = fotografia.height / (float)lado;

        for (int linha = 0; linha < lado; linha++)
        {
            for (int coluna = 0; coluna < lado; coluna++)
            {
                Rect recorte = new Rect(
                    coluna * largura,
                    (lado - 1 - linha) * altura,
                    largura,
                    altura
                );

                Sprite sprite = Sprite.Create(
                    fotografia,
                    recorte,
                    new Vector2(0.5f, 0.5f),
                    100f
                );

                sprites.Add(sprite);
            }
        }
    }

    private void CriarBotoes()
    {
        ModeloPeca.gameObject.SetActive(false);

        for (int i = 0; i < total; i++)
        {
            int indice = i;

            Button novo = Instantiate(ModeloPeca, Tabuleiro4x4);

            novo.gameObject.name = "Posicao_" + (i + 1).ToString("00");

            novo.onClick.RemoveAllListeners();

            novo.onClick.AddListener(() => TentarMover(indice));

            CriarNumero(novo, i);

            novo.gameObject.SetActive(true);
            botoes[i] = novo;
        }
    }

    private void CriarNumero(Button botao, int indice)
    {
        GameObject objeto = new GameObject("NumeroDica", typeof(RectTransform));

        objeto.transform.SetParent(botao.transform, false);

        RectTransform rect = objeto.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-6, -4);
        rect.sizeDelta = new Vector2(55, 40);

        TextMeshProUGUI texto =
            objeto.AddComponent<TextMeshProUGUI>();

        texto.font = TMP_Settings.defaultFontAsset;
        texto.fontSize = 28;
        texto.fontStyle = FontStyles.Bold;
        texto.color = Color.red;
        texto.alignment = TextAlignmentOptions.Center;
        texto.raycastTarget = false;

        objeto.SetActive(false);

        numeros[indice] = texto;
    }



    public void Reiniciar()
    {
        for (int i = 0; i < total; i++)
            pecas[i] = i;

        pecas[total - 1] = -1;
        concluido = false;
        mostrarDica = false;

        int vazio = total - 1;
        int ultimoVazio = -1;

        for (int passo = 0;
             passo < 100;
             passo++)
        {
            List<int> opcoes = Vizinhos(vazio);

            if (opcoes.Count > 1)
                opcoes.Remove(ultimoVazio);

            int destino = opcoes[Random.Range(0, opcoes.Count)];

            pecas[vazio] = pecas[destino];
            pecas[destino] = -1;

            ultimoVazio = vazio;
            vazio = destino;
        }

        if (EstaResolvido()){
            int destino = Vizinhos(vazio)[0];

            pecas[vazio] = pecas[destino];
            pecas[destino] = -1;
        }

        if (textoBotaoDica != null) {
            textoBotaoDica.text = "Dica";
        }
           
        AtualizarTela();

    }

    private void TentarMover(int posicao)
    {
        if (concluido)
            return;

        int vazio = System.Array.IndexOf(pecas, -1);

        if (!SaoVizinhos(posicao, vazio))
            return;

        pecas[vazio] = pecas[posicao];
        pecas[posicao] = -1;

        if (EstaResolvido())
        {
            concluido = true;
            mostrarDica = false;
            PlayerPrefs.SetInt("PuzzleFoto", 1);
            PlayerPrefs.Save();
            if (textoBotaoDica != null) {
                textoBotaoDica.text = "VOLTAR";
            }
                

        }
        AtualizarTela();
    }

    private void AcaoBotaoDica()
    {
        if (concluido){
            Voltar();
            return;
        }
        mostrarDica = !mostrarDica;
        AtualizarTela();
    }

    private bool SaoVizinhos(int a, int b)
    {
        int la = a / lado;
        int ca = a % lado;

        int lb = b / lado;
        int cb = b % lado;

        return Mathf.Abs(la - lb) + Mathf.Abs(ca - cb) == 1;
    }

    private List<int> Vizinhos(int i)
    {
        List<int> resultado = new List<int>();

        int linha = i / lado;
        int coluna = i % lado;

        if (linha > 0)
            resultado.Add(i - lado);

        if (linha < lado - 1)
            resultado.Add(i + lado);

        if (coluna > 0)
            resultado.Add(i - 1);

        if (coluna < lado - 1)
            resultado.Add(i + 1);

        return resultado;
    }

    private bool EstaResolvido()
    {
        for (int i = 0; i < total - 1; i++)
        {
            if (pecas[i] != i)
                return false;
        }

        return pecas[total - 1] == -1;
    }

    private void AtualizarTela()
    {
        for (int i = 0; i < total; i++)
        {
            bool vazia = pecas[i] == -1;

            Image imagem = botoes[i].image;

            if (vazia && concluido){
                imagem.sprite = sprites[total - 1];
            }
            else{
                imagem.sprite = vazia ? null : sprites[pecas[i]];
            }
            imagem.color = vazia && !concluido ? new Color(0.07f, 0.04f, 0.05f, 1f) : Color.white;

            imagem.type = Image.Type.Simple;
            imagem.preserveAspect = false;

            botoes[i].interactable = !vazia && !concluido;

            if (!vazia && mostrarDica && !concluido){
                numeros[i].text = (pecas[i] + 1).ToString();
                numeros[i].gameObject.SetActive(true);
            }
            else{
                numeros[i].gameObject.SetActive(false);
            }
        }
    }

    public void Voltar()
    {
        PlayerPrefs.SetString(
            "NomePontoEntrada", pontoRetorno
        );

        PlayerPrefs.SetInt("VeioDeOutraCena", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(cenaRetorno);
    }
}
