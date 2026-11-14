using System.Collections;
using UnityEngine;

/// <summary>
/// RF-28: muro de roca. Los normales se rompen a golpes (J); los reforzados
/// solo con el Pulso de Calor (Q) y se ven de color rojizo.
/// </summary>
public class MuroRompible : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spriteIntacto;
    public Sprite spriteRoto;

    [Header("Efectos")]
    public AudioClip sonidoRotura;
    public GameObject efectoRotura;
    public float retrasoDesaparicion = 0.5f;

    [Header("Cómo se rompe")]
    [Tooltip("Activo: también se rompe a golpes con J. Inactivo: muro reforzado, solo con el Pulso de Calor (Q)")]
    public bool seRompeConAtaque = false;
    [Tooltip("Golpes de J necesarios si se rompe con ataque")]
    public int golpesNecesarios = 3;
    [Tooltip("Color del muro reforzado para que el jugador lo distinga")]
    public Color colorReforzado = new Color(1f, 0.55f, 0.45f);

    [Header("Opciones")]
    [Tooltip("Si está activo, el muro se destruye del todo al romperse")]
    public bool destruirObjeto = true;

    private bool roto;
    private int golpes;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (sr != null && spriteIntacto != null)
            sr.sprite = spriteIntacto;

        if (sr != null && !seRompeConAtaque)
            sr.color = colorReforzado;
    }

    /// <summary>Lo llama PlayerCombat al golpear con J.</summary>
    public void RecibirGolpe()
    {
        if (roto) return;
        StartCoroutine(Sacudir());
        if (!seRompeConAtaque) return;   // reforzado: solo el Pulso lo rompe

        golpes++;
        if (golpes >= golpesNecesarios)
            Romper();
    }

    private IEnumerator Sacudir()
    {
        Vector3 origen = transform.position;
        for (int i = 0; i < 6; i++)
        {
            transform.position = origen + new Vector3((i % 2 == 0 ? 0.08f : -0.08f), 0f, 0f);
            yield return new WaitForSeconds(0.03f);
        }
        transform.position = origen;
    }

    /// <summary>Lo llama PulsoDeCalor.</summary>
    public void Romper()
    {
        if (roto) return;
        roto = true;

        if (sr != null && spriteRoto != null)
            sr.sprite = spriteRoto;

        if (col != null)
            col.enabled = false;

        AudioManager.SFX(sonidoRotura, 1f, transform.position);

        if (efectoRotura != null)
            Instantiate(efectoRotura, transform.position, Quaternion.identity);

        if (destruirObjeto)
            Destroy(gameObject, retrasoDesaparicion);
    }

    public bool EstaRoto => roto;
}
