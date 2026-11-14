using System.Collections;
using UnityEngine;

/// <summary>
/// RF-30: puente que se derrumba al pisarlo (Zona 2).
/// Colócalo sobre una plataforma con Collider2D.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PuenteDerrumbable : MonoBehaviour
{
    [Header("Derrumbe")]
    public float retrasoAntesDeCaer = 0.7f;
    public float tiempoHastaDesaparecer = 2f;
    public float intensidadTemblor = 0.06f;

    [Header("Reaparición (opcional)")]
    public bool reaparece = true;
    public float tiempoReaparicion = 6f;

    [Header("Efectos")]
    public AudioClip sonidoCrujido;
    public AudioClip sonidoCaida;
    public GameObject efectoPolvo;

    private bool activado;
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            IniciarDerrumbe();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            IniciarDerrumbe();
    }

    private void IniciarDerrumbe()
    {
        if (activado) return;
        activado = true;
        StartCoroutine(Derrumbar());
    }

    private IEnumerator Derrumbar()
    {
        AudioManager.SFX(sonidoCrujido, 0.8f, transform.position);

        // Temblor de aviso
        float t = 0f;
        while (t < retrasoAntesDeCaer)
        {
            t += Time.deltaTime;
            transform.position = posicionInicial + (Vector3)(Random.insideUnitCircle * intensidadTemblor);
            yield return null;
        }
        transform.position = posicionInicial;

        if (col != null) col.enabled = false;

        AudioManager.SFX(sonidoCaida, 1f, transform.position);
        if (efectoPolvo != null)
            Instantiate(efectoPolvo, transform.position, Quaternion.identity);

        // Cae
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.8f;
            rb.freezeRotation = false;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1.8f;
        }

        rb.linearVelocity = new Vector2(Random.Range(-0.5f, 0.5f), 0f);

        yield return new WaitForSeconds(tiempoHastaDesaparecer);

        if (sr != null) sr.enabled = false;
        if (rb != null) rb.simulated = false;

        if (!reaparece)
        {
            Destroy(gameObject);
            yield break;
        }

        yield return new WaitForSeconds(tiempoReaparicion);

        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        if (rb != null)
        {
            rb.simulated = true;
            rb.bodyType = RigidbodyType2D.Static;
            rb.linearVelocity = Vector2.zero;
        }

        if (sr != null) sr.enabled = true;
        if (col != null) col.enabled = true;

        activado = false;
    }
}
