using System.Collections;
using UnityEngine;

/// <summary>
/// RF-09: dash con 0,3 s de invencibilidad y 1,5 s de recarga.
/// Tecla por defecto: K (el sprint es Shift, ver PlayerMovement).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]
    public float velocidad = 14f;
    public float duracion = 0.3f;
    public float cooldown = 1.5f;
    public KeyCode tecla = KeyCode.K;

    [Header("Efectos")]
    public GameObject efectoDash;
    [Tooltip("Copias semitransparentes de Kael que quedan detrás durante el dash")]
    public bool estela = true;
    public float intervaloEstela = 0.04f;
    public float duracionEstela = 0.25f;
    public Color colorEstela = new Color(0.45f, 0.75f, 1f, 0.6f);
    public AudioClip sonidoDash;

    private Rigidbody2D rb;
    private PlayerMovement movimiento;
    private PlayerHealth vida;
    private Animator animator;
    private SpriteRenderer sr;
    private float timer;
    private bool dashing;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movimiento = GetComponent<PlayerMovement>();
        vida = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (dashing || timer > 0f) return;
        if (vida != null && vida.EstaMuerto) return;
        if (!Input.GetKeyDown(tecla)) return;

        StartCoroutine(EjecutarDash());
    }

    private IEnumerator EjecutarDash()
    {
        dashing = true;
        timer = cooldown;

        float direccion = Input.GetAxisRaw("Horizontal");

        if (Mathf.Approximately(direccion, 0f))
            direccion = transform.localScale.x >= 0f ? 1f : -1f;

        direccion = Mathf.Sign(direccion);

        if (movimiento != null) movimiento.enabled = false;
        if (rb != null) rb.linearVelocity = new Vector2(direccion * velocidad, 0f);

        // RF-09: invencibilidad durante el dash
        if (vida != null) vida.HacerInvencible(duracion);

        // Animación: estado Dash del Animator (la de correr al doble de velocidad)
        if (animator != null) animator.SetBool("Dashing", true);
        if (estela && sr != null) StartCoroutine(CrearEstela());

        AudioManager.SFX(sonidoDash, 0.8f, transform.position);

        if (efectoDash != null)
        {
            GameObject efecto = Instantiate(efectoDash, transform.position, Quaternion.identity);
            Destroy(efecto, 1f);
        }

        yield return new WaitForSeconds(duracion);

        if (rb != null) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (animator != null) animator.SetBool("Dashing", false);

        // No reactivar el movimiento si Kael murió durante el dash
        if (movimiento != null && (vida == null || !vida.EstaMuerto))
            movimiento.enabled = true;

        dashing = false;
    }
    private void OnDisable()
    {
        // Si el dash se corta (muerte, cambio de escena), que no se quede la animación
        if (animator != null) animator.SetBool("Dashing", false);
        dashing = false;
    }

    /// <summary>Deja copias de Kael que se desvanecen mientras dura el dash.</summary>
    private IEnumerator CrearEstela()
    {
        while (dashing)
        {
            GameObject copia = new GameObject("EstelaDash");
            copia.transform.SetPositionAndRotation(transform.position, transform.rotation);
            copia.transform.localScale = transform.lossyScale;
            SpriteRenderer c = copia.AddComponent<SpriteRenderer>();
            c.sprite = sr.sprite;
            c.flipX = sr.flipX;
            c.color = colorEstela;
            c.sortingLayerID = sr.sortingLayerID;
            c.sortingOrder = sr.sortingOrder - 1;
            StartCoroutine(Desvanecer(c));
            yield return new WaitForSeconds(intervaloEstela);
        }
    }

    private IEnumerator Desvanecer(SpriteRenderer c)
    {
        float t = 0f;
        Color inicial = c.color;
        while (t < duracionEstela && c != null)
        {
            t += Time.deltaTime;
            Color col = inicial; col.a = Mathf.Lerp(inicial.a, 0f, t / duracionEstela);
            c.color = col;
            yield return null;
        }
        if (c != null) Destroy(c.gameObject);
    }
}
