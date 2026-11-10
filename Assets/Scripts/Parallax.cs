using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform cam;

    [Header("Efecto parallax")]
    [Tooltip("0 = fijo en pantalla (muy lejano), 1 = se mueve igual que el mundo")]
    [SerializeField, Range(0f, 1f)] private float intensidadX = 0.2f;
    [SerializeField, Range(0f, 1f)] private float intensidadY = 0f;

    [Header("Acercamiento")]
    [Tooltip("1 = se ve la imagen completa, 2 = el doble de cerca, 3 = el triple...")]
    [SerializeField, Range(1f, 6f)] private float zoom = 2.5f;
    [Tooltip("Qué parte vertical de la imagen se ve (0 a 1). Pruébalo en Play.")]
    [SerializeField, Range(0f, 1f)] private float posicionVertical = 0.5f;

    [Header("Opciones")]
    [Tooltip("El plano acompaña a la cámara para que nunca se salga de la vista")]
    [SerializeField] private bool seguirCamara = true;
    [Tooltip("Velocidad extra automática (nubes, humo). 0 = desactivado")]
    [SerializeField] private float velocidadAutomaticaX = 0f;

    private Renderer rend;
    private Material material;
    private Vector3 ultimaPosCamara;
    private float offsetX;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        material = rend.material; // instancia propia del material

        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;
    }

    private void Start()
    {
        if (cam != null) ultimaPosCamara = cam.position;
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        // Tamaño del plano en el mundo (un Plane girado -90 en X tiene su alto en Y)
        Vector3 tamano = rend.bounds.size;
        float ancho = Mathf.Max(tamano.x, 0.0001f);
        float alto = Mathf.Max(Mathf.Max(tamano.y, tamano.z), 0.0001f);
        float aspectoPlano = ancho / alto;

        // Proporción de la imagen, para que no se estire
        Texture tex = material.mainTexture;
        float aspectoImagen = (tex != null && tex.height > 0) ? (float)tex.width / tex.height : 1f;

        // Parte de la imagen que se muestra (en UV)
        float uvAlto = 1f / zoom;
        float uvAncho = aspectoPlano / (aspectoImagen * zoom);

        // Cuánto se movió la cámara en este frame
        Vector3 delta = cam.position - ultimaPosCamara;
        ultimaPosCamara = cam.position;

        // Fracción del plano que se movió, convertida a UV visibles
        offsetX += (delta.x * intensidadX / ancho) * uvAncho;
        offsetX += (velocidadAutomaticaX * Time.deltaTime / ancho) * uvAncho;
        offsetX = Mathf.Repeat(offsetX, 1f);

        // Tiling Y negativo = imagen derecha (corrige el volteo del Plane)
        material.mainTextureScale = new Vector2(uvAncho, -uvAlto);
        material.mainTextureOffset = new Vector2(offsetX, posicionVertical + uvAlto * 0.5f);

        // El plano sigue a la cámara (solo X e Y, conserva su Z)
        if (seguirCamara)
        {
            transform.position = new Vector3(cam.position.x, cam.position.y, transform.position.z);
        }
    }

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
    }
}