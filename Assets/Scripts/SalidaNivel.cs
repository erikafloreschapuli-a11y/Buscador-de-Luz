using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Puerta de salida al final de cada nivel (Ignar -> Zona2 -> Zona3).
/// Es independiente de la palanca: se abre sola cuando Kael llega,
/// guarda la nueva escena para "Continuar" y la carga.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class SalidaNivel : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena siguiente (debe estar en Build Settings)")]
    public string escenaSiguiente = "Zona2";

    [Header("Puerta (opcional)")]
    [Tooltip("Sprite de la puerta cerrada; si está vacío, la salida es invisible")]
    public Sprite spriteCerrada;
    public Sprite spriteAbierta;
    [Tooltip("Segundos con la puerta abierta antes de cargar el nivel")]
    public float tiempoAntesDeCargar = 0.9f;

    [Header("Audio (opcional)")]
    public AudioClip sonidoSalida;

    private bool usada;
    private SpriteRenderer sr;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && spriteCerrada != null) sr.sprite = spriteCerrada;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (usada || !other.CompareTag("Player")) return;
        usada = true;
        StartCoroutine(Salir());
    }

    private IEnumerator Salir()
    {
        if (sr != null && spriteAbierta != null) sr.sprite = spriteAbierta;
        AudioManager.SFX(sonidoSalida, 0.8f, transform.position);
        DatosJuego.GuardarInicioEscena(escenaSiguiente);
        Time.timeScale = 1f;

        // Kael entra por la puerta: sin control, camina al centro y se desvanece
        GameObject kael = GameObject.FindWithTag("Player");
        if (kael != null)
        {
            var mov = kael.GetComponent<PlayerMovement>();
            var com = kael.GetComponent<PlayerCombat>();
            var dash = kael.GetComponent<PlayerDash>();
            if (mov != null) mov.enabled = false;
            if (com != null) com.enabled = false;
            if (dash != null) dash.enabled = false;

            Rigidbody2D rb = kael.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.linearVelocity = Vector2.zero; rb.bodyType = RigidbodyType2D.Kinematic; }

            SpriteRenderer ksr = kael.GetComponent<SpriteRenderer>();
            Vector3 inicio = kael.transform.position;
            Vector3 escala = kael.transform.localScale;
            float centroX = (GetComponent<Collider2D>() != null) ? GetComponent<Collider2D>().bounds.center.x : transform.position.x;
            float pisoY = (GetComponent<Collider2D>() != null) ? GetComponent<Collider2D>().bounds.min.y : inicio.y;
            float altoPies = (kael.GetComponent<Collider2D>() != null) ? inicio.y - kael.GetComponent<Collider2D>().bounds.min.y : 0f;
            Vector3 destino = new Vector3(centroX, pisoY + altoPies, inicio.z);

            float t = 0f;
            while (t < tiempoAntesDeCargar)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / tiempoAntesDeCargar);
                kael.transform.position = Vector3.Lerp(inicio, destino, Mathf.Min(1f, k * 2f));
                kael.transform.localScale = escala * Mathf.Lerp(1f, 0.85f, k);
                if (ksr != null) { Color c = ksr.color; c.a = 1f - k; ksr.color = c; }
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(tiempoAntesDeCargar);
        }

        SceneManager.LoadScene(escenaSiguiente);
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        if (c == null) return;
        Gizmos.color = new Color(0.3f, 1f, 0.4f, 0.4f);
        Gizmos.DrawCube(transform.position + (Vector3)c.offset, c.size);
    }
}
