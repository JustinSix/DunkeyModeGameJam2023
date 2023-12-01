using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpriteFromAtlas : MonoBehaviour {

    public SpriteAtlas atlas;
    public string spriteName;

    public void Start(){
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }

    public void SetImage(string spriteName) {
        this.spriteName = spriteName;
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }

    public void Set(SpriteAtlas atlas, string spriteName) {
        this.atlas = atlas;
        this.spriteName = spriteName;
    }
}