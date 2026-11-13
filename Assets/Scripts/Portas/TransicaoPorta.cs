
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TransicaoPorta : MonoBehaviour
{
    public string nomeCenaDestino;
    public string nomePontoEntrada;

    [SerializeField] private float distanciaMaxima = 1.5f;

    private BloqueioPortaPuzzle bloqueio;
    private Transform jogador;
    private bool jogadorPerto;

    private void Awake()
    {
        bloqueio = GetComponent<BloqueioPortaPuzzle>();
    }

    private void Update()
    {
        if (!jogadorPerto || jogador == null)
            return;

        if (Keyboard.current == null ||
            !Keyboard.current.eKey.wasPressedThisFrame)
            return;

        float distancia = Vector2.Distance(
            jogador.position,
            transform.position
        );

        if (distancia > distanciaMaxima)
            return;

        if (!EhPortaMaisProxima())
            return;

        if (bloqueio != null && !bloqueio.PodeEntrar())
        {
            Debug.Log("Porta bloqueada: " + gameObject.name);
            return;
        }

        PlayerPrefs.SetString("NomePontoEntrada", nomePontoEntrada);
        PlayerPrefs.SetInt("VeioDeOutraCena", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nomeCenaDestino);
    }

    private bool EhPortaMaisProxima()
    {
        TransicaoPorta[] portas = FindObjectsByType<TransicaoPorta>(
            FindObjectsSortMode.None
        );

        float menorDistancia = float.MaxValue;
        TransicaoPorta maisProxima = null;

        foreach (TransicaoPorta porta in portas)
        {
            if (!porta.isActiveAndEnabled)
                continue;

            if (!porta.jogadorPerto)
                continue;

            float distancia = Vector2.Distance(
                jogador.position,
                porta.transform.position
            );

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProxima = porta;
            }
        }

        return maisProxima == this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            jogador = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            jogador = null;
        }
    }
}
