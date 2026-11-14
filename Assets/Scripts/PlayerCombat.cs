using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float radioAtaque = 0.8f;
    public int daño = 1;
    public float cooldown = 0.4f;
    public float cooldownMinimo = 0.15f;
    public float retrasoGolpe = 0.15f; // momento del golpe dentro de la animación

    [Header("Audio (opcional)")]
    public AudioClip sonidoAtaque;
    public AudioClip sonidoImpacto;

    private Animator animator;
    private float timer = 0f;
    private bool siguienteEsPatada = false;
    public bool Atacando { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();

        // RF-37: las mejoras recogidas en la partida reducen el cooldown
        cooldown = Mathf.Max(cooldownMinimo, cooldown - DatosJuego.Mejoras * 0.5f);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J) && timer <= 0f)
        {
            timer = cooldown;

            if (animator != null)
                animator.SetTrigger(siguienteEsPatada ? "Attack2" : "Attack1");

            siguienteEsPatada = !siguienteEsPatada;

            AudioManager.SFX(sonidoAtaque, 0.6f, transform.position);
            StartCoroutine(Golpear());
        }
    }

    /// <summary>RF-37: Cristal de Mejora.</summary>
    public void ReducirCooldown(float cantidad)
    {
        cooldown = Mathf.Max(cooldownMinimo, cooldown - cantidad);
    }

    private IEnumerator Golpear()
    {
        Atacando = true;
        yield return new WaitForSeconds(retrasoGolpe);

        if (attackPoint != null)
        {
            Collider2D[] golpeados = Physics2D.OverlapCircleAll(attackPoint.position, radioAtaque);

            foreach (Collider2D col in golpeados)
            {
                if (col == null) continue;

                if (col.TryGetComponent(out GolemitaHealth golemita))
                {
                    golemita.RecibirDaño(daño);
                    AudioManager.SFX(sonidoImpacto, 0.7f, col.transform.position);
                }
                else if (col.TryGetComponent(out ChispaHealth chispa))
                {
                    chispa.RecibirDaño(daño);
                    AudioManager.SFX(sonidoImpacto, 0.7f, col.transform.position);
                }
                else if (col.TryGetComponent(out MuroRompible muro))
                {
                    muro.RecibirGolpe();   // normales: se rompen a golpes; reforzados: solo tiemblan
                    AudioManager.SFX(sonidoImpacto, 0.6f, col.transform.position);
                }
                else if (col.TryGetComponent(out MolrakHealth molrak))
                {
                    molrak.RecibirDaño(daño, attackPoint.position);
                    AudioManager.SFX(sonidoImpacto, 0.8f, col.transform.position);
                }
            }
        }

        yield return new WaitForSeconds(Mathf.Max(0f, cooldown - retrasoGolpe));
        Atacando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, radioAtaque);
    }
}
