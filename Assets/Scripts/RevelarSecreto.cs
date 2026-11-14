using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Zona secreta: mientras Kael está dentro, la pared falsa / roca decorativa
/// se vuelve casi transparente para que se vea el interior. Al salir, se cierra.
/// Pon este componente en un objeto con un BoxCollider2D que cubra el interior.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class RevelarSecreto : MonoBehaviour
{
    [Tooltip("Tilemap decorativo que tapa la zona (opcional)")]
    public Tilemap tilemap;
    [Tooltip("Sprite que tapa la zona, por ejemplo la ParedFalsa (opcional)")]
    public SpriteRenderer sprite;

    [Range(0f, 1f)] public float alfaDentro = 0.1f;
    public float velocidad = 4f;

    private float actual = 1f;
    private float objetivo = 1f;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) objetivo = alfaDentro; }
    private void OnTriggerStay2D(Collider2D other)  { if (other.CompareTag("Player")) objetivo = alfaDentro; }
    private void OnTriggerExit2D(Collider2D other)  { if (other.CompareTag("Player")) objetivo = 1f; }

    private void Update()
    {
        if (Mathf.Approximately(actual, objetivo)) return;
        actual = Mathf.MoveTowards(actual, objetivo, velocidad * Time.deltaTime);
        if (tilemap != null) { Color c = tilemap.color; c.a = actual; tilemap.color = c; }
        if (sprite != null) { Color c = sprite.color; c.a = actual; sprite.color = c; }
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        if (c == null) return;
        Gizmos.color = new Color(0.6f, 0.3f, 1f, 0.35f);
        Gizmos.DrawCube(transform.position + (Vector3)c.offset, c.size);
    }
}
