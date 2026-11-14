using UnityEngine;

/// <summary>
/// RF-34: puerta de la arena de Molrak. Se cierra al entrar y se abre al vencerlo.
/// </summary>
public class PuertaJefe : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteCerrada;
    public Sprite spriteAbierta;

    [Header("Estado inicial")]
    public bool empiezaCerrada = true;

    [Header("Sonido")]
    public AudioClip sonidoCerrar;
    public AudioClip sonidoAbrir;

    private bool cerrada;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (empiezaCerrada) Cerrar(true);
        else Abrir(true);
    }

    public void Cerrar(bool silencioso = false)
    {
        cerrada = true;

        if (sr != null && spriteCerrada != null) sr.sprite = spriteCerrada;
        if (col != null) col.enabled = true;

        if (!silencioso)
            AudioManager.SFX(sonidoCerrar, 1f, transform.position);
    }

    public void Abrir(bool silencioso = false)
    {
        cerrada = false;

        if (sr != null && spriteAbierta != null) sr.sprite = spriteAbierta;
        if (col != null) col.enabled = false;

        if (!silencioso)
            AudioManager.SFX(sonidoAbrir, 1f, transform.position);
    }

    public bool EstaCerrada => cerrada;
}
