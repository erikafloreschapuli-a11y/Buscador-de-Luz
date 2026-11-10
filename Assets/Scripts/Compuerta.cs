using UnityEngine;

public class Compuerta : MonoBehaviour
{
    public Sprite spriteCerrada;
    public Sprite spriteAbierta;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        sr.sprite = spriteCerrada;
    }

    public void Abrir()
    {
        sr.sprite = spriteAbierta;
        col.enabled = false;
        Debug.Log("Compuerta abierta");
    }
}