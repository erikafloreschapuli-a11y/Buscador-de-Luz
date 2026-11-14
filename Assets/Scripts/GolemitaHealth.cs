using UnityEngine;

public class GolemitaHealth : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 3;

    [Header("Drop (RF-22)")]
    [Tooltip("Probabilidad en % de soltar un Cristal de Luz pequeño")]
    [Range(0f, 100f)] public float probabilidadDrop = 20f;
    public GameObject cristalPrefab;
    public Transform puntoDrop;
    public float alturaDrop = 0.5f;

    [Header("Efectos")]
    public AudioClip sonidoDaño;
    public AudioClip sonidoMuerte;

    private int vidaActual;
    private Animator animator;
    private bool muerto;

    private void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
    }

    public void RecibirDaño(int cantidad)
    {
        if (muerto) return;

        vidaActual -= cantidad;

        if (animator != null)
            animator.SetTrigger("Hurt");

        AudioManager.SFX(sonidoDaño, 0.7f, transform.position);

        if (vidaActual <= 0)
            Morir();
    }

    private void Morir()
    {
        if (muerto) return;
        muerto = true;

        if (animator != null)
            animator.SetTrigger("Death");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        GolemitaIA ia = GetComponent<GolemitaIA>();
        if (ia != null) ia.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        AudioManager.SFX(sonidoMuerte, 0.9f, transform.position);

        SoltarItem();
        DatosJuego.SumarEnemigo();

        Destroy(gameObject, 1f);
    }

    /// <summary>RF-22: 20 % de probabilidad de soltar un cristal pequeño.</summary>
    private void SoltarItem()
    {
        if (cristalPrefab == null) return;

        if (Random.Range(0f, 100f) >= probabilidadDrop) return;

        Vector3 pos = puntoDrop != null
            ? puntoDrop.position
            : transform.position + Vector3.up * alturaDrop;

        Instantiate(cristalPrefab, pos, Quaternion.identity);
    }
}
