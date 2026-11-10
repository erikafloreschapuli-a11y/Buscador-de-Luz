using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private CanvasGroup panelGameOver;

    [Header("Escenas")]
    [SerializeField] private string escenaJuego = "Ignar";
    [SerializeField] private string escenaMenu = "MainMenu";

    [Header("Tiempos")]
    [Tooltip("Espera antes de mostrar el panel, para que se vea la animación de muerte")]
    [SerializeField] private float retraso = 1f;
    [SerializeField] private float duracionFade = 0.8f;

    [Header("Sonido (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGameOver;

    private bool activo;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // por si se llegó desde una pausa

        if (panelGameOver != null)
        {
            panelGameOver.alpha = 0f;
            panelGameOver.interactable = false;
            panelGameOver.blocksRaycasts = false;
            panelGameOver.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // Llámalo cuando Kael muera
    public void MostrarGameOver()
    {
        if (activo) return;
        activo = true;
        StartCoroutine(RutinaGameOver());
    }

    private IEnumerator RutinaGameOver()
    {
        // Mientras corre el tiempo normal, se ve la animación de muerte
        yield return new WaitForSeconds(retraso);

        panelGameOver.gameObject.SetActive(true);
        panelGameOver.alpha = 0f;
        panelGameOver.blocksRaycasts = true;

        if (audioSource != null && sonidoGameOver != null)
            audioSource.PlayOneShot(sonidoGameOver);

        // A partir de aquí el juego se pausa; el fade usa tiempo real
        Time.timeScale = 0f;

        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.unscaledDeltaTime;
            panelGameOver.alpha = Mathf.Clamp01(t / duracionFade);
            yield return null;
        }

        panelGameOver.alpha = 1f;
        panelGameOver.interactable = true;
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaJuego);
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}