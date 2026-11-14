using System.Collections;
using UnityEngine;

/// <summary>
/// RF-24: IA de Molrak. Fase 1: embestida y puñetazo.
/// Fase 2: lluvia de rocas y salto de impacto.
/// Fase 3: explosión de lava y embestidas más rápidas.
/// El Animator es opcional: si no hay clips, el jefe funciona igual.
/// </summary>
public class MolrakIA : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public float velocidadEmbestida = 8f;
    public float rangoAtaque = 2.4f;
    public float rangoDeteccion = 16f;

    [Header("Ataques")]
    public float intervaloAtaque = 2.4f;
    public int dañoContacto = 1;
    public float dañoDeArea = 1;
    public float radioGolpe = 3f;
    public float radioExplosion = 4.5f;
    public float fuerzaSalto = 9f;

    [Header("Lluvia de rocas (Fase 2)")]
    public GameObject rocaPrefab;
    public int rocasPorRafaga = 5;
    public float anchoLluvia = 10f;

    [Header("Efectos")]
    public GameObject efectoGolpe;
    public GameObject efectoExplosion;
    public AudioClip sonidoGolpe;
    public AudioClip sonidoEmbestida;
    public AudioClip sonidoRugido;

    [Header("Estado")]
    [Tooltip("ZonaCombateJefe lo activa cuando Kael entra a la arena")]
    public bool activo = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private MolrakHealth salud;
    private Transform kael;
    private PlayerHealth kaelVida;
    private float timerAtaque;
    private bool atacando;

    public bool EstaMuerto => salud != null && salud.EstaMuerto;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        salud = GetComponent<MolrakHealth>();

        if (rb != null) rb.freezeRotation = true;

        BuscarKael();

        if (salud != null)
            salud.AlCambiarFase += ReaccionarCambioFase;

        timerAtaque = intervaloAtaque;
    }

    private void OnDestroy()
    {
        if (salud != null)
            salud.AlCambiarFase -= ReaccionarCambioFase;
    }

    private void BuscarKael()
    {
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            kael = jugador.transform;
            kaelVida = jugador.GetComponent<PlayerHealth>();
        }
    }

    public void Activar()
    {
        activo = true;
        timerAtaque = 1f;
    }

    private void Update()
    {
        if (!activo || EstaMuerto) return;

        if (kael == null)
        {
            BuscarKael();
            if (kael == null) return;
        }

        MirarHaciaKael();

        if (atacando) return;

        float distancia = Vector2.Distance(transform.position, kael.position);

        if (distancia > rangoDeteccion)
        {
            Frenar();
            return;
        }

        if (distancia > rangoAtaque)
            MoverHaciaKael();
        else
            Frenar();

        timerAtaque -= Time.deltaTime;
        if (timerAtaque <= 0f && distancia <= rangoDeteccion)
            StartCoroutine(ElegirAtaque());
    }

    private void MirarHaciaKael()
    {
        if (sr != null && kael != null)
            sr.flipX = kael.position.x < transform.position.x;
    }

    private void MoverHaciaKael()
    {
        float direccion = Mathf.Sign(kael.position.x - transform.position.x);

        if (rb != null)
            rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
        else
            transform.position += Vector3.right * direccion * velocidad * Time.deltaTime;

        if (animator != null) animator.SetBool("Walking", true);
    }

    private void Frenar()
    {
        if (rb != null)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (animator != null) animator.SetBool("Walking", false);
    }

    private IEnumerator ElegirAtaque()
    {
        atacando = true;
        Frenar();

        int fase = salud != null ? salud.Fase : 1;

        if (fase == 1)
        {
            if (Random.value < 0.5f) yield return Embestida();
            else yield return Puñetazo();
        }
        else if (fase == 2)
        {
            if (Random.value < 0.5f) yield return SaltoImpacto();
            else yield return LluviaDeRocas();
        }
        else
        {
            if (Random.value < 0.5f) yield return ExplosionDeLava();
            else yield return Embestida();
        }

        float multiplicador = fase >= 3 ? 0.65f : 1f;
        timerAtaque = intervaloAtaque * multiplicador;
        atacando = false;
    }

    private IEnumerator Embestida()
    {
        if (animator != null) animator.SetTrigger("Embestida");
        AudioManager.SFX(sonidoEmbestida, 0.8f, transform.position);

        yield return new WaitForSeconds(0.4f);

        float direccion = kael != null ? Mathf.Sign(kael.position.x - transform.position.x) : 1f;
        float duracion = 0.7f;
        float t = 0f;

        while (t < duracion)
        {
            t += Time.deltaTime;

            if (rb != null)
                rb.linearVelocity = new Vector2(direccion * velocidadEmbestida, rb.linearVelocity.y);
            else
                transform.position += Vector3.right * direccion * velocidadEmbestida * Time.deltaTime;

            yield return null;
        }

        Frenar();
        yield return new WaitForSeconds(0.35f);
    }

    private IEnumerator Puñetazo()
    {
        if (animator != null) animator.SetTrigger("Puñetazo");

        yield return new WaitForSeconds(0.45f);

        if (kael != null && Vector2.Distance(transform.position, kael.position) <= radioGolpe)
        {
            if (kaelVida != null)
                kaelVida.RecibirDaño(dañoContacto);

            AudioManager.SFX(sonidoGolpe, 1f, transform.position);
            if (efectoGolpe != null) Instantiate(efectoGolpe, kael.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.35f);
    }

    private IEnumerator SaltoImpacto()
    {
        if (animator != null) animator.SetTrigger("Salto");

        if (rb != null)
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.75f);

        if (kael != null && Vector2.Distance(transform.position, kael.position) <= radioGolpe)
        {
            if (kaelVida != null)
                kaelVida.RecibirDaño((int)dañoDeArea);
        }

        AudioManager.SFX(sonidoGolpe, 1f, transform.position);
        if (efectoGolpe != null) Instantiate(efectoGolpe, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator LluviaDeRocas()
    {
        if (animator != null) animator.SetTrigger("Lluvia");

        for (int i = 0; i < rocasPorRafaga; i++)
        {
            if (rocaPrefab != null && kael != null)
            {
                float x = kael.position.x + Random.Range(-anchoLluvia * 0.5f, anchoLluvia * 0.5f);
                Vector3 pos = new Vector3(x, transform.position.y + 8f, 0f);
                LluviaRocas.CrearRoca(rocaPrefab, pos, 5f);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.4f);
    }

    private IEnumerator ExplosionDeLava()
    {
        if (animator != null) animator.SetTrigger("Explosion");
        AudioManager.SFX(sonidoRugido, 1f, transform.position);

        yield return new WaitForSeconds(0.6f);

        if (efectoExplosion != null)
            Instantiate(efectoExplosion, transform.position, Quaternion.identity);

        if (kael != null && Vector2.Distance(transform.position, kael.position) <= radioExplosion)
        {
            if (kaelVida != null)
                kaelVida.RecibirDaño((int)dañoDeArea);
        }

        yield return new WaitForSeconds(0.6f);
    }

    private void ReaccionarCambioFase(int nuevaFase)
    {
        AudioManager.SFX(sonidoRugido, 1f, transform.position);

        if (animator != null)
            animator.SetTrigger("Fase" + nuevaFase);

        // Pequeño respiro entre fases para que el jugador pueda reaccionar
        StartCoroutine(PausaEntreFases());
    }

    private IEnumerator PausaEntreFases()
    {
        atacando = true;
        Frenar();
        yield return new WaitForSeconds(1.2f);
        atacando = false;
        timerAtaque = 1f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!activo || EstaMuerto) return;

        if (collision.collider.CompareTag("Player"))
            DañarPorContacto(collision.collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activo || EstaMuerto) return;

        if (collision.collider.CompareTag("Player"))
            DañarPorContacto(collision.collider);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!activo || EstaMuerto) return;

        if (other.CompareTag("Player"))
            DañarPorContacto(other);
    }

    private void DañarPorContacto(Collider2D jugador)
    {
        PlayerHealth vida = jugador.GetComponent<PlayerHealth>();
        if (vida != null)
            vida.RecibirDaño(dañoContacto);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioGolpe);
        Gizmos.color = new Color(1f, 0.4f, 0f);
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
