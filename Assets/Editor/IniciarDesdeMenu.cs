using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Solo en el editor: al pulsar Play se empieza siempre desde MainMenu,
/// como en el build. Se activa o desactiva en el menú
/// "Buscador de Luz > Iniciar Play desde el menú principal".
/// Desactívalo cuando quieras probar directamente Zona2 o Zona3.
/// </summary>
[InitializeOnLoad]
public static class IniciarDesdeMenu
{
    private const string CLAVE = "BuscadorDeLuz.IniciarDesdeMenu";
    private const string MENU = "Buscador de Luz/Iniciar Play desde el menú principal";
    private const string ESCENA_MENU = "Assets/Scenes/MainMenu.unity";

    static IniciarDesdeMenu()
    {
        EditorApplication.delayCall += Aplicar;
    }

    private static bool Activo => EditorPrefs.GetBool(CLAVE, true);

    [MenuItem(MENU)]
    private static void Alternar()
    {
        EditorPrefs.SetBool(CLAVE, !Activo);
        Aplicar();
    }

    [MenuItem(MENU, true)]
    private static bool Validar()
    {
        Menu.SetChecked(MENU, Activo);
        return true;
    }

    private static void Aplicar()
    {
        EditorSceneManager.playModeStartScene = Activo ? AssetDatabase.LoadAssetAtPath<SceneAsset>(ESCENA_MENU) : null;
    }
}
