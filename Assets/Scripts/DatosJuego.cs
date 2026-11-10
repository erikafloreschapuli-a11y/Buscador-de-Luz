using UnityEngine;

public static class DatosJuego
{
    private const string KEY_PARTIDA = "HayPartidaGuardada"; // la misma que usa MainMenuManager
    private const string KEY_TIEMPO = "TiempoJuego";
    private const string KEY_ENEMIGOS = "EnemigosDerrotados";
    private const string KEY_CRISTALES = "CristalesLuz";
    private const string KEY_MEJORAS = "MejorasEncontradas";
    private const string KEY_PROGRESO = "ProgresoIgnar";

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