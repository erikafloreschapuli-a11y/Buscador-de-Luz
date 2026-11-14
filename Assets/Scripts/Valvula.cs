using UnityEngine;

/// <summary>
/// RF-31: válvula del puzzle de la Zona 2. Se activa al tocarla.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Valvula : MonoBehaviour
{
    [Header("Puzzle")]
    public PuzzleValvulas puzzle;
    [Tooltip("Número de válvula (0, 1, 2...) para el orden correcto")]
    public int indice;

    [Header("Sprites")]
    public Sprite spriteInactiva;
    public Sprite spriteActiva;

    [Header("Audio")]
    public AudioClip sonidoActivar;

    private bool activada;
    private SpriteRenderer sr;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && spriteInactiva != null)
            sr.sprite = spriteInactiva;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Activar();
    }

    public void Activar()
    {
        if (activada) return;
        activada = true;

        if (sr != null && spriteActiva != null)
            sr.sprite = spriteActiva;

        AudioManager.SFX(sonidoActivar, 0.8f, transform.position);

        if (puzzle != null)
            puzzle.ActivarValvula(this);
    }

    /// <summary>El puzzle la llama cuando el orden es incorrecto.</summary>
    public void Reiniciar()
    {
        activada = false;

        if (sr != null && spriteInactiva != null)
            sr.sprite = spriteInactiva;
    }

    public bool EstaActiva => activada;
}
