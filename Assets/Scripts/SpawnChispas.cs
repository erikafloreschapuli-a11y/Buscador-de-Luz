using UnityEngine;

/// <summary>
/// Genera grupos de Chispas en los puntos indicados (RF-23).
/// </summary>
public class SpawnChispas : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject chispaPrefab;
    public Transform[] puntoSpawn;
    public float tiempoEntreSpawn = 5f;

    private float timer;
    private GameObject[] chispasVivas;

    private void Start()
    {
        CrearPuntosSiFaltan();

        if (chispaPrefab == null) return;
        if (puntoSpawn == null || puntoSpawn.Length == 0) return;

        chispasVivas = new GameObject[puntoSpawn.Length];
        SpawnearGrupo();
    }

    /// <summary>Si no se asignaron puntos, crea tres alrededor del objeto (grupo de 3, RF-23).</summary>
    private void CrearPuntosSiFaltan()
    {
        if (puntoSpawn != null && puntoSpawn.Length > 0) return;

        Vector3[] posiciones =
        {
            new Vector3(-3f, 2f, 0f),
            new Vector3(0f, 3.5f, 0f),
            new Vector3(3f, 2f, 0f)
        };

        puntoSpawn = new Transform[posiciones.Length];

        for (int i = 0; i < posiciones.Length; i++)
        {
            GameObject punto = new GameObject("PuntoChispa" + (i + 1));
            punto.transform.SetParent(transform, false);
            punto.transform.localPosition = posiciones[i];
            puntoSpawn[i] = punto.transform;
        }
    }

    private void Update()
    {
        if (chispaPrefab == null || puntoSpawn == null || puntoSpawn.Length == 0) return;

        timer += Time.deltaTime;
        if (timer < tiempoEntreSpawn) return;

        timer = 0f;
        SpawnearGrupo();
    }

    private void SpawnearGrupo()
    {
        for (int i = 0; i < puntoSpawn.Length; i++)
        {
            if (puntoSpawn[i] == null) continue;

            if (chispasVivas[i] == null)
                chispasVivas[i] = Instantiate(chispaPrefab, puntoSpawn[i].position, Quaternion.identity);
        }
    }
}
