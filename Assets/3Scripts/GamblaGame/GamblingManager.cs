using UnityEngine;
using UnityEngine.UI;
public class GamblingManager : MonoBehaviour
{
    [SerializeField] private Button betOne, betAll, spin, inc, dec;
    [SerializeField] private GGG ggg;

    public void Start() {
        betOne.onClick.AddListener(() => ggg.BetOne());
        betAll.onClick.AddListener(() => ggg.BetAll());
        spin.onClick.AddListener(() => ggg.Spin());
        inc.onClick.AddListener(() => ggg.Inc());
        dec.onClick.AddListener(() => ggg.Dec());
    }
}
