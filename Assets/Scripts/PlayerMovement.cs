 using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public float velocidad;
   public float FuerzaSalto;
   public LayerMask CapaSuelo; 
   public float saltosMaximos;

   [Header("Sprint (RF-10)")]
   public float multiplicadorSprint = 1.6f;
   public KeyCode teclaSprint = KeyCode.LeftShift;

   private float SaltosRestantes;
   private new Rigidbody2D rigidbody;
   private BoxCollider2D boxCollider;
   private bool mirandoDerecha = true;
   private Animator animator;
   private float ultimoSalto = -1f;

    private void Start()
    {
         rigidbody = GetComponent<Rigidbody2D>();
         boxCollider = GetComponent<BoxCollider2D>();
         SaltosRestantes = saltosMaximos;
         animator = GetComponent<Animator>();
    }
   
    void Update()
    {
        ProcesarMovimiento();
        Procesarsalto();
    }

    bool EstaEnSuelo()
    {
         // Solo una franja delgada bajo los pies (90 % del ancho) para no detectar
         // paredes a los lados ni el suelo justo después de despegar.
         Bounds b = boxCollider.bounds;
         Vector2 origen = new Vector2(b.center.x, b.min.y + 0.05f);
         Vector2 tamaño = new Vector2(b.size.x * 0.9f, 0.1f);
         RaycastHit2D raycastHit = Physics2D.BoxCast(origen, tamaño, 0f, Vector2.down, 0.1f, CapaSuelo);
         return raycastHit.collider != null;
    }

    void Procesarsalto()
    {
        // Recargar saltos solo al estar apoyado (no mientras sube tras saltar)
        if (EstaEnSuelo() && rigidbody.linearVelocity.y <= 0.01f)
        {
            SaltosRestantes = saltosMaximos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && SaltosRestantes > 0)
        {
            SaltosRestantes--;
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0f);
            rigidbody.AddForce(Vector2.up * FuerzaSalto, ForceMode2D.Impulse);
            
            // NUEVO: Activa la animación de salto
            animator.SetTrigger("Jump");
            ultimoSalto = Time.time;
        }

        // Animator: Kael está en el suelo (para salir de la animación de salto al aterrizar)
        animator.SetBool("Grounded", EstaEnSuelo() && rigidbody.linearVelocity.y <= 0.01f && Time.time - ultimoSalto > 0.15f);
    }

    void ProcesarMovimiento()
    {
       // Logica de movimiento
       float inputMovimientoo = Input.GetAxis("Horizontal");
       
       if (inputMovimientoo != 0f)
       {
           animator.SetBool("Running", true);
       }
       else
       {
           animator.SetBool("Running", false);
       }

       float multiplicador = Input.GetKey(teclaSprint) ? multiplicadorSprint : 1f;
       // Animación de correr (estado Run del Animator) al mantener Shift y moverse
       animator.SetBool("Sprinting", Input.GetKey(teclaSprint) && inputMovimientoo != 0f);
       rigidbody.linearVelocity = new Vector2(inputMovimientoo * velocidad * multiplicador, rigidbody.linearVelocity.y);
       GestionarOrientacion(inputMovimientoo);
    }

    void GestionarOrientacion(float inputMovimientoo)
    {
          if ((mirandoDerecha == true && inputMovimientoo < 0) || (mirandoDerecha == false && inputMovimientoo > 0))
          {
             mirandoDerecha = !mirandoDerecha; 
             transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
          }
    }
}