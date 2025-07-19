using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KilowattsText;
    [SerializeField] private Image Darken;
    [SerializeField] private TextMeshProUGUI BuildingsDown;

    private static UI Instance;

    private void Awake()
    {
        Instance = this;
        Darken.gameObject.SetActive(true);
        Darken.DOColor(Color.clear, .5f).From(Color.black).OnComplete(() =>
        {
            Darken.gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        Refresh();
    }

    public static void Refresh()
    {
        if(Instance == null) return;
        Instance.InstanceRefresh();
    }

    private void InstanceRefresh()
    {
        int kilos = GameManager.Instance.Kilowatts;
        KilowattsText.text = kilos + (" kW");
        
        int buildingsDown = BuildingManager.Instance.BuildingsDown;
        int buildings = BuildingManager.Instance.TotalBuildings;
        BuildingsDown.text = $"{buildingsDown} building{(buildingsDown == 1 ? "" : "s")} without power";
    }
}
