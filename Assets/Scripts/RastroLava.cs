using UnityEngine;

public class RastroLava : MonoBehaviour
{
    public float duracion = 3f;
    public int daño = 1;
    private float timerDaño = 0f;
    private float tiempoEntreDaño = 1f;

    private void Start()
    {
        Destroy(gameObject, duracion);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Kael está tocando el fuego");
            timerDaño -= Time.deltaTime;
            if (timerDaño <= 0)
            {
                PlayerHealth vida = other.GetComponent<PlayerHealth>(); // NUEVO: validación
                if (vida != null)
                    vida.RecibirDaño(daño);

                timerDaño = tiempoEntreDaño;
            }
        }
    }
}