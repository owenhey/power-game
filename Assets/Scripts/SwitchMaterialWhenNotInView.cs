using System;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterialWhenNotInView : MonoBehaviour
{
    private Camera _camera;

    public LayerMask hitMask;
    public List<MeshRenderer> meshRends;

    public Material VisibleMat;
    public Material BehindMat;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 towardsMe = transform.position - _camera.transform.position;
        if (Physics.Raycast(_camera.transform.position, towardsMe, out RaycastHit hit, 300, hitMask))
        {
            foreach (var meshre in meshRends)
            {
                meshre.material = BehindMat;
            }
        }
        else
        {
            foreach (var meshre in meshRends)
            {
                meshre.material = VisibleMat;
            }
        }
    }
}
