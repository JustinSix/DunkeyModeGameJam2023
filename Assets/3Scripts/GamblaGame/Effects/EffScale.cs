using UnityEngine;

public class EffScale : MonoBehaviour {
    private float direction = 1f, speed;

    [SerializeField] private float minSpeed = 0.44f, maxSpeed = 0.824f;
    [Space]
    [SerializeField] private bool infinity = false;
    [SerializeField] private float minScale = 0f, maxScale = 1f;

    public void Start() {
        speed = Random.Range(minSpeed, maxSpeed); 
    }

    public void Update() {
        float scale = transform.localScale.x;

        if (direction == 0 && !infinity) return;

        scale += speed * Time.deltaTime * direction;
        if (direction == 1f) {
            if (scale > maxScale) {
                scale = maxScale;
                direction *= infinity? -1 : 0;
            }
        } else if (scale < minScale)  {
            scale = minScale;
            direction *= infinity? -1 : 0;
        }

        transform.localScale = new Vector2(scale, scale);
    }

    public void ScaleDown() {
        transform.localScale = new(maxScale, maxScale);
        direction = -1;
    }

    public void ScaleUp() {
        transform.localScale = new(minScale, minScale);
        direction = 1f;
    }

    public void Set(float minScale, float maxScale) {
        this.minScale = minScale;
        this.maxScale = maxScale;
    }

    public bool IsFinished(){
        if (infinity) return false;

        float scale = transform.localScale.x;

        return (direction == -1f && scale == maxSpeed) ||
               (direction ==  1f && scale == minScale);
    }
}
