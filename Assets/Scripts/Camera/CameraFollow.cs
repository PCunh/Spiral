
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jogador;
    public float suavidade = 5f;

    void LateUpdate()
    {
        if (jogador == null)
            return;

        Vector3 posicaoDesejada = new Vector3(
            jogador.position.x,
            jogador.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            suavidade * Time.deltaTime
        );
    }
}
