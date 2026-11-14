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

    [Header("Terreno (no caer ni atorarse)")]
    public float distanciaPiso = 1.2f;   // qué tan abajo busca piso delante
    public float distanciaPared = 0.25f; // qué tan cerca detecta paredes

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool muertaEnLava;
    private SpriteRenderer sr;
    private Transform kael;
    private PlayerHealth kaelVida;
    private Vector3 puntoInicio;
    private bool moviendoDerecha = true;
    private float timerDaño = 0f;
    private float tiempoEntreDaño = 1f;

    private bool empujado;
    private float timerEmpuje;
    private Vector2 velocidadEmpuje;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (rb != null) rb.freezeRotation = true;

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            kael = jugador.transform;
            kaelVida = jugador.GetComponent<PlayerHealth>();
        }

        puntoInicio = transform.position;
    }

    /// <summary>RF-11: el Pulso de Calor empuja a la Golemita.</summary>
    public void Empujar(Vector2 fuerza, float duracion)
    {
        if (rb == null) return;

        velocidadEmpuje = fuerza;
        timerEmpuje = Mathf.Max(0.05f, duracion);
        empujado = true;

        if (animator != null)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Attracking1", false);
        }
    }

    private void Update()
    {
        if (empujado)
        {
            timerEmpuje -= Time.deltaTime;
            rb.linearVelocity = new Vector2(velocidadEmpuje.x, rb.linearVelocity.y);
            velocidadEmpuje = Vector2.Lerp(velocidadEmpuje, Vector2.zero, Time.deltaTime * 7f);

            if (timerEmpuje <= 0f)
            {
                empujado = false;
                puntoInicio = transform.position;
            }
            return;
        }

        if (kael == null)
        {
            Patrullar();
            return;
        }

        if (Mathf.Abs(transform.position.x - kael.position.x) > distanciaActivacion)
        {
            if (rb != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
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

    private void Patrullar()
    {
        if (animator != null)
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Attracking1", false);
        }

        if (rb == null) return;

        if (!PuedeAvanzar(moviendoDerecha ? 1f : -1f))
        {
            moviendoDerecha = !moviendoDerecha;
            if (!PuedeAvanzar(moviendoDerecha ? 1f : -1f))
            {
                Quieta();
                return;
            }
        }

        if (moviendoDerecha)
        {
            rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);
            if (sr != null) sr.flipX = true;

            if (transform.position.x >= puntoInicio.x + distanciaPatrulla)
                moviendoDerecha = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocidad, rb.linearVelocity.y);
            if (sr != null) sr.flipX = false;

            if (transform.position.x <= puntoInicio.x - distanciaPatrulla)
                moviendoDerecha = true;
        }
    }

    private void PerseguirKael()
    {
        if (animator != null)
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Attracking1", false);
        }

        if (rb == null) return;

        float dirKael = kael.position.x > transform.position.x ? 1f : -1f;
        if (sr != null) sr.flipX = dirKael > 0f;
        if (!PuedeAvanzar(dirKael))
        {
            Quieta();
            return;
        }

        if (kael.position.x > transform.position.x)
        {
            rb.linearVelocity = new Vector2(velocidad, rb.linearVelocity.y);
            if (sr != null) sr.flipX = true;
        }
        else
        {
            rb.linearVelocity = new Vector2(-velocidad, rb.linearVelocity.y);
            if (sr != null) sr.flipX = false;
        }
    }

    private void Atacar()
    {
        if (rb != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (animator != null)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Attracking1", true);
        }

        timerDaño -= Time.deltaTime;
        if (timerDaño <= 0)
        {
            if (kaelVida != null)
                kaelVida.RecibirDaño(daño);
            timerDaño = tiempoEntreDaño;
        }
    }

    private void Quieta()
    {
        if (rb != null) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (animator != null)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Attracking1", false);
        }
    }

    /// <summary>True si delante hay piso (que no sea lava) y no hay pared.</summary>
    private bool PuedeAvanzar(float dir)
    {
        if (col == null) return true;
        // en el aire no decide nada
        if (rb != null && Mathf.Abs(rb.linearVelocity.y) > 0.5f) return true;

        Bounds b = col.bounds;
        float frente = dir > 0f ? b.max.x : b.min.x;

        // pared: rayos a media altura y abajo
        for (int i = 0; i < 2; i++)
        {
            float y = i == 0 ? b.min.y + 0.4f : b.center.y;
            Collider2D pared = PrimerSolido(new Vector2(frente, y), new Vector2(dir, 0f), distanciaPared);
            if (pared != null && !pared.CompareTag("Lava")) return false;
        }

        // piso delante
        Collider2D piso = PrimerSolido(new Vector2(frente + dir * 0.3f, b.min.y + 0.1f), Vector2.down, distanciaPiso);
        return piso != null && !piso.CompareTag("Lava");
    }

    private Collider2D PrimerSolido(Vector2 origen, Vector2 dir, float dist)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origen, dir, dist);
        foreach (RaycastHit2D h in hits)
        {
            Collider2D c = h.collider;
            if (c == null || c == col || c.isTrigger) continue;
            if (c.CompareTag("Player")) continue;
            if (c.GetComponent<GolemitaIA>() != null) continue;
            return c;
        }
        return null;
    }

    // Si cae en la lava (por ejemplo empujada por el Pulso de Calor) muere
    private void OnCollisionEnter2D(Collision2D c) { RevisarLava(c.collider); }
    private void OnTriggerEnter2D(Collider2D c) { RevisarLava(c); }

    private void RevisarLava(Collider2D c)
    {
        if (muertaEnLava || c == null || !c.CompareTag("Lava")) return;
        muertaEnLava = true;
        GolemitaHealth vida = GetComponent<GolemitaHealth>();
        if (vida != null) vida.RecibirDaño(999);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
