using UnityEngine;

public class EstadisticasJuego : MonoBehaviour
{
    [Tooltip("Cada cuántos segundos se guarda el tiempo")]
    [SerializeField] private float intervaloGuardado = 10f;

    private float tiempo;
    private float contador;

    private void Start()
    {
        tiempo = DatosJuego.Tiempo;
    }

    private void Update()
    {
        // Con el juego pausado (timeScale 0) deltaTime vale 0, así que no cuenta
        tiempo += Time.deltaTime;
        contador += Time.deltaTime;

        if (contador >= intervaloGuardado)
        {
            contador = 0f;
            GuardarTiempo();
        }
    }

    private void GuardarTiempo()
    {
        DatosJuego.Tiempo = tiempo;
        DatosJuego.Guardar();
    }

    private void OnDestroy()
    {
        GuardarTiempo();
    }

    private void OnApplicationQuit()
    {
        GuardarTiempo();
    }
}