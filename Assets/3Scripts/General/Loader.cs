using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        LoadingScene,
        MainMenu,
        GambleGame,
        FullGuysGame,
        StreamerScene,
    }

    private static Scene targetSceneIndex;


    public static void Load(Scene targetScene)
    {
        Loader.targetSceneIndex = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }


    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSceneIndex.ToString());
    }
}