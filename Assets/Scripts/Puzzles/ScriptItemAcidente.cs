using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptItemAcidente : MonoBehaviour
{
    [SerializeField, Range(0, 4)]
    private int idObjeto;

    [SerializeField]
    private ScriptMensagemItemAcidente mensagemColeta;

    private bool jogadorPerto = false;

    private void Start()
    {
        if (DadosPuzzleAcidente.FoiColetado(idObjeto))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!jogadorPerto)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Coletar();
        }
    }

    private void Coletar()
    {
        if (DadosPuzzleAcidente.FoiColetado(idObjeto))
            return;

        DadosPuzzleAcidente.RegistrarColeta(idObjeto);


        if (mensagemColeta != null)
            mensagemColeta.Mostrar(idObjeto);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jogadorPerto = false;
    }
}
