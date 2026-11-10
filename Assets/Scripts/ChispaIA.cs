using UnityEngine;

public class ChispaIA : MonoBehaviour
{
    [Header("Vuelo")]
    public float velocidad = 3f;
    public float amplitudOscilacion = 0.5f;
    public float frecuenciaOscilacion = 2f;

    [Header("Detección")]
    public float rangoDeteccion = 6f;
    public float rangoAtaque = 1.5f;
    public int daño = 1;
    public float distanciaActivacion = 40f; // lejos de Kael no hace nada

    [Header("Grupo")]
    public bool esLider = false;
    public ChispaIA[] grupoChispas;

    [Header("Explosión")]
    public GameObject efectoExplosion;
    public float tiempoDestruccion = 0.6f; // largo del clip Deathchispita

    private Animator animator;
    private SpriteRenderer sr;
    private Transform kael;
    private PlayerHealth kaelVida;
    private Vector3 posicionInicio;
    private float timerDaño = 0f;
    private float tiempoEntreDaño = 1f;
    private bool atacando = false;
    private float timerAtaque = 0f;
    private bool explotando = false;
    private bool animatorValido;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        posicionInicio = transform.position;
        animatorValido = animator != null && animator.runtimeAnimatorController != null;

        BuscarKael();
    }

    void BuscarKael()
    {
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            kael = jugador.transform;
            kaelVida = jugador.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (explotando) return;

        if (kael == null)
        {
            BuscarKael();
            if (kael == null) return;
        }

        if (Mathf.Abs(transform.position.x - kael.position.x) > distanciaActivacion)
            return;

        float distanciaKael = Vector2.Distance(transform.position, kael.position);

        if (atacando)
        {
            EjecutarPicada();
        }
        else if (distanciaKael <= rangoAtaque)
        {
            DañarKael();
        }
        else if (distanciaKael <= rangoDeteccion)
        {
            if (esLider)
                IniciarPicada();
            else
                Flotar();
        }
        else
        {
            Flotar();
        }
    }

    void Flotar()
    {
        if (animatorValido)
        {
            animator.SetBool("Flying", false);
            animator.SetBool("Diving", false);
        }

        float oscilar = Mathf.Sin(Time.time * frecuenciaOscilacion) * amplitudOscilacion;
        transform.position = new Vector3(
            posicionInicio.x,
            posicionInicio.y + oscilar,
            transform.position.z
        );
    }

    void IniciarPicada()
    {
        atacando = true;
        if (animatorValido)
        {
            animator.SetBool("Flying", false);
            animator.SetBool("Diving", true);
        }

        if (grupoChispas != null)
        {
            foreach (ChispaIA chispa in grupoChispas)
            {
                if (chispa != null && chispa != this)
                    chispa.UnirseAPicada();
            }
        }
    }

    public void UnirseAPicada()
    {
        atacando = true;
        if (animatorValido)
            animator.SetBool("Diving", true);
    }

    void EjecutarPicada()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            kael.position,
            velocidad * Time.deltaTime
        );

        if (sr != null)
            sr.flipX = kael.position.x <= transform.position.x;

        timerAtaque += Time.deltaTime;
        if (timerAtaque >= 2f)
        {
            atacando = false;
            timerAtaque = 0f;
            posicionInicio = transform.position;
            if (animatorValido)
                animator.SetBool("Diving", false);
        }
    }

    void DañarKael()
    {
        timerDaño -= Time.deltaTime;
        if (timerDaño <= 0)
        {
            if (kaelVida != null)
                kaelVida.RecibirDaño(daño);
            timerDaño = tiempoEntreDaño;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (explotando) return;

        if (other.CompareTag("Player"))
            Explotar();
    }

    void Explotar()
    {
        explotando = true;

        if (kaelVida != null)
            kaelVida.RecibirDaño(daño);

        if (efectoExplosion != null)
            Instantiate(efectoExplosion, transform.position, Quaternion.identity);

        if (animatorValido)
            animator.SetTrigger("Death");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, tiempoDestruccion);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}