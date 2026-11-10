using UnityEngine;

public class AutoDestruir : MonoBehaviour
{
    public float tiempoDeVida = 2f; // segundos que dura el rastro

    private void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }
}