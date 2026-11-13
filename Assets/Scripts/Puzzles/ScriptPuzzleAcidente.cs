using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPuzzleAcidente : MonoBehaviour
{
    [Header("Linha do tempo")]
    [SerializeField] private Button[] slots = new Button[5];

    [Header("Objetos coletados")]
    // Ordem: Ingresso, Cafe, Celular, Radio, Arvore
    [SerializeField] private Button[] botoesItens = new Button[5];

    [Header("Descricoes")]
    [SerializeField] private TMP_Text nomeObjeto;
    [SerializeField] private TMP_Text descricaoObjeto;
    [SerializeField] private TMP_Text mensagem;

    [Header("Controles")]
    [SerializeField] private Button botaoVerificar;
    [SerializeField] private Button botaoLimpar;
    [SerializeField] private Button botaoFechar;

    [Header("Cena de retorno")]
    [SerializeField] private string cenaRetorno = "Acidente";
    [SerializeField]
    private string pontoRetorno =
        "PontoEntrada_Acidente";

    private int[] ordem = { -1, -1, -1, -1, -1 };

    // A sequencia correta, usando os IDs dos itens.
    private readonly int[] solucao = { 0, 1, 2, 3, 4 };

    private int slotSelecionado = -1;
    private bool concluido;

    private void Start()
    {
        concluido = PlayerPrefs.GetInt(
            "PuzzleAcidenteConcluido", 0
        ) == 1;

        for (int i = 0; i < 5; i++)
        {
            ordem[i] = PlayerPrefs.GetInt(
                "AcidenteSlot_" + i, -1
            );
        }

        for (int i = 0; i < 5; i++)
        {
            int indice = i;

            slots[i].onClick.AddListener(
                () => SelecionarSlot(indice)
            );

            botoesItens[i].onClick.AddListener(
                () => SelecionarItem(indice)
            );
        }

        botaoVerificar.onClick.AddListener(Verificar);
        botaoLimpar.onClick.AddListener(Limpar);
        botaoFechar.onClick.AddListener(Fechar);

        botaoVerificar.interactable = !concluido;
        botaoLimpar.interactable = !concluido;

        mensagem.text = concluido
            ? "Memoria recuperada."
            : "Reconstrua a sequencia dos acontecimentos.";

        AtualizarTela();
    }

    private void SelecionarSlot(int indice)
    {
        if (concluido)
            return;

        slotSelecionado = indice;
        mensagem.text = "Escolha uma evidencia.";

        AtualizarTela();
    }

    private void SelecionarItem(int id)
    {
        if (!DadosPuzzleAcidente.FoiColetado(id))
            return;

        nomeObjeto.text = DadosPuzzleAcidente.Nomes[id];
        descricaoObjeto.text = DadosPuzzleAcidente.Descricoes[id];

        if (concluido)
            return;

        if (slotSelecionado == -1)
        {
            mensagem.text =
                "Selecione primeiro uma posicao da linha do tempo.";
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            if (ordem[i] == id)
                ordem[i] = -1;
        }

        ordem[slotSelecionado] = id;
        slotSelecionado = -1;

        SalvarOrdem();
        AtualizarTela();

        mensagem.text = "Evidencia posicionada.";
    }

    private void AtualizarTela()
    {
        for (int i = 0; i < 5; i++)
        {
            int id = ordem[i];

            TMP_Text texto =
                slots[i].GetComponentInChildren<TMP_Text>();

            bool disponivel =
                id >= 0 && id < DadosPuzzleAcidente.Total
                && DadosPuzzleAcidente.FoiColetado(id);

            texto.text = disponivel
                ? (i + 1) + ". " + DadosPuzzleAcidente.Nomes[id]
                : (i + 1) + ". Selecionar evidencia";

            bool selecionado = slotSelecionado == i;

            slots[i].image.color = selecionado
                ? new Color(0.75f, 0.55f, 0.35f)
                : new Color(0.85f, 0.80f, 0.70f);

            slots[i].interactable = !concluido;
        }

        for (int i = 0; i < 5; i++)
        {
            TMP_Text texto =
                botoesItens[i].GetComponentInChildren<TMP_Text>();

            bool coletado = DadosPuzzleAcidente.FoiColetado(i);

            texto.text = coletado
                ? DadosPuzzleAcidente.Nomes[i]
                : "Evidencia nao encontrada";

            botoesItens[i].interactable = coletado;
        }
    }

    private void SalvarOrdem()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(
                "AcidenteSlot_" + i, ordem[i]
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

        nomeObjeto.text = "";
        descricaoObjeto.text = "";

        SalvarOrdem();
        AtualizarTela();

        mensagem.text = "Linha do tempo limpa.";
    }

    private void Verificar()
    {
        if (concluido)
            return;

        if (!DadosPuzzleAcidente.TodosColetados())
        {
            mensagem.text =
                "Ainda faltam evidencias no local do acidente.";
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            if (ordem[i] != solucao[i])
            {
                mensagem.text =
                    "A sequencia dos acontecimentos esta incorreta.";
                return;
            }
        }

        concluido = true;

        PlayerPrefs.SetInt(
            "PuzzleAcidenteConcluido", 1
        );

        PlayerPrefs.Save();

        mensagem.text =
            "Memoria recuperada! O acidente foi reconstruido.";

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
}
