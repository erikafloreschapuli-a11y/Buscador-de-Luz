using UnityEngine;

/// <summary>
/// RF-27: palanca que abre una compuerta. Null-safe y con sonido.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Palanca : MonoBehaviour
{
    [Header("Referencias")]
    public Compuerta compuerta;

    [Header("Sprites")]
    public Sprite spriteInactivo;
    public Sprite spriteActivo;

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
        if (sr != null && spriteInactivo != null)
            sr.sprite = spriteInactivo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || activada) return;

        activada = true;

        if (sr != null && spriteActivo != null)
            sr.sprite = spriteActivo;

        AudioManager.SFX(sonidoActivar, 0.8f, transform.position);

        if (compuerta != null)
            compuerta.Abrir();
    }
}
