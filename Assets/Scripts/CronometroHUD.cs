using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// HUD superior derecho: cronómetro de la partida (mm:ss) y botón de pausa (II).
/// Mismo estilo que el ícono del Pulso de Calor: panel oscuro con borde naranja.
/// El tiempo viene de DatosJuego.Tiempo (EstadisticasJuego), así que sigue entre
/// Ignar, Zona2 y Zona3, y se detiene con la pausa y el Game Over.
/// Se crea solo en cada escena de juego; no hay que colocarlo a mano.
/// </summary>
public class CronometroHUD : MonoBehaviour
{
    public TMP_Text texto;
    private float tiempo;

    private static readonly Color Dorado = new Color(1f, 0.88f, 0.65f, 1f);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Iniciar()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
        SceneManager.sceneLoaded += AlCargarEscena;
        Crear();
    }

    private static void AlCargarEscena(Scene escena, LoadSceneMode modo) => Crear();

    private static void Crear()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        if (Object.FindAnyObjectByType<CronometroHUD>() != null) return;
        if (GameObject.FindWithTag("Player") == null) return;

        // Debajo del Canvas principal (orden -1) para que el panel de pausa lo cubra
        Canvas canvas = ConstructorUI.CrearCanvas("Canvas_HUDSuperior", -1);
        Transform raiz = canvas.transform;

        // ---------- Botón de pausa ----------
        RectTransform boton = Marco(raiz, "BotonPausa", new Vector2(64f, 64f), new Vector2(-24f, -24f), out Image fondoBoton);
        foreach (float x in new[] { -8f, 8f })
        {
            RectTransform barra = ConstructorUI.CrearRect(boton, "Barra");
            barra.anchorMin = barra.anchorMax = barra.pivot = new Vector2(0.5f, 0.5f);
            barra.sizeDelta = new Vector2(9f, 28f);
            barra.anchoredPosition = new Vector2(x, 0f);
            Image img = barra.gameObject.AddComponent<Image>();
            img.color = Dorado;
            img.raycastTarget = false;
        }
        Button b = boton.gameObject.AddComponent<Button>();
        b.targetGraphic = fondoBoton;
        ColorBlock cb = b.colors;
        cb.highlightedColor = new Color(1f, 0.75f, 0.5f, 1f);
        cb.pressedColor = new Color(0.8f, 0.5f, 0.3f, 1f);
        b.colors = cb;
        b.onClick.AddListener(PausarDesdeBoton);

        // ---------- Cronómetro ----------
        RectTransform panel = Marco(raiz, "Cronometro", new Vector2(176f, 64f), new Vector2(-100f, -24f), out _);
        TMP_Text etiqueta = ConstructorUI.CrearTexto(panel, "Etiqueta", "TIEMPO", 15f, new Vector2(150f, 20f), new Vector2(0f, 16f),
                                                    TextAlignmentOptions.Center, ConstructorUI.ColorNaranja);
        etiqueta.fontStyle = FontStyles.Bold;
        etiqueta.characterSpacing = 6f;
        TMP_Text t = ConstructorUI.CrearTexto(panel, "Valor", "00:00", 34f, new Vector2(150f, 40f), new Vector2(0f, -8f),
                                             TextAlignmentOptions.Center, Dorado);
        t.fontStyle = FontStyles.Bold;

        CronometroHUD c = panel.gameObject.AddComponent<CronometroHUD>();
        c.texto = t;
    }

    /// <summary>Marco con borde naranja y fondo oscuro, anclado arriba a la derecha.</summary>
    private static RectTransform Marco(Transform padre, string nombre, Vector2 tamano, Vector2 posicion, out Image fondo)
    {
        RectTransform rt = ConstructorUI.CrearRect(padre, nombre);
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1f, 1f);
        rt.sizeDelta = tamano;
        rt.anchoredPosition = posicion;
        Image borde = rt.gameObject.AddComponent<Image>();
        borde.sprite = ConstructorUI.SpriteRedondeado;
        borde.type = Image.Type.Sliced;
        borde.color = ConstructorUI.ColorNaranja;

        RectTransform interior = ConstructorUI.CrearRect(rt, "Fondo");
        interior.anchorMin = Vector2.zero;
        interior.anchorMax = Vector2.one;
        interior.offsetMin = new Vector2(3f, 3f);
        interior.offsetMax = new Vector2(-3f, -3f);
        fondo = interior.gameObject.AddComponent<Image>();
        fondo.sprite = ConstructorUI.SpriteRedondeado;
        fondo.type = Image.Type.Sliced;
        fondo.color = ConstructorUI.ColorPanel;
        return rt;
    }

    private static void PausarDesdeBoton()
    {
        if (Time.timeScale == 0f) return;                                            // ya está en pausa
        if (GameOverManager.Instance != null && GameOverManager.Instance.EnCurso) return;
        PauseManager pausa = Object.FindAnyObjectByType<PauseManager>();
        if (pausa != null) pausa.Pausar();
    }

    private void Start()
    {
        tiempo = DatosJuego.Tiempo;
    }

    private void Update()
    {
        tiempo += Time.deltaTime;
        int s = Mathf.FloorToInt(tiempo);
        if (texto != null) texto.text = (s / 60).ToString("00") + ":" + (s % 60).ToString("00");
    }
}
