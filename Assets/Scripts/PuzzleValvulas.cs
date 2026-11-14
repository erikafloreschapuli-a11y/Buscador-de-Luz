using UnityEngine;

/// <summary>
/// RF-31: puzzle de 3 válvulas. Hay que activarlas en el orden correcto.
/// Si se falla, todas vuelven a su estado inicial.
/// </summary>
public class PuzzleValvulas : MonoBehaviour
{
    [Header("Orden correcto")]
    [Tooltip("Índices de las válvulas en el orden en que deben activarse")]
    public int[] ordenCorrecto = { 0, 1, 2 };

    [Header("Válvulas")]
    public Valvula[] valvulas;

    [Header("Qué se abre al resolverlo")]
    public Compuerta compuerta;
    public PuertaJefe puertaJefe;
    public GameObject objetoQueSeAbre;

    [Header("Audio")]
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoError;
    public AudioClip sonidoResuelto;

    public bool Resuelto { get; private set; }

    private int paso;

    public void ActivarValvula(Valvula valvula)
    {
        if (Resuelto || valvula == null) return;

        if (paso >= ordenCorrecto.Length || valvula.indice != ordenCorrecto[paso])
        {
            Fallar();
            return;
        }

        paso++;
        AudioManager.SFX(sonidoCorrecto, 0.7f, transform.position);

        if (paso >= ordenCorrecto.Length)
            Resolver();
    }

    private void Fallar()
    {
        paso = 0;
        AudioManager.SFX(sonidoError, 1f, transform.position);

        if (valvulas == null) return;
        foreach (Valvula v in valvulas)
        {
            if (v != null) v.Reiniciar();
        }
    }

    private void Resolver()
    {
        Resuelto = true;
        AudioManager.SFX(sonidoResuelto, 1f, transform.position);

        if (compuerta != null) compuerta.Abrir();
        if (puertaJefe != null) puertaJefe.Abrir();
        if (objetoQueSeAbre != null) objetoQueSeAbre.SetActive(true);
    }
}
