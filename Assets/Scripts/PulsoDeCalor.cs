using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RF-11 / RF-28: Pulso de Calor. Onda que empuja enemigos y rompe muros.
/// Se desbloquea al obtener el Fragmento de Aethon (DatosJuego.PulsoDesbloqueado).
/// Tecla por defecto: Q.
/// </summary>
public class PulsoDeCalor : MonoBehaviour
{
    [Header("Uso")]
    public KeyCode tecla = KeyCode.Q;
    [Tooltip("Desactívalo si quieres probar el pulso antes de vencer a Molrak")]
    public bool requiereDesbloqueo = true;

    [Header("Onda")]
    public float radio = 3.5f;
    public float fuerzaEmpuje = 10f;
    public float duracionEmpuje = 0.25f;

    [Header("Recarga")]
    public float cooldownBase = 4f; // RF-11: 4 s
    public float cooldownMinimo = 1f;

    [Header("Efectos")]
    public GameObject efectoOnda;
    public AudioClip sonidoPulso;

    private float timer;
    private bool desbloqueado;

    private float CooldownActual =>
        Mathf.Max(cooldownMinimo, cooldownBase - DatosJuego.Mejoras * 0.5f);

    private void Start()
    {
        desbloqueado = !requiereDesbloqueo || DatosJuego.PulsoDesbloqueado;

        if (HUDManager.instance != null)
        {
            HUDManager.instance.MostrarHabilidad(desbloqueado);
            HUDManager.instance.ActualizarCooldown(1f);
        }
    }

    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;

        if (HUDManager.instance != null)
            HUDManager.instance.ActualizarCooldown(1f - Mathf.Clamp01(timer / CooldownActual));

        if (!desbloqueado || timer > 0f) return;

        if (Input.GetKeyDown(tecla))
            Usar();
    }

    /// <summary>Por si otra secuencia desbloquea el pulso sin pasar por el Fragmento.</summary>
    public void Desbloquear()
    {
        desbloqueado = true;
        DatosJuego.PulsoDesbloqueado = true;
        DatosJuego.Guardar();

        if (HUDManager.instance != null)
            HUDManager.instance.MostrarHabilidad(true);
    }

    public void Usar()
    {
        if (!desbloqueado) return;

        timer = CooldownActual;

        AudioManager.SFX(sonidoPulso, 1f, transform.position);

        if (efectoOnda != null)
        {
            GameObject onda = Instantiate(efectoOnda, transform.position, Quaternion.identity);
            Destroy(onda, 1.5f);
        }

        Collider2D[] alcanzados = Physics2D.OverlapCircleAll(transform.position, radio);
        HashSet<GolemitaIA> golemitasEmpujadas = new HashSet<GolemitaIA>();
        HashSet<ChispaIA> chispasEmpujadas = new HashSet<ChispaIA>();

        foreach (Collider2D col in alcanzados)
        {
            if (col == null) continue;

            MuroRompible muro = col.GetComponentInParent<MuroRompible>();
            if (muro != null)
            {
                muro.Romper();
                continue;
            }

            Vector2 direccion = (col.transform.position - transform.position);
            if (direccion.sqrMagnitude < 0.001f)
                direccion = Vector2.right;
            direccion.Normalize();

            GolemitaIA golemita = col.GetComponentInParent<GolemitaIA>();
            if (golemita != null && golemitasEmpujadas.Add(golemita))
            {
                golemita.Empujar(direccion * fuerzaEmpuje, duracionEmpuje);
                continue;
            }

            ChispaIA chispa = col.GetComponentInParent<ChispaIA>();
            if (chispa != null && chispasEmpujadas.Add(chispa))
                chispa.Empujar(direccion * fuerzaEmpuje, duracionEmpuje);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.55f, 0.1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
