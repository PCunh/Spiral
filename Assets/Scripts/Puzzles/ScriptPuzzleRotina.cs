
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPuzzleRotina : MonoBehaviour
{
    [Header("Linha do tempo")]
    [SerializeField] private Button[] slots = new Button[5];

    [Header("Botoes das acoes")]
    [SerializeField] private Button[] botoesRotinas = new Button[5];

    [Header("Mensagem")]
    [SerializeField] private TMP_Text mensagem;

    [Header("Controles")]
    [SerializeField] private Button botaoVerificar;
    [SerializeField] private Button botaoLimpar;
    [SerializeField] private Button botaoFechar;
    [SerializeField] private Button botaoRotina;

    [Header("Cena de retorno")]
    [SerializeField] private string cenaRetorno = "Game";
    [SerializeField] private string pontoRetorno = "PontoEntrada_Rotina";

    private readonly string[] nomes =
    {
        "Cafeteira",
        "Mandar E-mails",
        "Reuniao",
        "Impressora",
        "Registrar no Computador"
    };

    private readonly int[] solucao = { 0, 4, 2, 3, 1 };

    private int[] ordem = { -1, -1, -1, -1, -1 };

    private int slotSelecionado = -1;
    private bool concluido;

    private void Start()
    {
        concluido = PlayerPrefs.GetInt(
            "PuzzleRotinaConcluido", 0
        ) == 1;

        for (int i = 0; i < 5; i++)
        {
            ordem[i] = PlayerPrefs.GetInt(
                "RotinaSlot_" + i, -1
            );
        }

        for (int i = 0; i < 5; i++)
        {
            int indice = i;

            slots[i].onClick.AddListener(
                () => SelecionarSlot(indice)
            );

            botoesRotinas[i].onClick.AddListener(
                () => SelecionarRotina(indice)
            );
        }

        botaoVerificar.onClick.AddListener(Verificar);
        botaoLimpar.onClick.AddListener(Limpar);
        botaoFechar.onClick.AddListener(Fechar);
        botaoRotina.onClick.AddListener(Rotina);

        botaoVerificar.interactable = !concluido;
        botaoLimpar.interactable = !concluido;

        mensagem.text = concluido
            ? "Memoria recuperada."
            : "Organize a rotina de Clara na ordem correta.";

        AtualizarTela();
    }

    private void SelecionarSlot(int indice)
    {
        if (concluido)
            return;

        slotSelecionado = indice;
        mensagem.text = "Escolha uma acao da rotina.";

        AtualizarTela();
    }

    private void SelecionarRotina(int id)
    {
        if (concluido)
            return;

        if (slotSelecionado == -1)
        {
            mensagem.text =
                "Selecione primeiro uma posicao da rotina.";
            return;
        }

        // Remove a mesma acao de outra posicao.
        for (int i = 0; i < 5; i++)
        {
            if (ordem[i] == id)
                ordem[i] = -1;
        }

        ordem[slotSelecionado] = id;
        slotSelecionado = -1;

        SalvarOrdem();
        AtualizarTela();

        mensagem.text = "Acao posicionada.";
    }

    private void AtualizarTela()
    {
        // Atualiza os cinco slots.
        for (int i = 0; i < 5; i++)
        {
            int id = ordem[i];

            TMP_Text texto =
                slots[i].GetComponentInChildren<TMP_Text>();

            texto.text = id >= 0 && id < nomes.Length
                ? (i + 1) + ". " + nomes[id]
                : (i + 1) + ". Selecionar acao";

            bool selecionado = slotSelecionado == i;

            slots[i].image.color = selecionado
                ? new Color(0.75f, 0.55f, 0.35f)
                : new Color(0.85f, 0.80f, 0.70f);

            slots[i].interactable = !concluido;
        }

        // Atualiza os botoes das acoes.
        for (int i = 0; i < 5; i++)
        {
            TMP_Text texto =
                botoesRotinas[i].GetComponentInChildren<TMP_Text>();

            texto.text = nomes[i];
            botoesRotinas[i].interactable = !concluido;
        }
    }

    private void SalvarOrdem()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(
                "RotinaSlot_" + i, ordem[i]
            );
        }

        PlayerPrefs.Save();
    }

    private void Limpar()
    {
        if (concluido)
            return;

        for (int i = 0; i < 5; i++)
            ordem[i] = -1;

        slotSelecionado = -1;

        SalvarOrdem();
        AtualizarTela();

        mensagem.text = "Rotina limpa.";
    }

    private void Verificar()
    {
        if (concluido)
            return;

        for (int i = 0; i < 5; i++)
        {
            if (ordem[i] != solucao[i])
            {
                mensagem.text =
                    "A ordem da rotina esta incorreta.";
                return;
            }
        }

        concluido = true;

        PlayerPrefs.SetInt(
            "PuzzleRotinaConcluido", 1
        );

        PlayerPrefs.Save();

        mensagem.text =
            "Memoria recuperada! A rotina de Clara foi reconstruida.";

        botaoVerificar.interactable = false;
        botaoLimpar.interactable = false;

        AtualizarTela();
    }

    private void Fechar()
    {
        PlayerPrefs.SetString(
            "NomePontoEntrada", pontoRetorno
        );

        PlayerPrefs.SetInt("VeioDeOutraCena", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(cenaRetorno);
    }

    private void Rotina()
    {
        SceneManager.LoadScene("PuzzleRotina2");
    }
}
