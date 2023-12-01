using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(SpriteFromAtlas))]
public class EffButtonHover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public string hover, normal;

    private SpriteFromAtlas sfa;
    public void Start() { sfa = GetComponent<SpriteFromAtlas>(); }
    public void OnPointerDown(PointerEventData e) { Enter(); }
    public void OnPointerUp(PointerEventData e) { Exit(); }

    public void OnPointerExit(PointerEventData e) { Exit(); }

    private void Enter() {
        sfa.SetImage(hover);
    }

    private void Exit() {
        sfa.SetImage(normal);
    }
}
