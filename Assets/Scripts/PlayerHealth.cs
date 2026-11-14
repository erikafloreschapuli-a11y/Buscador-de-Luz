using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [Tooltip("Corazones máximos (RF-07: 5)")]
    public int vidaMaxima = 5;

    [Header("Protección tras recibir daño")]
    [Tooltip("Segundos de invencibilidad y parpadeo tras cada golpe")]
    public float tiempoInvencibilidad = 1f;

    // La vida se guarda en medios corazones para poder curar 0,5 (RF-35)
    private int mediosActuales;
    private bool invencible;
    private bool muerto;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement movimiento;
    private PlayerCombat combate;

    private int MediosMaximos => vidaMaxima * 2;

    /// <summary>0 = sin vida, 1 = vida completa. Útil para fases de jefes.</summary>
    public float VidaNormalizada => MediosMaximos <= 0 ? 0f : (float)mediosActuales / MediosMaximos;

    private void Start()
    {
        mediosActuales = MediosMaximos;
        spriteRenderer = GetComponent<SpriteRenderer>();
        movimiento = GetComponent<PlayerMovement>();
        combate = GetComponent<PlayerCombat>();
        ActualizarHUD();
    }

    /// <summary>Daño en corazones completos (1 = un corazón).</summary>
    public void RecibirDaño(int corazones)
    {
        if (muerto || invencible || corazones <= 0) return;

        mediosActuales = Mathf.Clamp(mediosActuales - corazones * 2, 0, MediosMaximos);
        ActualizarHUD();

        if (mediosActuales <= 0)
            Morir();
        else
            StartCoroutine(Invencibilidad(tiempoInvencibilidad));
    }

    /// <summary>RF-35/36: cura en corazones (0,5 = medio corazón, 1 = un corazón).</summary>
    public void Curar(float corazones)
    {
        if (muerto || corazones <= 0f) return;

        mediosActuales = Mathf.Clamp(mediosActuales + Mathf.RoundToInt(corazones * 2f), 0, MediosMaximos);
        ActualizarHUD();
    }

    /// <summary>RF-09: el dash y otros efectos pueden dar invencibilidad temporal.</summary>
    public void HacerInvencible(float segundos)
    {
        if (muerto || segundos <= 0f) return;
        StopAllCoroutines();
        RestaurarAlpha();
        StartCoroutine(Invencibilidad(segundos));
    }

    private IEnumerator Invencibilidad(float duracion)
    {
        invencible = true;
        float t = 0f;
        const float parpadeo = 0.1f;

        while (t < duracion)
        {
            t += Time.deltaTime;
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = (Mathf.Repeat(t, parpadeo * 2f) < parpadeo) ? 1f : 0.35f;
                spriteRenderer.color = c;
            }
            yield return null;
        }

        RestaurarAlpha();
        invencible = false;
    }

    private void RestaurarAlpha()
    {
        if (spriteRenderer == null) return;
        Color c = spriteRenderer.color;
        c.a = 1f;
        spriteRenderer.color = c;
    }

    private void Morir()
    {
        if (muerto) return;
        muerto = true;

        StopAllCoroutines();
        RestaurarAlpha();
        invencible = false;

        if (movimiento != null) movimiento.enabled = false;
        if (combate != null) combate.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        gameObject.SetActive(false);

        // RF-45: Game Over con "Reintentar desde el último checkpoint"
        if (GameOverManager.Instance != null)
            GameOverManager.Instance.MostrarGameOver();
        else if (GameManager.instance != null)
            GameManager.instance.Invoke(nameof(GameManager.RespawnPlayer), 1f); // RF-08 de respaldo
    }

    /// <summary>Lo llama GameManager.RespawnPlayer() al reaparecer en el checkpoint.</summary>
    public void Revivir()
    {
        mediosActuales = MediosMaximos;
        muerto = false;
        invencible = false;
        StopAllCoroutines();
        RestaurarAlpha();

        if (movimiento != null) movimiento.enabled = true;
        if (combate != null) combate.enabled = true;

        ActualizarHUD();
    }

    private void ActualizarHUD()
    {
        if (HUDManager.instance != null)
            HUDManager.instance.ActualizarVida(mediosActuales, vidaMaxima);
    }

    public int GetMediosActuales() => mediosActuales;
    public int GetVida() => Mathf.CeilToInt(mediosActuales / 2f);
    public int GetVidaMaxima() => vidaMaxima;
    public bool EstaMuerto => muerto;
}
