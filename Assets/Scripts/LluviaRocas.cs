using UnityEngine;

/// <summary>
/// RF-32: lluvia de rocas volcánicas. Colócalo en un objeto vacío sobre la zona
/// y asigna un prefab de roca (basta con SpriteRenderer; el script añade
/// colisionador, Rigidbody2D y daño automáticamente).
/// </summary>
public class LluviaRocas : MonoBehaviour
{
    [Header("Rocas")]
    public GameObject rocaPrefab;
    public float intervalo = 2.5f;
    public int rocasPorRafaga = 1;
    public float anchoArea = 12f;
    public float alturaAparicion = 8f;
    public float tiempoVidaRoca = 5f;

    [Header("Activación")]
    public bool soloConJugadorCerca = true;
    public float rangoActivacion = 18f;

    private float timer;
    private Transform kael;

    private void Start()
    {
        BuscarKael();
        timer = intervalo;
    }

    private void BuscarKael()
    {
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null) kael = jugador.transform;
    }

    private void Update()
    {
        if (rocaPrefab == null) return;

        if (kael == null)
        {
            BuscarKael();
            if (kael == null) return;
        }

        if (soloConJugadorCerca && Mathf.Abs(kael.position.x - transform.position.x) > rangoActivacion)
            return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = intervalo;
        for (int i = 0; i < rocasPorRafaga; i++)
            SoltarUnaRoca();
    }

    private void SoltarUnaRoca()
    {
        float x = transform.position.x + Random.Range(-anchoArea * 0.5f, anchoArea * 0.5f);
        Vector3 pos = new Vector3(x, transform.position.y + alturaAparicion, 0f);
        CrearRoca(rocaPrefab, pos, tiempoVidaRoca);
    }

    /// <summary>Crea una roca lista para caer y dañar. La usan también las fases de Molrak.</summary>
    public static GameObject CrearRoca(GameObject prefab, Vector3 posicion, float tiempoVida)
    {
        if (prefab == null) return null;

        GameObject roca = Instantiate(prefab, posicion, Quaternion.identity);

        Rigidbody2D rb = roca.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = roca.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.6f;
            rb.freezeRotation = true;
        }

        Collider2D col = roca.GetComponent<Collider2D>();
        if (col == null)
        {
            CircleCollider2D circulo = roca.AddComponent<CircleCollider2D>();
            circulo.radius = 0.35f;
            col = circulo;
        }
        col.isTrigger = true;

        if (roca.GetComponent<DañoContacto>() == null)
        {
            DañoContacto daño = roca.AddComponent<DañoContacto>();
            daño.daño = 1;
            daño.tiempoEntreDaño = 1f;
        }

        if (roca.GetComponent<AutoDestruir>() == null)
            roca.AddComponent<AutoDestruir>();

        Destroy(roca, tiempoVida);
        return roca;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.35f, 0.1f, 0.7f);
        Vector3 centro = transform.position + Vector3.up * alturaAparicion;
        Gizmos.DrawWireCube(centro, new Vector3(anchoArea, 0.3f, 0f));
        Gizmos.DrawLine(transform.position, centro);
    }
}
