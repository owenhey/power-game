using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KilowattsText;
    [SerializeField] private Image Darken;
    [SerializeField] private TextMeshProUGUI BuildingsDown;
    [SerializeField] private TextMeshProUGUI TimePassed;
    [SerializeField] private TextMeshProUGUI MainText;
    [SerializeField] private TextMeshProUGUI SecondText;
    [SerializeField] private TextMeshProUGUI TotalKW;
    [SerializeField] private CarController Player1;
    [SerializeField] private CarController Player2;
    [SerializeField] private TextMeshProUGUI Player1TotalKW;
    [SerializeField] private TextMeshProUGUI Player1TotalParcels;
    [SerializeField] private TextMeshProUGUI Player2TotalKW;
    [SerializeField] private TextMeshProUGUI Player2TotalParcels;
    [SerializeField] private GameObject WonThings;
    [SerializeField] private CanvasGroup PauseMenu;
    [SerializeField] private CanvasGroup EndGame;
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button EndGameButton;
    [SerializeField] private Button QuitButton;
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
        
        EndGameButton.onClick.AddListener(QuitGame);
        PlayButton.onClick.AddListener(UnPause);
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        EndGame.gameObject.SetActive(false);
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

    private void Pause() {
        IsPaused = true;
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
        BuildingsDown.text = $"{buildingsDown} building{(buildingsDown == 1 ? "" : "s")}";
        
        TimePassed.text = $"{FloatToTimeString(GameManager.Instance.TimeSinceGameBegan)}";
    }
    
    public static string FloatToTimeString(float totalSeconds)
    {
        int minutes = (int)(totalSeconds / 60);
        int seconds = (int)(totalSeconds % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public static void ShowEndGame() {
        Instance.InstanceShowEndGame();
    }

    private void QuitGame() {
        Darken.gameObject.SetActive(true);
        Darken.DOColor(Color.black, .5f).From(Color.clear).SetUpdate(true).OnComplete(() =>
        {
            SceneManager.LoadScene("Title");
        });
    }

    private void InstanceShowEndGame() {
        MainText.text = GameManager.Lost ? "Game Over!" : "VICTORY!";
        SecondText.text = GameManager.Lost ? "Over a third of the buildings in Kinetic City lost power!" : "You saved Kinetic City from a POWER DISASTER";
        WonThings.gameObject.SetActive(GameManager.Lost == false);

        TotalKW.text = (Player1.totalPowerCollected + Player2.totalPowerCollected).ToString() + "KW";

        Player1TotalKW.text = Player1.totalPowerCollected.ToString() + "KW";
        Player1TotalParcels.text = Player1.parcelsCollected.ToString();
        Player2TotalKW.text = Player2.totalPowerCollected.ToString() + "KW";
        Player2TotalParcels.text = Player2.parcelsCollected.ToString();

        EndGame.gameObject.SetActive(true);
        EndGame.DOFade(1.0f, 25f);
    }
}
