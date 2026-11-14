using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Entrada estilo "Mario Bros": cuando la compuerta está abierta, si Kael está
/// frente a ella y se pulsa W / Flecha arriba, entra y aparece en el destino
/// (la zona secreta). Si no se pulsa nada, Kael sigue caminando normal.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PuertaSecreta : MonoBehaviour
{
    [Tooltip("Compuerta que debe estar abierta (con la palanca) para poder entrar. Vacío = siempre abierta")]
    public Compuerta compuerta;

    [Tooltip("Punto donde aparece Kael (dentro de la zona secreta)")]
    public Transform destino;

    [Header("Controles")]
    public KeyCode tecla = KeyCode.W;
    public KeyCode teclaAlterna = KeyCode.UpArrow;

    [Header("Efecto")]
    public float duracionFundido = 0.35f;
    public AudioClip sonidoEntrar;

    private bool dentro;
    private bool usando;
    private GameObject kael;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private bool Abierta()
    {
        if (compuerta == null) return true;
        Collider2D c = compuerta.GetComponent<Collider2D>();
        return c == null || !c.enabled;   // Compuerta.Abrir() apaga su collider
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        dentro = true;
        kael = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) dentro = false;
    }

    private void Update()
    {
        if (!dentro || usando || destino == null || kael == null || !Abierta()) return;
        if (Input.GetKeyDown(tecla) || Input.GetKeyDown(teclaAlterna))
            StartCoroutine(Entrar());
    }

    private IEnumerator Entrar()
    {
        usando = true;

        PlayerMovement mov = kael.GetComponent<PlayerMovement>();
        PlayerCombat com = kael.GetComponent<PlayerCombat>();
        PlayerDash dash = kael.GetComponent<PlayerDash>();
        Rigidbody2D rb = kael.GetComponent<Rigidbody2D>();
        SpriteRenderer sr = kael.GetComponent<SpriteRenderer>();

        if (mov != null) mov.enabled = false;
        if (com != null) com.enabled = false;
        if (dash != null) dash.enabled = false;
        RigidbodyType2D tipo = rb != null ? rb.bodyType : RigidbodyType2D.Dynamic;
        if (rb != null) { rb.linearVelocity = Vector2.zero; rb.bodyType = RigidbodyType2D.Kinematic; }

        AudioManager.SFX(sonidoEntrar, 0.8f, transform.position);

        // Desvanecer
        yield return Fundido(sr, 1f, 0f);

        // Teletransportar y avisar a la cámara para que no haga un barrido
        Vector3 antes = kael.transform.position;
        kael.transform.position = destino.position;
        Physics2D.SyncTransforms();
        foreach (CinemachineCamera cam in Object.FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Exclude))
            cam.OnTargetObjectWarped(kael.transform, destino.position - antes);

        // Aparecer
        yield return Fundido(sr, 0f, 1f);

        if (rb != null) { rb.bodyType = tipo; rb.linearVelocity = Vector2.zero; }
        if (mov != null) mov.enabled = true;
        if (com != null) com.enabled = true;
        if (dash != null) dash.enabled = true;

        dentro = false;
        usando = false;
    }

    private IEnumerator Fundido(SpriteRenderer sr, float desde, float hasta)
    {
        float t = 0f;
        while (t < duracionFundido)
        {
            t += Time.deltaTime;
            if (sr != null) { Color c = sr.color; c.a = Mathf.Lerp(desde, hasta, t / duracionFundido); sr.color = c; }
            yield return null;
        }
        if (sr != null) { Color c = sr.color; c.a = hasta; sr.color = c; }
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.4f);
        if (c != null) Gizmos.DrawCube(transform.position + (Vector3)c.offset, c.size);
        if (destino != null) { Gizmos.DrawLine(transform.position, destino.position); Gizmos.DrawWireSphere(destino.position, 0.5f); }
    }
}
