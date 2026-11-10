using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float radioAtaque = 0.8f;
    public int daño = 1;
    public float cooldown = 0.4f;
    public float retrasoGolpe = 0.15f; // momento del golpe dentro de la animación

    private Animator animator;
    private float timer = 0f;
    private bool siguienteEsPatada = false;
    public bool Atacando { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J) && timer <= 0f)
        {
            timer = cooldown;
            animator.SetTrigger(siguienteEsPatada ? "Attack2" : "Attack1");
            siguienteEsPatada = !siguienteEsPatada;
            StartCoroutine(Golpear());
        }
    }

    IEnumerator Golpear()
    {
        Atacando = true;
        yield return new WaitForSeconds(retrasoGolpe);

        Collider2D[] golpeados = Physics2D.OverlapCircleAll(attackPoint.position, radioAtaque);
        foreach (Collider2D col in golpeados)
        {
            if (col.TryGetComponent(out GolemitaHealth golemita))
                golemita.RecibirDaño(daño);
            else if (col.TryGetComponent(out ChispaHealth chispa))
                chispa.RecibirDaño(daño);
        }

        yield return new WaitForSeconds(cooldown - retrasoGolpe);
        Atacando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, radioAtaque);
    }
}