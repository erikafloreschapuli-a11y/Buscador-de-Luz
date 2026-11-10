using UnityEngine;

public class GolemitaIA : MonoBehaviour
{
    [Header("Patrulla")]
    public float velocidad = 2f;
    public float distanciaPatrulla = 5f;

    [Header("Detección de Kael")]
    public float rangoDeteccion = 4f;
    public float rangoAtaque = 1f;
    public int daño = 1;
    public float distanciaActivacion = 40f; // lejos de Kael no hace nada

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform kael;
    private PlayerHealth kaelVida;
    private Vector3 puntoInicio;
    private bool moviendoDerecha = true;
    private float timerDaño = 0f;
    private float tiempoEntreDaño = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            kael = jugador.transform;
            kaelVida = jugador.GetComponent<PlayerHealth>();
        }

        puntoInicio = transform.position;
    }

    private void Update()
    {
        if (kael == null)
        {
            Patrullar();
            return;
        }

        if (Mathf.Abs(transform.position.x - kael.position.x) > distanciaActivacion)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float distanciaKael = Vector2.Distance(transform.position, kael.position);

        if (distanciaKael <= rangoAtaque)
            Atacar();
        else if (distanciaKael <= rangoDeteccion)
            PerseguirKael();
        else
            Patrullar();
    }

    void Patrullar()
    {
        animator.SetBool("Walking", true);
        animator.SetBool("Attracking1", false);

        if (moviendoDerecha)
        {
            rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);
            sr.flipX = true;

            if (transform.position.x >= puntoInicio.x + distanciaPatrulla)
                moviendoDerecha = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocidad, rb.linearVelocity.y);
            sr.flipX = false;

            if (transform.position.x <= puntoInicio.x - distanciaPatrulla)
                moviendoDerecha = true;
        }
    }

    void PerseguirKael()
    {
        animator.SetBool("Walking", true);
        animator.SetBool("Attracking1", false);

        if (kael.position.x > transform.position.x)
        {
            rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);
            sr.flipX = true;
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocidad, rb.linearVelocity.y);
            sr.flipX = false;
        }
    }

    void Atacar()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool("Walking", false);
        animator.SetBool("Attracking1", true);

        timerDaño -= Time.deltaTime;
        if (timerDaño <= 0)
        {
            if (kaelVida != null)
                kaelVida.RecibirDaño(daño);
            timerDaño = tiempoEntreDaño;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}