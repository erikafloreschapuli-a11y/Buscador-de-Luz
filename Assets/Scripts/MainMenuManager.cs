// MainMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Escenas")]
    [SerializeField] private string escenaJuego = "Ignar";

    [Header("Paneles")]
    [SerializeField] private GameObject panelPrincipal;
    [SerializeField] private GameObject panelOpciones;
    [SerializeField] private GameObject panelCreditos;
    [SerializeField] private GameObject panelMejoras;
    [SerializeField] private GameObject panelMapa;

    [Header("Botón Continuar")]
    [SerializeField] private GameObject botonContinuar;

    private const string KEY_PARTIDA = "HayPartidaGuardada";

    private void Start()
    {
        // Solo muestra "Continuar" si ya existe una partida
        bool hayPartida = PlayerPrefs.GetInt(KEY_PARTIDA, 0) == 1;
        if (botonContinuar != null) botonContinuar.SetActive(hayPartida);

        PrepararPaneles();
        MostrarPanel(panelPrincipal);
    }

    public void Continuar()
    {
        string escena = DatosJuego.EscenaGuardada;
        if (!Application.CanStreamedLevelBeLoaded(escena)) escena = escenaJuego;
        SceneManager.LoadScene(escena);
    }

    public void NuevaPartida()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(KEY_PARTIDA, 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(escenaJuego);
    }

    public void AbrirOpciones()
    {
        PrepararPaneles();
        MostrarPanel(panelOpciones);
    }

    public void AbrirCreditos()
    {
        PrepararPaneles();
        MostrarPanel(panelCreditos);
    }

    public void AbrirMejoras()
    {
        PrepararPaneles();
        MostrarPanel(panelMejoras);
    }

    public void AbrirMapa()
    {
        PrepararPaneles();
        MostrarPanel(panelMapa);
    }

    public void Volver() => MostrarPanel(panelPrincipal);

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void MostrarPanel(GameObject activo)
    {
        if (activo == null) activo = panelPrincipal;

        if (panelPrincipal != null) panelPrincipal.SetActive(panelPrincipal == activo);
        if (panelOpciones != null) panelOpciones.SetActive(panelOpciones == activo);
        if (panelCreditos != null) panelCreditos.SetActive(panelCreditos == activo);
        if (panelMejoras != null) panelMejoras.SetActive(panelMejoras == activo);
        if (panelMapa != null) panelMapa.SetActive(panelMapa == activo);
    }

    /// <summary>
    /// Si los paneles no están creados en la escena, se construyen por código.
    /// Así todos los botones del menú hacen algo desde el primer Play.
    /// </summary>
    private void PrepararPaneles()
    {
        if (panelOpciones == null)
        {
            Canvas canvas = ConstructorUI.CrearCanvas("Canvas_OpcionesMenu", 30);
            OpcionesUI ui = ConstructorUI.CrearPanelOpciones(canvas.transform, "PanelOpcionesMenu", Volver);
            panelOpciones = ui.gameObject;
        }

        if (panelCreditos == null)
        {
            string creditos =
                "BUSCADOR DE LUZ\n\n" +
                "Un viaje por Ignar, el planeta de las Forjas Dormidas.\n\n" +
                "Kael debe recuperar el Fragmento de Aethon y despertar el Pulso de Calor.\n\n" +
                "Diseño y programación: [TU NOMBRE]\n" +
                "Arte y música: [CRÉDITOS]\n" +
                "Motor: Unity 6\n\n" +
                "Gracias por jugar la beta.";

            panelCreditos = ConstructorUI.CrearPanelTexto("PanelCreditos", "CRÉDITOS", creditos, Volver);
        }

        if (panelMejoras == null)
        {
            string mejoras =
                $"Cristales de Luz: {DatosJuego.Cristales} / {DatosJuego.CristalesTotales}\n" +
                $"Mejoras encontradas: {DatosJuego.Mejoras} / {DatosJuego.MejorasTotales}\n" +
                $"Enemigos derrotados: {DatosJuego.Enemigos}\n" +
                $"Tiempo de juego: {DatosJuego.FormatearTiempo(DatosJuego.Tiempo)}\n\n" +
                $"Progreso de Ignar: {(DatosJuego.ProgresoIgnar >= 1 ? "COMPLETADO" : "EN CURSO")}\n" +
                $"Pulso de Calor: {(DatosJuego.PulsoDesbloqueado ? "Desbloqueado" : "Bloqueado")}";

            panelMejoras = ConstructorUI.CrearPanelTexto("PanelMejoras", "MEJORAS", mejoras, Volver);
        }

        if (panelMapa == null)
        {
            string mapa =
                "MAPA DE IGNAR\n\n" +
                "ZONA 1 — Forjas Dormidas\n" +
                "Lava, güiseres, palancas y la primera zona secreta.\n\n" +
                "ZONA 2 — Canales de Magma\n" +
                "Puentes que se derrumban, lluvia de rocas y el puzzle de válvulas.\n\n" +
                "ZONA 3 — El Núcleo\n" +
                "Plataformas sobre el lago de lava y la arena de Molrak.\n\n" +
                "Completa las tres zonas para recuperar el Fragmento de Aethon.";

            panelMapa = ConstructorUI.CrearPanelTexto("PanelMapa", "MAPA", mapa, Volver);
        }
    }
}
