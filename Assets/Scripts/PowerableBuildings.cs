using System;
using DG.Tweening;
using UnityEngine;

public class PowerableBuildings : MonoBehaviour {
    [SerializeField] private MeshRenderer meshRender;
    [Range(0, 100)] private float powerLevel = 100;
    public float PowerLevel => powerLevel;

    private static readonly int ShaderPowerLevel = Shader.PropertyToID("_PowerLevel");

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


    public void SetPowerLevel(float _powerLevel) {
        powerLevel = _powerLevel;
        material.SetFloat(ShaderPowerLevel, _powerLevel);
    }

    public void PowerDown() {
        DOTween.To(() => PowerLevel, SetPowerLevel, 0, .5f);
    }
}
