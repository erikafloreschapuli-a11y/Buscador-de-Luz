using UnityEngine;

/// <summary>
/// RF-27: compuerta que bloquea el paso hasta activar una palanca o resolver un puzzle.
/// </summary>
public class Compuerta : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteCerrada;
    public Sprite spriteAbierta;

    [Header("Audio")]
    public AudioClip sonidoAbrir;

    private SpriteRenderer sr;
    private Collider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (sr != null && spriteCerrada != null)
            sr.sprite = spriteCerrada;
    }

    public void Abrir()
    {
        if (sr != null && spriteAbierta != null)
            sr.sprite = spriteAbierta;

        if (col != null)
            col.enabled = false;

        AudioManager.SFX(sonidoAbrir, 0.9f, transform.position);
    }
}
