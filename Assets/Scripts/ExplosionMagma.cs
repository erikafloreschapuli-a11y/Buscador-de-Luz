using System.Collections;
using UnityEngine;

/// <summary>
/// RF-33: explosiones de magma aleatorias en el suelo (Zona 3).
/// Colócalo en el centro de la zona; el aviso aparece antes de la explosión.
/// </summary>
public class ExplosionMagma : MonoBehaviour
{
    [Header("Ritmo")]
    public float intervalo = 2.5f;
    public float variacionIntervalo = 1f;
    public float tiempoAviso = 0.8f;

    [Header("Área")]
    public float anchoArea = 12f;
    public float radioDaño = 1.2f;
    public int daño = 1;

    [Header("Efectos")]
    public GameObject avisoPrefab;
    public GameObject explosionPrefab;
    public AudioClip sonidoAviso;
    public AudioClip sonidoExplosion;

    private float timer;

    private void Start()
    {
        timer = intervalo;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = intervalo + Random.Range(-variacionIntervalo, variacionIntervalo);
        StartCoroutine(Explotar());
    }

    private IEnumerator Explotar()
    {
        float x = transform.position.x + Random.Range(-anchoArea * 0.5f, anchoArea * 0.5f);
        Vector3 pos = new Vector3(x, transform.position.y, 0f);

        AudioManager.SFX(sonidoAviso, 0.6f, pos);

        GameObject aviso = null;
        if (avisoPrefab != null)
        {
            aviso = Instantiate(avisoPrefab, pos, Quaternion.identity);
            Destroy(aviso, tiempoAviso + 0.5f);
        }

        yield return new WaitForSeconds(tiempoAviso);

        if (aviso != null) Destroy(aviso);

        AudioManager.SFX(sonidoExplosion, 1f, pos);
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        Collider2D[] alcanzados = Physics2D.OverlapCircleAll(pos, radioDaño);
        foreach (Collider2D col in alcanzados)
        {
            if (col == null || !col.CompareTag("Player")) continue;

            PlayerHealth vida = col.GetComponent<PlayerHealth>();
            if (vida != null) vida.RecibirDaño(daño);
            break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.6f);
        Vector3 izquierda = transform.position + Vector3.left * anchoArea * 0.5f;
        Vector3 derecha = transform.position + Vector3.right * anchoArea * 0.5f;
        Gizmos.DrawLine(izquierda, derecha);
        Gizmos.DrawWireSphere(transform.position, radioDaño);
    }
}
