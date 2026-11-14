using UnityEngine;

public class DañoContacto : MonoBehaviour
{
    public int daño = 1;

    [Tooltip("Segundos mínimos entre daños consecutivos (RF-15: 1 s)")]
    public float tiempoEntreDaño = 1f;

    private float timerDaño;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        timerDaño -= Time.deltaTime;
        if (timerDaño > 0f) return;

        PlayerHealth vida = other.GetComponent<PlayerHealth>();
        if (vida == null) return;

        vida.RecibirDaño(daño);
        timerDaño = tiempoEntreDaño;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            timerDaño = 0f;
    }
}
