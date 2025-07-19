using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KilowattsText;
    [SerializeField] private Image Darken;
    [SerializeField] private TextMeshProUGUI BuildingsDown;
    [SerializeField] private CanvasGroup PauseMenu;
    [SerializeField] private Button PauseButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer mixer;

    private static UI Instance;

    public static bool IsPaused = false;

    private void Awake()
    {
        Instance = this;
        Darken.gameObject.SetActive(true);
        Darken.DOColor(Color.clear, .5f).From(Color.black).OnComplete(() =>
        {
            Darken.gameObject.SetActive(false);
        });
        
        PauseButton.onClick.AddListener(Pause);
        volumeSlider.onValueChanged.AddListener(HandleSliderChange);

        UnPause();
    }
    
    private void Start()
    {
        Refresh();
        UnPause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    private void UnPause()
    {
        IsPaused = false;

        Time.timeScale = 1;
        PauseMenu.gameObject.SetActive(false);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.gameObject.SetActive(true);
        PauseMenu.DOFade(1.0f, .25f).From(0).SetUpdate(true);
    }

    private void HandleSliderChange(float f) {
        if (f < .5f) {
            float finalVal = f * 2;
            finalVal = 1 - finalVal;
            finalVal *= -80;
            mixer.SetFloat("Volume", finalVal);
        }
        else {
            float finalVal = (f - .5f) * 2;
            finalVal *= 20;
            mixer.SetFloat("Volume", finalVal);
        }
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
