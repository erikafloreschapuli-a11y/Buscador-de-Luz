using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Garantiza que la pantalla de "Fragmento obtenido" (RF-46) exista en cada
/// escena de juego aunque no se haya colocado a mano. En el menú no se crea.
/// </summary>
public static class BootstrapFragmento
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Iniciar()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
        SceneManager.sceneLoaded += AlCargarEscena;
        Aplicar();
    }

    private static void AlCargarEscena(Scene escena, LoadSceneMode modo) => Aplicar();

    private static void Aplicar()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        if (Object.FindAnyObjectByType<PanelFragmento>() != null) return;

        GameObject go = new GameObject("PanelFragmento");
        go.AddComponent<PanelFragmento>();
    }
}
