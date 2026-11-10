using UnityEngine;

public class SpawnChispas : MonoBehaviour
{
    public GameObject chispaPrefab;
    public Transform[] puntoSpawn;
    public float tiempoEntreSpawn = 5f;
    private float timer = 0f;
    private GameObject[] chispasVivas;

    private void Start()
    {
        chispasVivas = new GameObject[puntoSpawn.Length];
        SpawnearGrupo();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tiempoEntreSpawn)
        {
            timer = 0f;
            SpawnearGrupo();
        }
    }

    void SpawnearGrupo()
    {
        for (int i = 0; i < puntoSpawn.Length; i++)
        {
            if (chispasVivas[i] == null)
                chispasVivas[i] = Instantiate(chispaPrefab, puntoSpawn[i].position, Quaternion.identity);
        }
    }
}