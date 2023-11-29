using UnityEngine;

public class EffScale : MonoBehaviour {
    private const float MIN_SCALE = 0, MAX_SCALE = 1f;

    private float direction = 1f, speed;
    [SerializeField] private float minSpeed = 0.44f, maxSpeed = 0.824f;

    public void Start() {
        speed = Random.Range(minSpeed, maxSpeed); 
        ScaleUp();
    }

    public void Update() {
        float scale = transform.localScale.x;

        if (direction == 0) return;

        if ((scale += speed * Time.deltaTime * direction) > MAX_SCALE) {
            scale = MAX_SCALE;
            direction = 0;
        }
        else if (scale < MIN_SCALE)  {
            scale = MIN_SCALE;
            direction = 0;
        }

        transform.localScale = new Vector2(scale, scale);
    }

    public void ScaleDown() {
        transform.localScale = Vector2.one;
        direction = -1;
    }

    public void ScaleUp() {
        transform.localScale = Vector2.zero;
        direction = 1f;
    }

    public bool IsFinished(){
        float scale = transform.localScale.x;

        return (direction == -1f && scale == MAX_SCALE) ||
               (direction ==  1f && scale == MIN_SCALE);
    }
}
