using UnityEngine;

/// <summary>
/// Solo en el editor de Unity: cada vez que se pulsa Play, se borra el checkpoint
/// guardado para que Kael empiece desde el inicio de la escena.
/// Reintentar (Game Over) no se ve afectado, porque esto solo corre al pulsar Play.
/// En el build de Windows NO hace nada: ahí "Continuar" sigue funcionando (RF-19).
/// </summary>
public static class ReinicioAlProbar
{
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Reiniciar()
    {
        DatosJuego.BorrarCheckpoint();
        DatosJuego.Tiempo = 0f;
        PlayerPrefs.Save();
    }
#endif
}
