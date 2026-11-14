using System.Collections;
using UnityEngine;

/// <summary>
/// RF-24: vida de Molrak con 3 fases. En la Fase 3 el núcleo queda expuesto
/// y recibe el doble de daño si se golpea en el punto marcado.
/// </summary>
public class MolrakHealth : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 24;

    [Header("Fases")]
    [Range(0.1f, 0.9f)] public float umbralFase2 = 0.66f;
    [Range(0.05f, 0.6f)] public float umbralFase3 = 0.33f;

    [Header("Núcleo expuesto (Fase 3)")]
    public Transform nucleo;
    public float radioNucleo = 0.9f;
    public float multiplicadorNucleo = 2f;

    [Header("Muerte")]
    [Tooltip("Prefab del Fragmento de Aethon que suelta al morir (RF-38)")]
    public GameObject fragmentoPrefab;
    public Transform puntoFragmento;
    public float retrasoFragmento = 1.2f;
    public AudioClip sonidoMuerte;
    public GameObject efectoMuerte;
    public float retrasoDesaparicion = 3f;

    [Header("Inicio")]
    [Tooltip("Mientras es true no recibe daño (para la entrada en escena)")]
    public bool invulnerableAlInicio = true;
    public float duracionInvulnerabilidad = 1f;

    public event System.Action<int> AlCambiarFase;
    public event System.Action AlMorir;

    private int vidaActual;
    private int fase = 1;
    private bool muerto;
    private bool invulnerable;
    private Animator animator;

    public int Fase => fase;
    public bool EstaMuerto => muerto;
    public float VidaNormalizada => vidaMaxima <= 0 ? 0f : (float)vidaActual / vidaMaxima;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        vidaActual = vidaMaxima;

        if (invulnerableAlInicio)
        {
            invulnerable = true;
            StartCoroutine(QuitarInvulnerabilidad(duracionInvulnerabilidad));
        }
    }

    private IEnumerator QuitarInvulnerabilidad(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        invulnerable = false;
    }

    public void RecibirDaño(int cantidad)
    {
        RecibirDaño(cantidad, transform.position);
    }

    /// <summary>punto = posición del golpe, para detectar el núcleo expuesto.</summary>
    public void RecibirDaño(int cantidad, Vector2 punto)
    {
        if (muerto || invulnerable || cantidad <= 0) return;

        // Si no se asignó un núcleo en el Inspector, en Fase 3 todo el cuerpo es punto débil
        bool golpeAlNucleo = fase >= 3 &&
                             (nucleo == null || Vector2.Distance(punto, nucleo.position) <= radioNucleo);

        int danoFinal = golpeAlNucleo
            ? Mathf.RoundToInt(cantidad * multiplicadorNucleo)
            : cantidad;

        vidaActual = Mathf.Max(0, vidaActual - danoFinal);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (vidaActual <= 0)
        {
            Morir();
            return;
        }

        float normalizada = VidaNormalizada;
        if (fase == 1 && normalizada <= umbralFase2)
            CambiarFase(2);
        else if (fase == 2 && normalizada <= umbralFase3)
            CambiarFase(3);
    }

    private void CambiarFase(int nuevaFase)
    {
        fase = nuevaFase;

        if (animator != null)
            animator.SetTrigger("Fase" + nuevaFase);

        AlCambiarFase?.Invoke(fase);
    }

    private void Morir()
    {
        if (muerto) return;
        muerto = true;

        if (animator != null)
            animator.SetTrigger("Death");

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;

        MolrakIA ia = GetComponent<MolrakIA>();
        if (ia != null) ia.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        AudioManager.SFX(sonidoMuerte, 1f, transform.position);

        if (efectoMuerte != null)
            Instantiate(efectoMuerte, transform.position, Quaternion.identity);

        DatosJuego.SumarEnemigo();
        AlMorir?.Invoke();

        StartCoroutine(SoltarFragmento());
        Destroy(gameObject, retrasoDesaparicion);
    }

    private IEnumerator SoltarFragmento()
    {
        yield return new WaitForSeconds(retrasoFragmento);

        if (fragmentoPrefab == null)
        {
            Debug.LogWarning("Molrak: falta asignar fragmentoPrefab para soltar el Fragmento de Aethon (RF-38).");
            yield break;
        }

        Vector3 pos = puntoFragmento != null ? puntoFragmento.position : transform.position;
        Instantiate(fragmentoPrefab, pos, Quaternion.identity);
    }
}
