using System;
using UnityEngine;

public class PowerableBuildings : MonoBehaviour {
    [SerializeField] private MeshRenderer meshRender;
    [Range(0, 100)] private float powerLevel = 100;
    public float PowerLevel => powerLevel;

    private static readonly int ShaderPowerLevel = Shader.PropertyToID("PowerLevel");

    public void SetPowerLevel(float _powerLevel) {
        powerLevel = _powerLevel;
        meshRender.material.SetFloat(ShaderPowerLevel, _powerLevel);
    }
}
