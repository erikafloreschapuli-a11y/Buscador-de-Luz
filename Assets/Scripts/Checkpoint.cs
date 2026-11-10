using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activado = false;
    private SpriteRenderer sr;
    public Sprite spriteInactivo;
    public Sprite spriteActivo;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteInactivo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            activado = true;
            sr.sprite = spriteActivo;
        
            GameManager.instance.SetCheckpoint(transform.position);
        }
    }
}