using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// RF-46: pantalla de "Fragmento obtenido" al derrotar a Molrak.
/// Si no se le asignan textos, construye su propia interfaz por código.
/// Deja este objeto ACTIVO en la escena: el script se oculta solo en Awake.
/// </summary>
public class PanelFragmento : MonoBehaviour
{
    public static PanelFragmento Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private CanvasGroup grupo;
    [SerializeField] private TMP_Text textoPrincipal;
    [SerializeField] private TMP_Text textoSecundario;

    [Header("Textos")]
    [SerializeField] private string mensaje = "FRAGMENTO DE AETHON OBTENIDO";
    [SerializeField] private string submensaje = "Ignar ha despertado. El Pulso de Calor es tuyo.";

    [Header("Tiempos")]
    [SerializeField] private float duracionFade = 0.8f;
    [SerializeField] private float tiempoVisible = 4f;

    [Header("Opciones")]
    [Tooltip("Si está activo, el juego se detiene mientras se muestra el mensaje")]
    [SerializeField] private bool pausarJuego = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoFragmento;

    private bool mostrando;

    private void Awake()
    {
        Instance = this;

        if (grupo == null)
            grupo = GetComponent<CanvasGroup>();

        if (grupo == null && (textoPrincipal == null || textoSecundario == null))
            ConstruirInterfaz();

        if (grupo == null)
            grupo = GetComponent<CanvasGroup>();

        if (grupo == null)
            grupo = gameObject.AddComponent<CanvasGroup>();

        if (textoPrincipal != null) textoPrincipal.text = mensaje;
        if (textoSecundario != null) textoSecundario.text = submensaje;

        grupo.alpha = 0f;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Mostrar()
    {
        if (mostrando) return;
        mostrando = true;

        // El objeto está desactivado tras Awake: hay que activarlo ANTES de la corrutina
        gameObject.SetActive(true);
        StartCoroutine(RutinaMostrar());
    }

    private IEnumerator RutinaMostrar()
    {
        if (audioSource != null && sonidoFragmento != null)
            audioSource.PlayOneShot(sonidoFragmento);

        if (pausarJuego)
            Time.timeScale = 0f;

        grupo.alpha = 0f;

        float t = 0f;
        while (t < duracionFade)
        {
            t += Time.unscaledDeltaTime;
            grupo.alpha = Mathf.Clamp01(t / duracionFade);
            yield return null;
        }
        grupo.alpha = 1f;

        yield return new WaitForSecondsRealtime(tiempoVisible);

        t = 0f;
        while (t < duracionFade)
        {
            t += Time.unscaledDeltaTime;
            grupo.alpha = 1f - Mathf.Clamp01(t / duracionFade);
            yield return null;
        }
        grupo.alpha = 0f;
        gameObject.SetActive(false);

        if (pausarJuego)
            Time.timeScale = 1f;

        mostrando = false;
    }

    /// <summary>Construye la pantalla completa por código (sin tocar el editor).</summary>
    private void ConstruirInterfaz()
    {
        Canvas canvas = ConstructorUI.CrearCanvas("Canvas_Fragmento", 60);
        canvas.transform.SetParent(transform, false);

        grupo = canvas.gameObject.AddComponent<CanvasGroup>();

        RectTransform fondo = ConstructorUI.CrearPanelCompleto(
            canvas.transform, "Fondo", new Color(0.03f, 0.01f, 0.0f, 0.82f));

        ConstructorUI.CrearTexto(fondo, "Titulo", mensaje, 54f,
            new Vector2(1400f, 90f), new Vector2(0f, 120f),
            TextAlignmentOptions.Center, new Color(1f, 0.72f, 0.25f, 1f));

        ConstructorUI.CrearTexto(fondo, "Subtitulo", submensaje, 30f,
            new Vector2(1200f, 70f), new Vector2(0f, 30f),
            TextAlignmentOptions.Center, Color.white);

        ConstructorUI.CrearTexto(fondo, "Pista", "Pulsa Q para usar el Pulso de Calor", 24f,
            new Vector2(1200f, 50f), new Vector2(0f, -70f),
            TextAlignmentOptions.Center, new Color(1f, 1f, 1f, 0.6f));

        textoPrincipal = fondo.Find("Titulo")?.GetComponent<TMP_Text>();
        textoSecundario = fondo.Find("Subtitulo")?.GetComponent<TMP_Text>();
    }
}
