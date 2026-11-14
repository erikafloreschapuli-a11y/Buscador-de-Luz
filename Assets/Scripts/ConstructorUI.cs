using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Constructor de interfaz en tiempo de ejecución.
/// Permite que el HUD, las opciones y el panel del Fragmento existan aunque
/// no se hayan creado a mano en el editor. Si el usuario sí los crea y los
/// asigna en el Inspector, estos métodos no se usan.
/// </summary>
public static class ConstructorUI
{
    private static Sprite spriteBase;

    /// <summary>
    /// Sprite redondeado generado por código (blanco, 16×16, con esquinas suaves).
    /// No usa Resources.GetBuiltinResource: esa ruta no existe en todas las versiones
    /// de Unity y lanzaba "Failed to find UI/Skin/UISprite.psd".
    /// </summary>
    public static Sprite SpriteRedondeado
    {
        get
        {
            if (spriteBase == null)
                spriteBase = CrearSpriteRedondeado();
            return spriteBase;
        }
    }

    private static Sprite CrearSpriteRedondeado()
    {
        const int lado = 16;
        const int radio = 5;

        Texture2D textura = new Texture2D(lado, lado, TextureFormat.RGBA32, false);
        textura.hideFlags = HideFlags.HideAndDontSave;
        textura.wrapMode = TextureWrapMode.Clamp;
        textura.filterMode = FilterMode.Bilinear;

        Color32[] pixeles = new Color32[lado * lado];

        for (int y = 0; y < lado; y++)
        {
            for (int x = 0; x < lado; x++)
            {
                float dx = 0f;
                float dy = 0f;

                if (x < radio) dx = radio - x - 0.5f;
                else if (x >= lado - radio) dx = x - (lado - radio) + 0.5f;

                if (y < radio) dy = radio - y - 0.5f;
                else if (y >= lado - radio) dy = y - (lado - radio) + 0.5f;

                float distancia = Mathf.Sqrt(dx * dx + dy * dy);
                float alfa = Mathf.Clamp01(radio + 0.5f - distancia);

                pixeles[y * lado + x] = new Color32(255, 255, 255, (byte)(alfa * 255f));
            }
        }

        textura.SetPixels32(pixeles);
        textura.Apply();

        return Sprite.Create(
            textura,
            new Rect(0f, 0f, lado, lado),
            new Vector2(0.5f, 0.5f),
            lado,
            0,
            SpriteMeshType.FullRect,
            new Vector4(radio, radio, radio, radio));
    }

    public static readonly Color ColorPanel = new Color(0.09f, 0.05f, 0.03f, 0.97f);
    public static readonly Color ColorBoton = new Color(0.95f, 0.45f, 0.10f, 1f);
    public static readonly Color ColorBotonTexto = new Color(0.10f, 0.04f, 0.01f, 1f);
    public static readonly Color ColorNaranja = new Color(1f, 0.55f, 0.12f, 1f);

    /// <summary>Crea (o reutiliza) un Canvas de pantalla completa.</summary>
    public static Canvas CrearCanvas(string nombre, int orden)
    {
        GameObject existente = GameObject.Find(nombre);
        if (existente != null)
        {
            Canvas c = existente.GetComponent<Canvas>();
            if (c != null) return c;
        }

        GameObject go = new GameObject(nombre, typeof(RectTransform));

        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = orden;

        CanvasScaler escalador = go.AddComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.matchWidthOrHeight = 0.5f;

        go.AddComponent<GraphicRaycaster>();

        AsegurarEventSystem();
        return canvas;
    }

    /// <summary>Sin EventSystem los botones no reciben clics.</summary>
    public static void AsegurarEventSystem()
    {
        if (EventSystem.current != null) return;
        if (Object.FindAnyObjectByType<EventSystem>() != null) return;

        GameObject go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();

#if ENABLE_INPUT_SYSTEM
        go.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        go.AddComponent<StandaloneInputModule>();
#endif
    }

    public static RectTransform CrearRect(Transform padre, string nombre)
    {
        GameObject go = new GameObject(nombre, typeof(RectTransform));
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(padre, false);
        return rt;
    }

    /// <summary>Panel que ocupa toda la pantalla.</summary>
    public static RectTransform CrearPanelCompleto(Transform padre, string nombre, Color color, float margen = 0f)
    {
        RectTransform rt = CrearRect(padre, nombre);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(margen, margen);
        rt.offsetMax = new Vector2(-margen, -margen);

        Image imagen = rt.gameObject.AddComponent<Image>();
        imagen.sprite = SpriteRedondeado;
        imagen.type = Image.Type.Sliced;
        imagen.color = color;
        return rt;
    }

    /// <summary>Panel centrado de tamaño fijo.</summary>
    public static RectTransform CrearPanelCentrado(Transform padre, string nombre, Vector2 tamano, Color color)
    {
        RectTransform rt = CrearRect(padre, nombre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = tamano;
        rt.anchoredPosition = Vector2.zero;

        Image imagen = rt.gameObject.AddComponent<Image>();
        imagen.sprite = SpriteRedondeado;
        imagen.type = Image.Type.Sliced;
        imagen.color = color;
        return rt;
    }

    public static TMP_Text CrearTexto(
        Transform padre,
        string nombre,
        string contenido,
        float tamanoFuente,
        Vector2 tamano,
        Vector2 posicion,
        TextAlignmentOptions alineacion = TextAlignmentOptions.Center,
        Color? color = null)
    {
        RectTransform rt = CrearRect(padre, nombre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = tamano;
        rt.anchoredPosition = posicion;

        TextMeshProUGUI texto = rt.gameObject.AddComponent<TextMeshProUGUI>();
        texto.text = contenido;
        texto.fontSize = tamanoFuente;
        texto.alignment = alineacion;
        texto.color = color ?? Color.white;
        texto.raycastTarget = false;
        return texto;
    }

    public static Button CrearBoton(
        Transform padre,
        string nombre,
        string etiqueta,
        Vector2 tamano,
        Vector2 posicion,
        UnityAction alPulsar)
    {
        return CrearBotonAnclado(padre, nombre, etiqueta, tamano, posicion, alPulsar, new Vector2(0.5f, 0.5f));
    }

    public static Button CrearBotonAnclado(
        Transform padre,
        string nombre,
        string etiqueta,
        Vector2 tamano,
        Vector2 posicion,
        UnityAction alPulsar,
        Vector2 anclaje)
    {
        RectTransform rt = CrearRect(padre, nombre);
        rt.anchorMin = anclaje;
        rt.anchorMax = anclaje;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = tamano;
        rt.anchoredPosition = posicion;

        Image fondo = rt.gameObject.AddComponent<Image>();
        fondo.sprite = SpriteRedondeado;
        fondo.type = Image.Type.Sliced;
        fondo.color = ColorBoton;

        Button boton = rt.gameObject.AddComponent<Button>();
        boton.targetGraphic = fondo;

        ColorBlock colores = boton.colors;
        colores.normalColor = Color.white;
        colores.highlightedColor = new Color(1f, 0.85f, 0.6f, 1f);
        colores.pressedColor = new Color(0.8f, 0.7f, 0.5f, 1f);
        boton.colors = colores;

        if (alPulsar != null)
            boton.onClick.AddListener(alPulsar);

        TMP_Text texto = CrearTexto(
            rt,
            "Texto",
            etiqueta,
            Mathf.Max(18f, tamano.y * 0.45f),
            new Vector2(tamano.x - 16f, tamano.y - 6f),
            Vector2.zero);
        texto.color = ColorBotonTexto;
        texto.fontStyle = FontStyles.Bold;

        return boton;
    }

    public static Slider CrearSlider(Transform padre, string nombre, Vector2 tamano, Vector2 posicion)
    {
        RectTransform rt = CrearRect(padre, nombre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = tamano;
        rt.anchoredPosition = posicion;

        Slider slider = rt.gameObject.AddComponent<Slider>();

        // Fondo de la barra
        RectTransform fondoRT = CrearRect(rt, "Fondo");
        fondoRT.anchorMin = new Vector2(0f, 0.30f);
        fondoRT.anchorMax = new Vector2(1f, 0.70f);
        fondoRT.offsetMin = Vector2.zero;
        fondoRT.offsetMax = Vector2.zero;

        Image fondo = fondoRT.gameObject.AddComponent<Image>();
        fondo.sprite = SpriteRedondeado;
        fondo.type = Image.Type.Sliced;
        fondo.color = new Color(0.16f, 0.10f, 0.07f, 1f);

        // Zona de relleno
        RectTransform areaRT = CrearRect(rt, "AreaRelleno");
        areaRT.anchorMin = new Vector2(0f, 0.30f);
        areaRT.anchorMax = new Vector2(1f, 0.70f);
        areaRT.offsetMin = Vector2.zero;
        areaRT.offsetMax = Vector2.zero;

        RectTransform rellenoRT = CrearRect(areaRT, "Relleno");
        rellenoRT.anchorMin = Vector2.zero;
        rellenoRT.anchorMax = Vector2.one;
        rellenoRT.offsetMin = Vector2.zero;
        rellenoRT.offsetMax = Vector2.zero;

        Image relleno = rellenoRT.gameObject.AddComponent<Image>();
        relleno.sprite = SpriteRedondeado;
        relleno.type = Image.Type.Sliced;
        relleno.color = ColorNaranja;

        // Zona del tirador
        RectTransform areaTiradorRT = CrearRect(rt, "AreaTirador");
        areaTiradorRT.anchorMin = Vector2.zero;
        areaTiradorRT.anchorMax = Vector2.one;
        areaTiradorRT.offsetMin = Vector2.zero;
        areaTiradorRT.offsetMax = Vector2.zero;

        RectTransform tiradorRT = CrearRect(areaTiradorRT, "Tirador");
        tiradorRT.sizeDelta = new Vector2(26f, 38f);

        Image tirador = tiradorRT.gameObject.AddComponent<Image>();
        tirador.sprite = SpriteRedondeado;
        tirador.type = Image.Type.Sliced;
        tirador.color = Color.white;

        slider.fillRect = rellenoRT;
        slider.handleRect = tiradorRT;
        slider.targetGraphic = tirador;
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;

        return slider;
    }

    public static Toggle CrearToggle(Transform padre, string nombre, Vector2 posicion)
    {
        RectTransform rt = CrearRect(padre, nombre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(36f, 36f);
        rt.anchoredPosition = posicion;

        Image fondo = rt.gameObject.AddComponent<Image>();
        fondo.sprite = SpriteRedondeado;
        fondo.type = Image.Type.Sliced;
        fondo.color = new Color(0.16f, 0.10f, 0.07f, 1f);

        RectTransform marcaRT = CrearRect(rt, "Marca");
        marcaRT.anchorMin = new Vector2(0.18f, 0.18f);
        marcaRT.anchorMax = new Vector2(0.82f, 0.82f);
        marcaRT.offsetMin = Vector2.zero;
        marcaRT.offsetMax = Vector2.zero;

        Image marca = marcaRT.gameObject.AddComponent<Image>();
        marca.sprite = SpriteRedondeado;
        marca.type = Image.Type.Sliced;
        marca.color = ColorNaranja;

        Toggle toggle = rt.gameObject.AddComponent<Toggle>();
        toggle.targetGraphic = fondo;
        toggle.graphic = marca;
        toggle.isOn = true;

        return toggle;
    }

    /// <summary>
    /// Crea el panel de Opciones completo (volumen, pantalla completa y volver).
    /// Devuelve el componente OpcionesUI; su gameObject es la ventana.
    /// </summary>
    public static OpcionesUI CrearPanelOpciones(Transform padre, string nombre, UnityAction alVolver)
    {
        RectTransform fondo = CrearPanelCompleto(padre, nombre, new Color(0f, 0f, 0f, 0.65f));
        RectTransform ventana = CrearPanelCentrado(fondo, "Ventana", new Vector2(720f, 580f), ColorPanel);

        CrearTexto(ventana, "Titulo", "OPCIONES", 46f, new Vector2(640f, 64f), new Vector2(0f, 230f));

        CrearTexto(ventana, "EtiquetaVolumen", "Volumen", 28f,
            new Vector2(280f, 40f), new Vector2(-170f, 120f), TextAlignmentOptions.Left);

        Slider slider = CrearSlider(ventana, "Volumen", new Vector2(320f, 34f), new Vector2(120f, 120f));

        CrearTexto(ventana, "EtiquetaPantalla", "Pantalla completa", 28f,
            new Vector2(360f, 40f), new Vector2(-130f, 40f), TextAlignmentOptions.Left);

        Toggle toggle = CrearToggle(ventana, "PantallaCompleta", new Vector2(220f, 40f));

        CrearTexto(ventana, "Nota", "Los cambios se guardan automáticamente.", 20f,
            new Vector2(620f, 30f), new Vector2(0f, -50f), TextAlignmentOptions.Center,
            new Color(1f, 1f, 1f, 0.6f));

        CrearBoton(ventana, "BotonVolver", "VOLVER", new Vector2(280f, 66f), new Vector2(0f, -190f), alVolver);

        OpcionesUI ui = fondo.gameObject.AddComponent<OpcionesUI>();
        ui.Configurar(slider, toggle, null, fondo.gameObject);

        slider.onValueChanged.AddListener(ui.CambiarVolumen);
        toggle.onValueChanged.AddListener(ui.CambiarPantallaCompleta);

        return ui;
    }

    /// <summary>Panel informativo con un título, un cuerpo de texto y un botón Volver.</summary>
    public static GameObject CrearPanelTexto(string nombre, string titulo, string cuerpo, UnityAction alVolver)
    {
        Canvas canvas = CrearCanvas("Canvas_" + nombre, 30);
        RectTransform fondo = CrearPanelCompleto(canvas.transform, nombre, new Color(0f, 0f, 0f, 0.75f));
        RectTransform ventana = CrearPanelCentrado(fondo, "Ventana", new Vector2(1000f, 640f), ColorPanel);

        CrearTexto(ventana, "Titulo", titulo, 46f, new Vector2(900f, 64f), new Vector2(0f, 250f));
        CrearTexto(ventana, "Cuerpo", cuerpo, 26f, new Vector2(880f, 400f), new Vector2(0f, 10f));
        CrearBoton(ventana, "BotonVolver", "VOLVER", new Vector2(280f, 62f), new Vector2(0f, -245f), alVolver);

        return fondo.gameObject;
    }
}
