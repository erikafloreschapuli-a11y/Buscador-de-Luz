using UnityEngine;

/// <summary>
/// RF-38: Fragmento de Aethon. Lo suelta Molrak al morir.
/// Al recogerlo se completa Ignar, se desbloquea el Pulso de Calor (RF-11)
/// y se muestra la pantalla de "Fragmento obtenido" (RF-46).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class FragmentoAethon : MonoBehaviour
{
    [Header("Presentación")]
    public AudioClip sonidoRecoger;
    public GameObject efectoRecoger;

    private bool recogido;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido || !other.CompareTag("Player")) return;

        recogido = true;

        // Progreso permanente (RF-19)
        DatosJuego.ProgresoIgnar = 1;
        DatosJuego.PulsoDesbloqueado = true;
        DatosJuego.Guardar();

        // HUD: slot iluminado + icono de habilidad (RF-41/42)
        if (HUDManager.instance != null)
        {
            HUDManager.instance.MostrarFragmento(true);
            HUDManager.instance.MostrarHabilidad(true);
        }

        AudioManager.SFX(sonidoRecoger, 1f, transform.position);

        if (efectoRecoger != null)
            Instantiate(efectoRecoger, transform.position, Quaternion.identity);

        // Pantalla final (RF-46)
        if (PanelFragmento.Instance != null)
            PanelFragmento.Instance.Mostrar();
        else
            Debug.Log("¡Fragmento de Aethon obtenido! Ignar ha despertado.");

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D colision = GetComponent<Collider2D>();
        if (colision != null) colision.enabled = false;

        Destroy(gameObject, 0.1f);
    }
}
