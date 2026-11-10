using TMPro;
using UnityEngine;

public class EstadisticasUI : MonoBehaviour
{
    [SerializeField] private TMP_Text valorTiempo;
    [SerializeField] private TMP_Text valorEnemigos;
    [SerializeField] private TMP_Text valorCristales;
    [SerializeField] private TMP_Text valorMejoras;

    private void OnEnable()
    {
        Actualizar();
    }

    public void Actualizar()
    {
        if (valorTiempo != null) valorTiempo.text = DatosJuego.FormatearTiempo(DatosJuego.Tiempo);
        if (valorEnemigos != null) valorEnemigos.text = DatosJuego.Enemigos.ToString();
        if (valorCristales != null) valorCristales.text = DatosJuego.Cristales + "/" + DatosJuego.CristalesTotales;
        if (valorMejoras != null) valorMejoras.text = DatosJuego.Mejoras + "/" + DatosJuego.MejorasTotales;
    }
}