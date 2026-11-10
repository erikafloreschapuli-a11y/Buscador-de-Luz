using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int vidaMaxima = 5;
    private int vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
        HUDManager.instance.ActualizarVida(vidaActual, vidaMaxima);
    }

    public void RecibirDaño(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        HUDManager.instance.ActualizarVida(vidaActual, vidaMaxima);
      
        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
       if (GameOverManager.Instance != null) GameOverManager.Instance.MostrarGameOver();
        gameObject.SetActive(false);
        Invoke("Respawn", 1f);
    }

    void Respawn()
    {
        vidaActual = vidaMaxima;
        HUDManager.instance.ActualizarVida(vidaActual, vidaMaxima);
        GameManager.instance.RespawnPlayer();
    }

    public int GetVida() => vidaActual;
    public int GetVidaMaxima() => vidaMaxima;
}