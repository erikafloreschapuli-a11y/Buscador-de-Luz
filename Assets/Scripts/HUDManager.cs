using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ActualizarVida(int vidaActual, int vidaMaxima)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidaActual)
                corazones[i].sprite = corazonLleno;
            else
                corazones[i].sprite = corazonVacio;
        }
    }
}