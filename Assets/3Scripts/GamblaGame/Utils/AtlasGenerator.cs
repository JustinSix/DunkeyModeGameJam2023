using UnityEditor.U2D;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasGenerator {
    [MenuItem("Tools/Create Sprite Atlas")]
    public static void CreateSpriteAtlas() {
        string[] spritePaths = AssetDatabase.GetAssetPathsFromAssetBundle("YourSpriteBundle");

        SpriteAtlas atlas = new();
        atlas.Add(StringToObject(spritePaths));
        atlas.SetPackingSettings(new SpriteAtlasPackingSettings { });
        atlas.SetTextureSettings(new SpriteAtlasTextureSettings { });
        AssetDatabase.CreateAsset(atlas, "Assets/YourSpriteAtlas.spriteatlas");

        AssetDatabase.Refresh();
    }

    private static Object[] StringToObject(string[] arr) {
        int N = arr.Length;

        Object[] arr2 = new Object[N];
        for (int i = 0; i < N; i++) arr2[i] = AssetDatabase.LoadAssetAtPath<Object>(arr[i]);

        return arr2;
    }
}