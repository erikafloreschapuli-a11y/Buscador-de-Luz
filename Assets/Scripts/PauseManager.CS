using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panelPausa;

    [Header("Si alguno de estos está activo, no se puede pausar")]
    [SerializeField] private GameObject[] panelesQueBloquean;

    [Header("Escenas")]
    [SerializeField] private string escenaJuego = "Ignar";
    [SerializeField] private string escenaMenu = "MainMenu";

    private bool pausado;

    private void Awake()
    {
        pausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    private void Update()
    {
        if (HayPanelBloqueante())
        {
            // Si Game Over o Victoria aparecen con la pausa abierta, se cierra la pausa
            if (pausado && panelPausa != null)
            {
                pausado = false;
                panelPausa.SetActive(false);
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
        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        pausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaJuego);
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        DatosJuego.Guardar();
        SceneManager.LoadScene(escenaMenu);
    }
}