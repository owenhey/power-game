using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Button BackButton;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button NextButton;
    [SerializeField] private Image Darkener;

    private int tutPhase = 0;
    public GameObject tut1;
    public GameObject tut2;
    public GameObject tut3;

    private void Awake()
    {
        Darkener.gameObject.SetActive(true);
        Darkener.DOColor(Color.clear, .5f).From(Color.black).OnComplete(() =>
        {
            Darkener.gameObject.SetActive(false);
        });
        
        BackButton.onClick.AddListener(Back);
        PlayButton.onClick.AddListener(Play);
        NextButton.onClick.AddListener(Next);
        
        UpdateTut();
    }

    private void Play()
    {
        Darkener.gameObject.SetActive(true);
        Darkener.DOColor(Color.black, .5f).From(Color.clear).OnComplete(() =>
        {
            SceneManager.LoadScene("Roads");
        });
    }

    private void Back()
    {
        Darkener.gameObject.SetActive(true);
        Darkener.DOColor(Color.black, .5f).From(Color.clear).OnComplete(() =>
        {
            SceneManager.LoadScene("Title");
        });
    }

    private void Next()
    {
        tutPhase += 1;
        UpdateTut();
    }

    private void UpdateTut()
    {
        tut1.gameObject.SetActive(tutPhase == 0);
        tut2.gameObject.SetActive(tutPhase == 1);
        tut3.gameObject.SetActive(tutPhase == 2);
    }
}
