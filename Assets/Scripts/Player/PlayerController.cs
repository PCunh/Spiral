
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float velocidade = 3f;

    private Rigidbody2D rb;
    private Vector2 movimento;
    private Vector2 ultimaDirecao = Vector2.down;

    public Vector2 UltimaDirecao => ultimaDirecao;
    public bool EstaMovendo => movimento != Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        float horizontal = 0f;
        float vertical = 0f;

        // Movimentação horizontal
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
            horizontal -= 1f;

        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
            horizontal += 1f;

        // Movimentação vertical
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
            vertical += 1f;

        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
            vertical -= 1f;

        movimento = new Vector2(horizontal, vertical).normalized;

        // Guarda a última direção para futuras animações
        if (movimento != Vector2.zero)
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                ultimaDirecao = new Vector2(Mathf.Sign(horizontal), 0f);
            else
                ultimaDirecao = new Vector2(0f, Mathf.Sign(vertical));
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movimento * velocidade;
    }
}
