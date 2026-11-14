using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Audio central del juego (RF-47/48/49).
/// Se crea solo al arrancar cualquier escena, así no hace falta colocarlo a mano.
/// La música se carga de Assets/Resources/Audio/Musica/&lt;NombreDeEscena&gt;
/// (por ejemplo Menu.ogg e Ignar.ogg).
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Fuentes")]
    [SerializeField] private AudioSource fuenteMusica;
    [SerializeField] private AudioSource fuenteSFX;

    [Header("Música de esta escena")]
    [SerializeField] private AudioClip musicaInicio;
    [SerializeField] private bool repetirMusica = true;

    [Header("Volumen (se guarda en PlayerPrefs)")]
    [Range(0f, 1f)] public float volumenGeneral = 1f;

    private const string KEY_VOLUMEN = "VolumenGeneral";
    private string escenaMusicaActual;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        AsegurarFuentes();

        volumenGeneral = PlayerPrefs.GetFloat(KEY_VOLUMEN, 1f);
        AplicarVolumen();
    }

    private void Start()
    {
        if (musicaInicio != null)
            ReproducirMusica(musicaInicio);
        else
            CargarMusicaDeEscena();
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    private void AsegurarFuentes()
    {
        if (fuenteMusica == null)
        {
            fuenteMusica = gameObject.AddComponent<AudioSource>();
            fuenteMusica.playOnAwake = false;
            fuenteMusica.loop = true;
            fuenteMusica.spatialBlend = 0f;
        }

        if (fuenteSFX == null)
        {
            fuenteSFX = gameObject.AddComponent<AudioSource>();
            fuenteSFX.playOnAwake = false;
            fuenteSFX.spatialBlend = 0f;
        }
    }

    /// <summary>
    /// Carga la música de la escena actual desde Resources.
    /// Nombres admitidos: Audio/Musica/&lt;Escena&gt; y Audio/Musica/Tema.
    /// </summary>
    public void CargarMusicaDeEscena()
    {
        string escena = SceneManager.GetActiveScene().name;
        if (escena == escenaMusicaActual) return;

        escenaMusicaActual = escena;

        AudioClip clip = Resources.Load<AudioClip>("Audio/Musica/" + escena);
        if (clip == null)
            clip = Resources.Load<AudioClip>("Audio/Musica/Tema");

        if (clip != null)
            ReproducirMusica(clip);
    }

    public void ReproducirMusica(AudioClip clip)
    {
        if (clip == null || fuenteMusica == null) return;

        if (fuenteMusica.clip == clip && fuenteMusica.isPlaying) return;

        fuenteMusica.clip = clip;
        fuenteMusica.loop = repetirMusica;
        fuenteMusica.Play();
    }

    public void ReproducirSFX(AudioClip clip, float volumen = 1f)
    {
        if (clip == null) return;

        if (fuenteSFX != null)
            fuenteSFX.PlayOneShot(clip, Mathf.Clamp01(volumen) * volumenGeneral);
        else
            AudioSource.PlayClipAtPoint(clip, transform.position, Mathf.Clamp01(volumen) * volumenGeneral);
    }

    public void SetVolumen(float valor)
    {
        volumenGeneral = Mathf.Clamp01(valor);
        PlayerPrefs.SetFloat(KEY_VOLUMEN, volumenGeneral);
        PlayerPrefs.Save();
        AplicarVolumen();
    }

    private void AplicarVolumen()
    {
        if (fuenteMusica != null) fuenteMusica.volume = volumenGeneral;
        if (fuenteSFX != null) fuenteSFX.volume = volumenGeneral;
    }

    /// <summary>Atajo para cualquier script: AudioManager.SFX(clip);</summary>
    public static void SFX(AudioClip clip, float volumen = 1f, Vector3? posicion = null)
    {
        if (clip == null) return;

        if (instance != null)
        {
            instance.ReproducirSFX(clip, volumen);
            return;
        }

        Vector3 pos = posicion ?? Vector3.zero;
        AudioSource.PlayClipAtPoint(clip, pos, Mathf.Clamp01(volumen));
    }
}
