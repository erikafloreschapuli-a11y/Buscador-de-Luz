using UnityEngine;

public class Geiser1 : MonoBehaviour
{
    public float tiempoEspera = 3f;
    public float tiempoActivo = 1f;
    public int daño = 1;
    public GameObject coliderFuego;

    private Animator animator;
    private bool activo = false;
    private float timer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        coliderFuego.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!activo && timer >= tiempoEspera)
        {
            Activar();
        }
        else if (activo && timer >= tiempoActivo)
        {
            Desactivar();
        }
    }

    void Activar()
    {
        activo = true;
        timer = 0f;
        animator.SetBool("Activo", true);
        coliderFuego.SetActive(true);
        Debug.Log("Géiser activado");
    }

    void Desactivar()
    {
        activo = false;
        timer = 0f;
        animator.SetBool("Activo", false);
        coliderFuego.SetActive(false);
    }
}