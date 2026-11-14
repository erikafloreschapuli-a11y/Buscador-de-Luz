using UnityEngine;

/// <summary>
/// RF-17/RF-39: Cristal de Checkpoint. Guarda la posición al tocarlo (no se recoge).
/// Coloca 3 en el nivel (uno por zona).
/// </summary>
public class Checkpoint : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteInactivo;
    public Sprite spriteActivo;

    [Header("Respawn")]
    [Tooltip("Altura extra sobre el checkpoint para que Kael no aparezca dentro del suelo")]
    public Vector3 offsetRespawn = new Vector3(0f, 1f, 0f);

    [Header("Audio (opcional)")]
    public AudioClip sonidoActivar;

    private bool activado;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && spriteInactivo != null)
            sr.sprite = spriteInactivo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || activado) return;

        activado = true;

        if (sr != null && spriteActivo != null)
            sr.sprite = spriteActivo;

        AudioManager.SFX(sonidoActivar, 0.8f, transform.position);

        if (GameManager.instance != null)
            GameManager.instance.SetCheckpoint(transform.position + offsetRespawn);
    }
}
