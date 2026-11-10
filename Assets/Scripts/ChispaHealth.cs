using UnityEngine;

public class ChispaHealth : MonoBehaviour
{
    public int vidaMaxima = 2;
    private int vidaActual;
    private Animator animator;

    private void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual -= cantidad;
        animator.SetTrigger("Hurt");

        if (vidaActual <= 0)
            Morir();
    }

    void Morir()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<ChispaIA>().enabled = false;
        Destroy(gameObject, 0.5f);
    }
}