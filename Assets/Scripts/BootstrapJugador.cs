using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Añade al jugador las habilidades que necesitan componente propio
/// (dash y Pulso de Calor) para que no haya que colocarlas a mano.
/// Se ejecuta al empezar y cada vez que se carga una escena (menú -> Ignar -> Zona2 -> Zona3, Reintentar).
/// Si ya existen en el prefab o en la escena, no se duplican.
/// </summary>
public static class BootstrapJugador
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
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador == null) return;

        if (jugador.GetComponent<PlayerDash>() == null)
            jugador.AddComponent<PlayerDash>();

        if (jugador.GetComponent<PulsoDeCalor>() == null)
            jugador.AddComponent<PulsoDeCalor>();
    }
}
