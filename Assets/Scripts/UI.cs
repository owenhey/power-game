using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI KilowattsText;
        [SerializeField] private TextMeshProUGUI BuildingsDown;

        private static UI Instance;

        private void Awake()
        {
            Instance = this;
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
            KilowattsText.text = kilos + (kilos == 1 ? " Kilowatt" : " Kilowatts");
            
            int buildingsDown = BuildingManager.Instance.BuildingsDown;
            int buildings = BuildingManager.Instance.TotalBuildings;
            BuildingsDown.text = $"{buildingsDown} building{(buildingsDown == 1 ? "" : "s")} without power";
        }
    }
}