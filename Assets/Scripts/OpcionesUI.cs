using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RF-43/44: opciones de volumen y pantalla, reutilizable en el menú y en la pausa.
/// Conecta los métodos a los componentes de UI desde el Inspector.
/// </summary>
public class OpcionesUI : MonoBehaviour
{
    [Header("Controles (opcionales)")]
    [SerializeField] private Slider volumen;
    [SerializeField] private Toggle pantallaCompleta;
    [SerializeField] private Dropdown resolucion;

    [Header("Panel")]
    [Tooltip("Panel que se cierra al pulsar Volver. Si está vacío se usa este objeto")]
    [SerializeField] private GameObject panel;

    private const string KEY_VOLUMEN = "VolumenGeneral";

    private void OnEnable()
    {
        Sincronizar();
    }

    /// <summary>Asigna los controles cuando el panel se construye por código.</summary>
    public void Configurar(Slider controlVolumen, Toggle controlPantalla, Dropdown controlResolucion, GameObject panelContenedor)
    {
        volumen = controlVolumen;
        pantallaCompleta = controlPantalla;
        resolucion = controlResolucion;
        panel = panelContenedor;

        Sincronizar();
    }

    private void Sincronizar()
    {
        if (volumen != null)
        {
            volumen.minValue = 0f;
            volumen.maxValue = 1f;
            volumen.value = PlayerPrefs.GetFloat(KEY_VOLUMEN, 1f);
        }

        if (pantallaCompleta != null)
            pantallaCompleta.isOn = Screen.fullScreen;

        if (resolucion != null)
            RellenarResoluciones();
    }

    private void RellenarResoluciones()
    {
        Resolution[] resoluciones = Screen.resolutions;
        if (resoluciones == null || resoluciones.Length == 0) return;

        List<string> opciones = new List<string>();
        int indiceActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            Resolution r = resoluciones[i];
            opciones.Add($"{r.width} x {r.height}");

            if (r.width == Screen.width && r.height == Screen.height)
                indiceActual = i;
        }

        resolucion.ClearOptions();
        resolucion.AddOptions(opciones);
        resolucion.value = indiceActual;
        resolucion.RefreshShownValue();
    }

    public void CambiarVolumen(float valor)
    {
        float v = Mathf.Clamp01(valor);
        PlayerPrefs.SetFloat(KEY_VOLUMEN, v);
        PlayerPrefs.Save();

        if (AudioManager.instance != null)
            AudioManager.instance.SetVolumen(v);
    }

    public void CambiarPantallaCompleta(bool completa)
    {
        Screen.fullScreen = completa;
    }

    public void CambiarResolucion(int indice)
    {
        Resolution[] resoluciones = Screen.resolutions;
        if (resoluciones == null || indice < 0 || indice >= resoluciones.Length) return;

        Resolution r = resoluciones[indice];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

    public void Abrir()
    {
        if (panel != null) panel.SetActive(true);
        else gameObject.SetActive(true);
    }

    public void Cerrar()
    {
        if (panel != null) panel.SetActive(false);
        else gameObject.SetActive(false);
    }
}
