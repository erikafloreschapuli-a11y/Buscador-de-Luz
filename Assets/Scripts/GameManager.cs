using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Tooltip("Referencia al jugador; si está vacía se busca por tag Player")]
    public GameObject player;

    private Vector3 checkpointPos;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (player == null)
            player = GameObject.FindWithTag("Player");

        // Posición por defecto: el spawn de la escena
        if (player != null)
            checkpointPos = player.transform.position;

        // RF-19: si hay checkpoint guardado, Kael aparece ahí (Continuar / Reintentar)
        if (DatosJuego.IntentarCargarCheckpoint(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, out Vector3 guardado))
        {
            checkpointPos = guardado;
            if (player != null)
                player.transform.position = guardado;
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    // RF-17 / RF-19: activar un checkpoint guarda la posición en disco
    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
        DatosJuego.GuardarCheckpoint(pos);
    }

    // RF-08 / RF-45: reposicionar a Kael en el último checkpoint con vida completa
    public void RespawnPlayer()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (player == null) return;

        player.transform.position = checkpointPos;
        player.SetActive(true);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        PlayerHealth vida = player.GetComponent<PlayerHealth>();
        if (vida != null) vida.Revivir();
    }

    public Vector3 GetCheckpoint() => checkpointPos;
}
