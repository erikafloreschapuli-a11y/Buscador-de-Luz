// BotonMenuHover.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BotonMenuHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Colores")]
    [SerializeField] private Color colorNormal = new Color(0.05f, 0.05f, 0.1f, 0.8f);
    [SerializeField] private Color colorHover  = new Color(1f, 0.67f, 0.2f, 1f); // #FFAA33

    [Header("Efecto")]
    [SerializeField] private float escalaHover = 1.05f;
    [SerializeField] private float velocidad = 12f;

    [Header("Sonido (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoHover;

    private Image imagen;
    private Vector3 escalaInicial;
    private bool sobreElBoton;

    private void Awake()
    {
        imagen = GetComponent<Image>();
        escalaInicial = transform.localScale;
        imagen.color = colorNormal;
    }

    private void Update()
    {
        Vector3 escalaObjetivo = sobreElBoton ? escalaInicial * escalaHover : escalaInicial;
        Color colorObjetivo = sobreElBoton ? colorHover : colorNormal;

        transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, Time.unscaledDeltaTime * velocidad);
        imagen.color = Color.Lerp(imagen.color, colorObjetivo, Time.unscaledDeltaTime * velocidad);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sobreElBoton = true;
        if (audioSource != null && sonidoHover != null) audioSource.PlayOneShot(sonidoHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sobreElBoton = false;
    }
}