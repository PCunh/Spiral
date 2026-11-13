
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TransicaoPorta : MonoBehaviour
{
    public string nomeCenaDestino;
    public string nomePontoEntrada;

    private bool jogadorPerto = false;

    private void Update()
    {
        if (jogadorPerto &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            
            PlayerPrefs.SetString("NomePontoEntrada", nomePontoEntrada);
            PlayerPrefs.SetInt("VeioDeOutraCena", 1);

            SceneManager.LoadScene(nomeCenaDestino);
        }
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
        {
            jogadorPerto = false;
        }
    }
}
