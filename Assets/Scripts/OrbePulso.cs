using UnityEngine;

/// <summary>
/// Premio de la zona secreta de Ignar: desbloquea el Pulso de Calor antes de
/// pelear con Molrak. Es opcional: si el jugador no lo encuentra, el Pulso se
/// desbloquea igual al recoger el Fragmento de Aethon.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class OrbePulso : MonoBehaviour
{
    [Header("Flotar")]
    public float amplitud = 0.25f;
    public float velocidad = 2f;

    [Header("Audio / efecto (opcional)")]
    public AudioClip sonidoRecoger;
    public GameObject efectoRecoger;

    private Vector3 inicio;
    private bool recogido;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        inicio = transform.position;
    }

    private void Update()
    {
        transform.position = inicio + Vector3.up * Mathf.Sin(Time.time * velocidad) * amplitud;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido || !other.CompareTag("Player")) return;
        recogido = true;

        PulsoDeCalor pulso = other.GetComponent<PulsoDeCalor>();
        if (pulso != null)
            pulso.Desbloquear();          // activa la Q al instante y enciende el ícono del HUD
        else
        {
            DatosJuego.PulsoDesbloqueado = true;
            DatosJuego.Guardar();
            if (HUDManager.instance != null) HUDManager.instance.MostrarHabilidad(true);
        }

        AudioManager.SFX(sonidoRecoger, 1f, transform.position);
        if (efectoRecoger != null) Destroy(Instantiate(efectoRecoger, transform.position, Quaternion.identity), 1f);
        Destroy(gameObject);
    }
}
