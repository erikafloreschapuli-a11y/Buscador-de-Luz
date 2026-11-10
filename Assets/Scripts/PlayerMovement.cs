 using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public float velocidad;
   public float FuerzaSalto;
   public LayerMask CapaSuelo; 
   public float saltosMaximos;

   private float SaltosRestantes;
   private new Rigidbody2D rigidbody;
   private BoxCollider2D boxCollider;
   private bool mirandoDerecha = true;
   private Animator animator;

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
         RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, CapaSuelo);
         return raycastHit.collider != null;
    }

    void Procesarsalto()
    {
        if (EstaEnSuelo())
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
        }
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

       rigidbody.linearVelocity = new Vector2(inputMovimientoo * velocidad, rigidbody.linearVelocity.y);
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