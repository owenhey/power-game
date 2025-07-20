using System;
using Assets.Scripts.Misc;
using DG.Tweening;
using UnityEngine;

public class PowerableBuildings : MonoBehaviour {
    [Header("References")]
    [SerializeField] private MeshRenderer meshRender;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip powerDownSound;
    [SerializeField] private GameObject poweredMapIndicator;
    
    [Header("Stats")]
    [Range(0, 100)] private float powerLevel = 100;
    [Min(0)] public int PowerRequired = 50;

    public bool PoweredOn = true;
        
    public float PowerLevel => powerLevel;
    
    private static readonly int ShaderPowerLevel = Shader.PropertyToID("_PowerLevel");

    private Tween powerShaderTween;

    public bool ForcePowerOff;

    private Material material {
        get {
            if (_material == null) {
                _material = new Material(meshRender.sharedMaterial);
                meshRender.material = _material;
            }
            return _material;
        }
    }
    private Material _material;


    public void OnTriggerEnter(Collider c) {
        if (c.CompareTag("Player"))
        {
            AttemptPowerUp();
        }
    }

    private void SetPowerLevel(float _powerLevel) {
        powerLevel = _powerLevel;
        material.SetFloat(ShaderPowerLevel, _powerLevel);
    }

    public void AttemptPowerUp()
    {
        if (PoweredOn) return;

        // For the tutorial
        if (ForcePowerOff)
        {
            PowerUp();
            return;
        }
        
        int collectedKilowatts = GameManager.Instance.Kilowatts;
        if (collectedKilowatts >= PowerRequired) {
            GameManager.Instance.Kilowatts -= PowerRequired;
            PowerUp();
        }
        else
        {
            MiscSounds.instance.PlayNotEnoughPower();
            UI.Instance.kwtext.DOShakeAnchorPos(.25f, 50, 15);
        }
            
        UI.Refresh();
    }
    
    public void PowerDown() {
        PoweredOn = false;
        powerShaderTween?.Kill();
        powerShaderTween = DOTween.To(() => PowerLevel, SetPowerLevel, 0, .5f);         
        UI.Refresh();

        audioSource.clip = powerDownSound;
        audioSource.Play();
        poweredMapIndicator.gameObject.SetActive(false);
        
        BuildingManager.Instance?.RecalculateLoseCondition();
    }

    private void PowerUp() {
        PoweredOn = true;
        powerShaderTween?.Kill();
        powerShaderTween = DOTween.To(() => PowerLevel, SetPowerLevel, 100, .5f);
        
        audioSource.clip = powerUpSound;
        audioSource.Play();
        
        poweredMapIndicator.gameObject.SetActive(true);
    }
}
