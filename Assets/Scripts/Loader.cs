using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scene targetScene;

    public enum Scene
    {
        MainMenu,
        LoadingScene,
        Game,
        Lobby,
        CharacterSelect
    }

    public static void Load(Scene scene)
    {
        targetScene = scene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString()); 
    }

    public static void LoadNetwork(Scene scene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
