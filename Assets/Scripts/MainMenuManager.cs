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

    [Header("Botón Continuar")]
    [SerializeField] private GameObject botonContinuar;

    private const string KEY_PARTIDA = "HayPartidaGuardada";

    private void Start()
    {
        // Solo muestra "Continuar" si ya existe una partida
        bool hayPartida = PlayerPrefs.GetInt(KEY_PARTIDA, 0) == 1;
        if (botonContinuar != null) botonContinuar.SetActive(hayPartida);

        MostrarPanel(panelPrincipal);
    }

    public void Continuar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void NuevaPartida()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(KEY_PARTIDA, 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(escenaJuego);
    }

    public void AbrirOpciones()  => MostrarPanel(panelOpciones);
    public void AbrirCreditos()  => MostrarPanel(panelCreditos);
    public void AbrirMejoras()   => MostrarPanel(panelMejoras);
    public void Volver()         => MostrarPanel(panelPrincipal);

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
        if (panelPrincipal != null) panelPrincipal.SetActive(panelPrincipal == activo);
        if (panelOpciones != null)  panelOpciones.SetActive(panelOpciones == activo);
        if (panelCreditos != null)  panelCreditos.SetActive(panelCreditos == activo);
        if (panelMejoras != null)   panelMejoras.SetActive(panelMejoras == activo);
    }
}