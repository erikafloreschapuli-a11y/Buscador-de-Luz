using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Crea el AudioManager automáticamente (RF-47/48/49) y carga la música
/// de cada escena al entrar en ella. No hay que colocarlo a mano.
/// </summary>
public static class BootstrapAudio
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
        if (AudioManager.instance == null)
        {
            GameObject go = new GameObject("AudioManager");
            go.AddComponent<AudioManager>();
            Object.DontDestroyOnLoad(go);
        }

        if (AudioManager.instance != null)
            AudioManager.instance.CargarMusicaDeEscena();
    }
}
