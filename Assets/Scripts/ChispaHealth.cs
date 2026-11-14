using UnityEngine;

public class ChispaHealth : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 2;

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

        ChispaIA ia = GetComponent<ChispaIA>();
        if (ia != null) ia.enabled = false;

        AudioManager.SFX(sonidoMuerte, 0.9f, transform.position);

        DatosJuego.SumarEnemigo();

        Destroy(gameObject, 0.5f);
    }
}
