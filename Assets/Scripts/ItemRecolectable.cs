using UnityEngine;

public enum TipoItem
{
    CristalPequeno, // RF-35: cura medio corazón
    CristalGrande,  // RF-36: cura un corazón
    CristalMejora   // RF-37: reduce el cooldown 0,5 s
}

/// <summary>
/// Cristales de Luz (RF-35/36/37). El objeto solo necesita un Collider2D:
/// el script lo vuelve trigger automáticamente para no depender del Inspector.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ItemRecolectable : MonoBehaviour
{
    [Header("Tipo")]
    public TipoItem tipo = TipoItem.CristalPequeno;

    [Header("Curación (solo cristales)")]
    [Tooltip("Medio corazón = 0,5 | Un corazón = 1")]
    public float cantidadCuracion = 0.5f;

    [Header("Mejora (solo Cristal de Mejora)")]
    public float reduccionCooldown = 0.5f;

    [Header("Presentación")]
    public AudioClip sonidoRecoger;
    public GameObject efectoRecoger;
    public float duracionEfecto = 0.3f;

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

        switch (tipo)
        {
            case TipoItem.CristalPequeno:
                Curar(other, cantidadCuracion);
                DatosJuego.SumarCristal();
                break;

            case TipoItem.CristalGrande:
                Curar(other, cantidadCuracion <= 0f ? 1f : cantidadCuracion);
                DatosJuego.SumarCristal();
                break;

            case TipoItem.CristalMejora:
                PlayerCombat combate = other.GetComponent<PlayerCombat>();
                if (combate != null)
                    combate.ReducirCooldown(reduccionCooldown);
                DatosJuego.SumarMejora();
                break;
        }

        AudioManager.SFX(sonidoRecoger, 0.8f, transform.position);

        if (efectoRecoger != null)
        {
            GameObject efecto = Instantiate(efectoRecoger, transform.position, Quaternion.identity);
            Destroy(efecto, duracionEfecto + 1f);
        }

        // Se oculta de inmediato para que no pueda recogerse dos veces
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D colision = GetComponent<Collider2D>();
        if (colision != null) colision.enabled = false;

        Destroy(gameObject, duracionEfecto);
    }

    private void Curar(Collider2D jugador, float corazones)
    {
        PlayerHealth vida = jugador.GetComponent<PlayerHealth>();
        if (vida != null)
            vida.Curar(corazones);
    }
}
