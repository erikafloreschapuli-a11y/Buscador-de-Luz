using UnityEngine;

/// <summary>
/// RF-34: arena de Molrak. Al entrar Kael se cierra la puerta y despierta el jefe.
/// Al morir Molrak, la puerta se abre y el Fragmento queda disponible.
/// Coloca este script en un objeto con Collider2D trigger que cubra la arena.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ZonaCombateJefe : MonoBehaviour
{
    [Header("Referencias")]
    public PuertaJefe puerta;
    public MolrakIA jefe;

    [Header("Música (opcional)")]
    public AudioClip musicaCombate;
    public AudioSource fuenteMusica;

    [Header("Opciones")]
    public bool cerrarPuertaAlEntrar = true;
    public float retrasoActivacion = 0.5f;

    private bool iniciado;
    private bool puertaAbierta;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void Start()
    {
        // Si no se enlazaron en el Inspector, se buscan solos en la escena
        if (jefe == null) jefe = FindAnyObjectByType<MolrakIA>();
        if (puerta == null) puerta = FindAnyObjectByType<PuertaJefe>();

        // Si el jefe ya estaba muerto (por ejemplo, al volver a la escena), no se bloquea
        if (jefe != null && jefe.EstaMuerto)
        {
            iniciado = true;
            puertaAbierta = true;
            if (puerta != null) puerta.Abrir(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (iniciado || !other.CompareTag("Player")) return;

        iniciado = true;
        StartCoroutine(IniciarCombate());
    }

    private System.Collections.IEnumerator IniciarCombate()
    {
        yield return new WaitForSeconds(retrasoActivacion);

        if (cerrarPuertaAlEntrar && puerta != null)
            puerta.Cerrar();

        if (jefe != null)
            jefe.Activar();

        if (musicaCombate != null && fuenteMusica != null)
        {
            fuenteMusica.clip = musicaCombate;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }
        else if (musicaCombate != null)
        {
            AudioManager.SFX(musicaCombate, 0.7f, transform.position);
        }
    }

    private void Update()
    {
        if (!iniciado || puertaAbierta || jefe == null) return;

        if (jefe.EstaMuerto)
        {
            puertaAbierta = true;
            if (puerta != null) puerta.Abrir();
        }
    }
}
