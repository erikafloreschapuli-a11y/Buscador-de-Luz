using System.Collections;
using UnityEngine;

/// <summary>
/// Emboscada de Chispas Voladoras (RF-23): un punto invisible del nivel.
/// Cuando Kael pasa cerca aparecen Chispas, tiemblan como aviso y se lanzan
/// una por una EN LÍNEA RECTA hacia donde estaba Kael. Se pueden esquivar
/// saltando, con dash o moviéndose. Si fallan, siguen de largo y explotan solas.
/// </summary>
public class ChispaEmboscada : MonoBehaviour
{
    [Header("Chispas")]
    public GameObject chispaPrefab;
    public int cantidad = 3;

    [Header("Activación (zona alrededor del punto)")]
    public float anchoZona = 8f;
    public float altoZona = 7f;

    [Header("Aparición (relativa a Kael)")]
    public float distanciaDelante = 9f;
    public float alturaAparicion = 5f;
    public float duracionAparicion = 0.35f;

    [Header("Ataque (esquivable)")]
    public float tiempoAviso = 0.6f;          // tiemblan antes de lanzarse
    public float separacionAtaques = 0.5f;    // tiempo entre una chispa y la siguiente
    public float velocidadPicada = 13f;
    public float distanciaMaxima = 26f;       // si no pegó tras recorrer esto, explota

    private bool activada;
    private Transform kael;
    private Rigidbody2D kaelRb;

    private void Update()
    {
        if (activada) return;

        if (kael == null)
        {
            GameObject j = GameObject.FindWithTag("Player");
            if (j == null) return;
            kael = j.transform;
            kaelRb = j.GetComponent<Rigidbody2D>();
        }

        Vector2 d = kael.position - transform.position;
        if (Mathf.Abs(d.x) <= anchoZona && Mathf.Abs(d.y) <= altoZona)
            Activar();
    }

    private void Activar()
    {
        activada = true;
        if (chispaPrefab == null) return;

        float dir;
        if (kaelRb != null && Mathf.Abs(kaelRb.linearVelocity.x) > 0.1f)
            dir = Mathf.Sign(kaelRb.linearVelocity.x);
        else
            dir = Mathf.Sign(transform.position.x - kael.position.x + 0.01f);

        for (int i = 0; i < cantidad; i++)
        {
            float sep = (i - (cantidad - 1) * 0.5f) * 2.4f;
            Vector3 pos = kael.position + new Vector3(
                dir * (distanciaDelante + Mathf.Abs(sep) * 0.5f),
                alturaAparicion + sep * 0.6f,
                0f);

            GameObject g = Instantiate(chispaPrefab, pos, Quaternion.identity);
            StartCoroutine(Ataque(g, duracionAparicion + tiempoAviso + i * separacionAtaques));
        }
    }

    private IEnumerator Ataque(GameObject g, float espera)
    {
        Transform t = g.transform;
        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
        Animator an = g.GetComponent<Animator>();
        ChispaIA ia = g.GetComponent<ChispaIA>();
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        bool animOk = an != null && an.runtimeAnimatorController != null;
        Vector3 escala = t.localScale;

        // La IA normal no persigue: aquí controlamos el vuelo.
        // (El contacto con Kael sigue explotando y haciendo daño desde ChispaIA.)
        t.localScale = Vector3.zero;
        yield return null; // deja correr Start() de ChispaIA (busca a Kael para hacerle daño)
        if (g == null) yield break;
        if (ia != null) ia.enabled = false;
        if (rb != null) { rb.linearVelocity = Vector2.zero; rb.gravityScale = 0f; }

        // 1) Aparecer
        Color c = sr != null ? sr.color : Color.white;
        Vector3 base0 = t.position;
        float tiempo = 0f;
        while (tiempo < duracionAparicion)
        {
            if (g == null) yield break;
            float k = tiempo / duracionAparicion;
            t.localScale = escala * k;
            if (sr != null) sr.color = new Color(c.r, c.g, c.b, k);
            tiempo += Time.deltaTime;
            yield return null;
        }
        if (g == null) yield break;
        t.localScale = escala;
        if (sr != null) sr.color = c;

        // 2) Aviso: flota y tiembla mirando a Kael
        if (animOk) an.SetBool("Flying", true);
        float restante = espera - duracionAparicion;
        tiempo = 0f;
        while (tiempo < restante)
        {
            if (g == null || ChispaMuerta(g)) yield break;
            float temblor = tiempo > restante - tiempoAviso ? 0.12f : 0.03f;
            t.position = base0 + new Vector3(Random.Range(-temblor, temblor), Mathf.Sin(tiempo * 6f) * 0.25f + Random.Range(-temblor, temblor), 0f);
            if (sr != null && kael != null) sr.flipX = kael.position.x <= t.position.x;
            tiempo += Time.deltaTime;
            yield return null;
        }
        if (g == null || ChispaMuerta(g) || kael == null) yield break;

        // 3) Picada en línea recta hacia donde estaba Kael
        if (animOk) { an.SetBool("Flying", false); an.SetBool("Diving", true); }
        Vector2 direccion = ((Vector2)(kael.position - t.position)).normalized;
        if (sr != null) sr.flipX = direccion.x < 0f;
        float recorrido = 0f;
        while (recorrido < distanciaMaxima)
        {
            if (g == null || ChispaMuerta(g)) yield break;
            float paso = velocidadPicada * Time.deltaTime;
            t.position += (Vector3)(direccion * paso);
            recorrido += paso;
            yield return null;
        }

        // 4) Falló: explota sola sin daño
        if (g != null && ia != null) ia.ExplotarSinDaño();
    }

    private static bool ChispaMuerta(GameObject g)
    {
        Collider2D col = g.GetComponent<Collider2D>();
        return col == null || !col.enabled;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.8f);
        Gizmos.DrawWireCube(transform.position, new Vector3(anchoZona * 2f, altoZona * 2f, 0f));
    }
}
