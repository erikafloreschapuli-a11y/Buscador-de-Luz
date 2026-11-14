using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [Header("Vida")]
    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;
    [Tooltip("Opcional: si está vacío, el medio corazón se dibuja lleno")]
    public Sprite corazonMedio;

    [Header("Fragmento de Aethon (RF-41)")]
    public Image slotFragmento;
    public Sprite fragmentoApagado;
    public Sprite fragmentoEncendido;

    [Header("Pulso de Calor (RF-42)")]
    [Tooltip("Se muestra solo tras desbloquear la habilidad")]
    public GameObject iconoHabilidad;
    [Tooltip("Image de tipo Filled para la barra de recarga")]
    public Image barraCooldown;

    [Header("Barra del Pulso con sprites")]
    [Tooltip("Picture6_71: lista para usar")] public Sprite pulsoListo;
    [Tooltip("Picture6_72: segundo cuadro del brillo (opcional)")] public Sprite pulsoListo2;
    [Tooltip("Picture6_73: vacía / recargando")] public Sprite pulsoVacio;
    public float velocidadBrillo = 4f;

    private bool hudExtraConstruido;
    private Image imagenPulso;
    private float ultimoCooldown = 1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // Si el usuario no creó el slot ni el icono en el editor, se generan por código
        ConstruirHUDExtraSiFalta();

        // Estado inicial según el progreso guardado (RF-19)
        MostrarFragmento(DatosJuego.ProgresoIgnar >= 1);
        MostrarHabilidad(DatosJuego.PulsoDesbloqueado);
    }

    /// <summary>mediosActuales va en medios corazones (10 = 5 corazones llenos).</summary>
    public void ActualizarVida(int mediosActuales, int corazonesMaximos)
    {
        if (corazones == null) return;

        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] == null) continue;

            int mediosDeEsteCorazon = mediosActuales - i * 2;

            if (mediosDeEsteCorazon >= 2)
                corazones[i].sprite = corazonLleno;
            else if (mediosDeEsteCorazon == 1)
                corazones[i].sprite = corazonMedio != null ? corazonMedio : corazonLleno;
            else
                corazones[i].sprite = corazonVacio;
        }
    }

    public void MostrarFragmento(bool obtenido)
    {
        if (slotFragmento == null) return;

        slotFragmento.gameObject.SetActive(true);
        if (fragmentoEncendido != null && fragmentoApagado != null)
            slotFragmento.sprite = obtenido ? fragmentoEncendido : fragmentoApagado;
        else
            slotFragmento.color = obtenido ? Color.white : new Color(1f, 1f, 1f, 0.25f);
    }

    public void MostrarHabilidad(bool desbloqueada)
    {
        if (iconoHabilidad != null)
            iconoHabilidad.SetActive(desbloqueada);
    }

    /// <summary>0 = recarga usada, 1 = lista para usar.</summary>
    public void ActualizarCooldown(float normalizado)
    {
        ultimoCooldown = Mathf.Clamp01(normalizado);
        if (barraCooldown != null) barraCooldown.fillAmount = ultimoCooldown;

        if (imagenPulso != null)
        {
            bool listo = ultimoCooldown >= 0.999f;
            if (barraCooldown != null) barraCooldown.enabled = !listo;
            if (!listo && pulsoVacio != null) imagenPulso.sprite = pulsoVacio;
        }
    }

    private void Update()
    {
        // Lista para usar: alterna los dos sprites encendidos para que brille
        if (imagenPulso == null || ultimoCooldown < 0.999f || pulsoListo == null) return;
        bool segundo = pulsoListo2 != null && Mathf.Repeat(Time.unscaledTime * velocidadBrillo, 2f) >= 1f;
        imagenPulso.sprite = segundo ? pulsoListo2 : pulsoListo;
    }

    // ------------------------------------------------------------------
    // HUD generado por código (solo si faltan referencias en el Inspector)
    // ------------------------------------------------------------------

    private void ConstruirHUDExtraSiFalta()
    {
        if (hudExtraConstruido) return;
        if (slotFragmento != null && iconoHabilidad != null && barraCooldown != null) return;

        hudExtraConstruido = true;

        Canvas canvas = ConstructorUI.CrearCanvas("Canvas_HUDExtra", -1);
        Transform raiz = canvas.transform;

        if (slotFragmento == null)
        {
            RectTransform rt = ConstructorUI.CrearRect(raiz, "SlotFragmento");
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.sizeDelta = new Vector2(58f, 58f);
            rt.anchoredPosition = new Vector2(432f, -122f);

            slotFragmento = rt.gameObject.AddComponent<Image>();
            slotFragmento.sprite = ConstructorUI.SpriteRedondeado;
            slotFragmento.type = Image.Type.Sliced;
            slotFragmento.color = new Color(1f, 1f, 1f, 0.25f);
        }

        if (iconoHabilidad == null && pulsoVacio != null)
        {
            // Barra con los sprites del juego, debajo de los corazones
            RectTransform rt = ConstructorUI.CrearRect(raiz, "BarraPulso");
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.sizeDelta = new Vector2(160f, 77f);
            rt.anchoredPosition = new Vector2(69f, -196f);
            iconoHabilidad = rt.gameObject;

            imagenPulso = rt.gameObject.AddComponent<Image>();
            imagenPulso.sprite = pulsoListo != null ? pulsoListo : pulsoVacio;
            imagenPulso.preserveAspect = true;

            // relleno de fuego dentro del tubo de Picture6_73
            RectTransform tubo = ConstructorUI.CrearRect(rt, "Recarga");
            tubo.anchorMin = new Vector2(0.392f, 0.395f);
            tubo.anchorMax = new Vector2(0.848f, 0.658f);
            tubo.offsetMin = Vector2.zero;
            tubo.offsetMax = Vector2.zero;
            barraCooldown = tubo.gameObject.AddComponent<Image>();
            barraCooldown.sprite = ConstructorUI.SpriteRedondeado;
            barraCooldown.type = Image.Type.Filled;
            barraCooldown.fillMethod = Image.FillMethod.Horizontal;
            barraCooldown.fillOrigin = (int)Image.OriginHorizontal.Left;
            barraCooldown.color = new Color(1f, 0.55f, 0.1f, 1f);
            barraCooldown.enabled = false;
        }

        if (iconoHabilidad == null)
        {
            RectTransform rt = ConstructorUI.CrearRect(raiz, "IconoPulso");
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.sizeDelta = new Vector2(58f, 58f);
            rt.anchoredPosition = new Vector2(98f, -78f);

            iconoHabilidad = rt.gameObject;

            Image fondo = rt.gameObject.AddComponent<Image>();
            fondo.sprite = ConstructorUI.SpriteRedondeado;
            fondo.type = Image.Type.Sliced;
            fondo.color = new Color(0.14f, 0.08f, 0.05f, 1f);

            RectTransform rellenoRT = ConstructorUI.CrearRect(rt, "Recarga");
            rellenoRT.anchorMin = new Vector2(0.12f, 0.12f);
            rellenoRT.anchorMax = new Vector2(0.88f, 0.88f);
            rellenoRT.offsetMin = Vector2.zero;
            rellenoRT.offsetMax = Vector2.zero;

            barraCooldown = rellenoRT.gameObject.AddComponent<Image>();
            barraCooldown.sprite = ConstructorUI.SpriteRedondeado;
            barraCooldown.type = Image.Type.Filled;
            barraCooldown.fillMethod = Image.FillMethod.Radial360;
            barraCooldown.fillOrigin = (int)Image.Origin360.Top;
            barraCooldown.fillClockwise = true;
            barraCooldown.color = ConstructorUI.ColorNaranja;

            ConstructorUI.CrearTexto(rt, "Tecla", "Q", 20f,
                new Vector2(30f, 24f), new Vector2(0f, -42f),
                TMPro.TextAlignmentOptions.Center,
                new Color(1f, 0.85f, 0.6f, 1f));
        }
    }
}
