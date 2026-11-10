using UnityEngine;

public class GolemitaHealth : MonoBehaviour
{
    public int vidaMaxima = 3;
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
        {
            Morir();
        }
    }

    void Morir()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<GolemitaIA>().enabled = false;
        SoltarItem();
        Destroy(gameObject, 1f);
    }

    void SoltarItem()
    {
        int random = Random.Range(0, 100);
        if (random < 20)
        {
           
            // Aquí instanciaremos el cristal después
        }
    }
}