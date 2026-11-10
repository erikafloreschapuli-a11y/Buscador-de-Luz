using UnityEngine;

public class DañoContacto : MonoBehaviour
{
    public int daño = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().RecibirDaño(daño);
        }
    }
}