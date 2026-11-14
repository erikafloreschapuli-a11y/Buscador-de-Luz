using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelPausa;

    [Header("Opciones (RF-44)")]
    [SerializeField] private GameObject panelOpciones;

    [Header("Si alguno de estos está activo, no se puede pausar")]
    [SerializeField] private GameObject[] panelesQueBloquean;

    [Header("Escenas")]
    [SerializeField] private string escenaJuego = "Ignar";
    [SerializeField] private string escenaMenu = "MainMenu";

    private bool pausado;
    private Button botonOpciones;

    private void Awake()
    {
        pausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    private void Start()
    {
        PrepararOpciones();
    }

    private void Update()
    {
        // Si las opciones están abiertas, Escape las cierra en vez de reanudar
        if (panelOpciones != null && panelOpciones.activeInHierarchy)
        {
            if (PausaPresionada()) CerrarOpciones();
            return;
        }

        if (HayPanelBloqueante())
        {
            // Si Game Over o Victoria aparecen con la pausa abierta, se cierra la pausa
            if (pausado && panelPausa != null)
            {
                pausado = false;
                panelPausa.SetActive(false);
                MostrarBotonOpciones(false);

                // Si no es Game Over el que bloquea, el juego vuelve a correr
                if (GameOverManager.Instance == null || !GameOverManager.Instance.EnCurso)
                    Time.timeScale = 1f;
            }
            return;
        }

        if (!PausaPresionada()) return;

        if (pausado) Reanudar();
        else Pausar();
    }

    private bool PausaPresionada()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    private bool HayPanelBloqueante()
    {
        // Si Game Over ya está en curso no se puede pausar: si no, Time.timeScale = 0
        // congelaría la espera del panel y nunca aparecería (RF-45).
        if (GameOverManager.Instance != null && GameOverManager.Instance.EnCurso)
            return true;

        if (panelesQueBloquean == null) return false;
        foreach (GameObject p in panelesQueBloquean)
        {
            if (p != null && p.activeInHierarchy) return true;
        }
        return false;
    }

    public void Pausar()
    {
        pausado = true;
        if (panelPausa != null) panelPausa.SetActive(true);
        MostrarBotonOpciones(true);
        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        pausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
        MostrarBotonOpciones(false);
        Time.timeScale = 1f;
    }

    // RF-44: botón Opciones dentro de la pausa
    public void AbrirOpciones()
    {
        if (panelOpciones == null) PrepararOpciones();
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        string escena = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(string.IsNullOrEmpty(escena) ? escenaJuego : escena);
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        DatosJuego.Guardar();
        SceneManager.LoadScene(escenaMenu);
    }

    /// <summary>
    /// Crea el panel de Opciones y el botón que lo abre si no existen en la escena.
    /// Así las opciones funcionan sin cablear nada a mano.
    /// </summary>
    private void PrepararOpciones()
    {
        if (panelOpciones == null)
        {
            Canvas canvas = ConstructorUI.CrearCanvas("Canvas_OpcionesJuego", 40);
            OpcionesUI ui = ConstructorUI.CrearPanelOpciones(canvas.transform, "PanelOpcionesJuego", CerrarOpciones);

            panelOpciones = ui.gameObject;
            panelOpciones.SetActive(false);
        }

        if (botonOpciones != null || panelPausa == null) return;

        Transform padre = panelPausa.transform.parent;
        if (padre == null) padre = panelPausa.transform;

        Transform existente = padre.Find("BotonOpcionesPausa");
        if (existente != null)
        {
            botonOpciones = existente.GetComponent<Button>();
        }
        else
        {
            botonOpciones = ConstructorUI.CrearBotonAnclado(
                padre,
                "BotonOpcionesPausa",
                "OPCIONES",
                new Vector2(230f, 58f),
                new Vector2(-150f, -60f),
                AbrirOpciones,
                new Vector2(1f, 1f));
        }

        MostrarBotonOpciones(false);
    }

    private void MostrarBotonOpciones(bool visible)
    {
        if (botonOpciones != null)
            botonOpciones.gameObject.SetActive(visible);
    }
}
