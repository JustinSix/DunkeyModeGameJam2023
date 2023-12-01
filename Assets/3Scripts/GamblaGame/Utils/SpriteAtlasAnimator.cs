using UnityEngine;

[RequireComponent(typeof(SpriteFromAtlas))]
public class SpriteAtlasAnimator : MonoBehaviour {

    public string spritename;
    [Tooltip("Add extra zero if x < 10")]
    public bool addExtraZero = true;
    public float swapTime = 0.56f;
    public int totalImages = 1;
    [Tooltip("coin_0 if startWithZero == true")]
    public bool startWithZero = false;

    private float time = 0;
    private int id;
    private SpriteFromAtlas sfa;

    public void Start() {
        sfa = GetComponent<SpriteFromAtlas>();
        Reset();
    }
    public void Update() {
        time += Time.deltaTime;
        int possibleId = (int)(time / swapTime);
        if(possibleId > 0) {
            id = (id + possibleId) % totalImages; //go around
            if (id == 0 && !startWithZero) id = 1;
            sfa.SetImage(GetName());
            time %= swapTime;
        }
    }

    public void Reset() {
        sfa.SetImage(GetName());
        id = startWithZero? 0 : 1;
        time = 0;
    }

    private string GetName() {
        return addExtraZero && id < 10 ?
            (spritename + "_0" + id) : (spritename + "_" + id);
    }
}
