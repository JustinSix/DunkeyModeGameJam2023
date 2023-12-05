using UnityEngine;
using UnityEngine.UI;

public class EffOpacity : MonoBehaviour {

    private float opacity = 0f, direction = 1f;
    private Graphic graphic;
    [SerializeField] private float speed = 4.2f; //angle in degrees

    public void Start() {
        graphic = GetComponent<Graphic>();
    }

    private void SetOpacityValue(float alpha) {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }

    public void Update() {
        float delta = Time.deltaTime;

        if((opacity += speed * delta * direction) > 1f) {
            opacity = 1f;
            direction *= -1;
        } else if(opacity < 0f) {
            opacity = 0f;
            direction *= -1;
        }

        SetOpacityValue(opacity);
    }
}
