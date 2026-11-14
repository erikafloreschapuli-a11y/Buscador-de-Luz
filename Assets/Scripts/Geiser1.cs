using UnityEngine;

/// <summary>
/// RF-26: güiser de fuego. Dispara una columna cada 3 s (tiempoEspera) y daña.
/// El animator y el collider de fuego son opcionales.
/// </summary>
public class Geiser1 : MonoBehaviour
{
    [Header("Tiempos")]
    public float tiempoEspera = 3f;
    public float tiempoActivo = 1f;

    [Header("Daño")]
    public int daño = 1;

    [Header("Referencias")]
    public GameObject coliderFuego;
    public AudioClip sonidoActivar;

    private Animator animator;
    private bool activo;
    private float timer;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (coliderFuego != null)
            coliderFuego.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!activo && timer >= tiempoEspera)
            Activar();
        else if (activo && timer >= tiempoActivo)
            Desactivar();
    }

    private void Activar()
    {
        activo = true;
        timer = 0f;

        if (animator != null) animator.SetBool("Activo", true);
        if (coliderFuego != null) coliderFuego.SetActive(true);

        AudioManager.SFX(sonidoActivar, 0.8f, transform.position);
    }

    private void Desactivar()
    {
        activo = false;
        timer = 0f;

        if (animator != null) animator.SetBool("Activo", false);
        if (coliderFuego != null) coliderFuego.SetActive(false);
    }
}
