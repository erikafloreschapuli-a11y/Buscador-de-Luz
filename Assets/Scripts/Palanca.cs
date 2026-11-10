using UnityEngine;

public class Palanca : MonoBehaviour
{
    public Compuerta compuerta;
    public Sprite spriteInactivo;
    public Sprite spriteActivo;
    private bool activada = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteInactivo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activada)
        {
            activada = true;
            sr.sprite = spriteActivo;
            compuerta.Abrir();
            Debug.Log("Palanca activada");
        }
    }
}