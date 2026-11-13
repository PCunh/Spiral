using System.Collections;
using TMPro;
using UnityEngine;

public class ScriptMensagemItemAcidente : MonoBehaviour
{
    [SerializeField] private GameObject painelColeta;
    [SerializeField] private TMP_Text nomeObjeto;
    [SerializeField] private TMP_Text descricaoObjeto;
    [SerializeField] private float duracao = 5f;

    private Coroutine rotinaMensagem;

    private void Start()
    {
        painelColeta.SetActive(false);
    }

    public void Mostrar(int id)
    {
        if (rotinaMensagem != null)
            StopCoroutine(rotinaMensagem);

        nomeObjeto.text = DadosPuzzleAcidente.Nomes[id];
        descricaoObjeto.text = DadosPuzzleAcidente.Descricoes[id];

        painelColeta.SetActive(true);

        rotinaMensagem = StartCoroutine(EsconderDepois());
    }

    private IEnumerator EsconderDepois()
    {
        yield return new WaitForSeconds(duracao);

        painelColeta.SetActive(false);
        rotinaMensagem = null;
    }
}
