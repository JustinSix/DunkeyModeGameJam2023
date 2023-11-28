using UnityEngine;

public class EffRotate : MonoBehaviour {

    private static Vector3 ROTATE_NORMAL = new(0f, 0f, 1f);

    private RectTransform box;
    [SerializeField] float speed = 4.2f; //angle in degrees

    public void Start() {
        box = GetComponent<RectTransform>();
    }

    public void Update() {
        float delta = Time.deltaTime;
        box.Rotate(ROTATE_NORMAL, delta * speed);
    }
}
