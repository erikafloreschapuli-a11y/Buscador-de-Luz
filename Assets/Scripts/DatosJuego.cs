using UnityEngine;
using UnityEngine.SceneManagement;

public static class DatosJuego
{
    private const string KEY_PARTIDA = "HayPartidaGuardada"; // la misma que usa MainMenuManager
    private const string KEY_TIEMPO = "TiempoJuego";
    private const string KEY_ENEMIGOS = "EnemigosDerrotados";
    private const string KEY_CRISTALES = "CristalesLuz";
    private const string KEY_MEJORAS = "MejorasEncontradas";
    private const string KEY_PROGRESO = "ProgresoIgnar";
    private const string KEY_CP_ACTIVO = "CheckpointActivo";
    private const string KEY_CP_X = "CheckpointX";
    private const string KEY_CP_Y = "CheckpointY";
    private const string KEY_PULSO = "PulsoDesbloqueado";
    private const string KEY_ESCENA = "EscenaGuardada"; // escena del checkpoint / del último nivel alcanzado
    public const string EscenaInicial = "Ignar";

    public const int CristalesTotales = 100;
    public const int MejorasTotales = 10;

    public static float Tiempo
    {
        get => PlayerPrefs.GetFloat(KEY_TIEMPO, 0f);
        set => PlayerPrefs.SetFloat(KEY_TIEMPO, value);
    }

    public static int Enemigos
    {
        get => PlayerPrefs.GetInt(KEY_ENEMIGOS, 0);
        set => PlayerPrefs.SetInt(KEY_ENEMIGOS, value);
    }

    public static int Cristales
    {
        get => PlayerPrefs.GetInt(KEY_CRISTALES, 0);
        set => PlayerPrefs.SetInt(KEY_CRISTALES, value);
    }

    public static int Mejoras
    {
        get => PlayerPrefs.GetInt(KEY_MEJORAS, 0);
        set => PlayerPrefs.SetInt(KEY_MEJORAS, value);
    }

    public static int ProgresoIgnar
    {
        get => PlayerPrefs.GetInt(KEY_PROGRESO, 0);
        set => PlayerPrefs.SetInt(KEY_PROGRESO, value);
    }

    // RF-11: se desbloquea al derrotar a Molrak y recoger el Fragmento
    public static bool PulsoDesbloqueado
    {
        get => PlayerPrefs.GetInt(KEY_PULSO, 0) == 1;
        set => PlayerPrefs.SetInt(KEY_PULSO, value ? 1 : 0);
    }

    // Llámalas desde los scripts del juego
    public static void SumarEnemigo()
    {
        Enemigos++;
        Guardar();
    }
    public static void SumarCristal()
    {
        Cristales = Mathf.Min(Cristales + 1, CristalesTotales);
        Guardar();
    }

    public static void SumarMejora()
    {
        Mejoras = Mathf.Min(Mejoras + 1, MejorasTotales);
        Guardar();
    }

    // RF-19: posición del checkpoint guardada en disco
    public static void GuardarCheckpoint(Vector3 pos)
    {
        PlayerPrefs.SetInt(KEY_CP_ACTIVO, 1);
        PlayerPrefs.SetFloat(KEY_CP_X, pos.x);
        PlayerPrefs.SetFloat(KEY_CP_Y, pos.y);
        PlayerPrefs.SetString(KEY_ESCENA, SceneManager.GetActiveScene().name);
        Guardar();
    }

    public static bool IntentarCargarCheckpoint(out Vector3 pos)
    {
        if (PlayerPrefs.GetInt(KEY_CP_ACTIVO, 0) == 1)
        {
            pos = new Vector3(
                PlayerPrefs.GetFloat(KEY_CP_X, 0f),
                PlayerPrefs.GetFloat(KEY_CP_Y, 0f),
                0f);
            return true;
        }

        pos = Vector3.zero;
        return false;
    }

    /// <summary>Escena donde está el checkpoint guardado (o el último nivel alcanzado).</summary>
    public static string EscenaGuardada => PlayerPrefs.GetString(KEY_ESCENA, EscenaInicial);

    /// <summary>Solo devuelve el checkpoint si pertenece a la escena indicada.</summary>
    public static bool IntentarCargarCheckpoint(string escena, out Vector3 pos)
    {
        if (escena != EscenaGuardada) { pos = Vector3.zero; return false; }
        return IntentarCargarCheckpoint(out pos);
    }

    /// <summary>Al pasar de nivel: Continuar abrirá la nueva escena desde su inicio.</summary>
    public static void GuardarInicioEscena(string escena)
    {
        BorrarCheckpoint();
        PlayerPrefs.SetString(KEY_ESCENA, escena);
        Guardar();
    }

    public static void BorrarCheckpoint()
    {
        PlayerPrefs.DeleteKey(KEY_CP_ACTIVO);
        PlayerPrefs.DeleteKey(KEY_CP_X);
        PlayerPrefs.DeleteKey(KEY_CP_Y);
    }

    public static void Guardar()
    {
        PlayerPrefs.SetInt(KEY_PARTIDA, 1);
        PlayerPrefs.Save();
    }

    public static string FormatearTiempo(float segundos)
    {
        int total = Mathf.FloorToInt(segundos);
        int h = total / 3600;
        int m = (total % 3600) / 60;
        int s = total % 60;
        return $"{h:00}:{m:00}:{s:00}";
    }
}