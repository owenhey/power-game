using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Transform cameraCenter;
    public float rotateSpeed = 10;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Buildings");
    }

    private void Update()
    {
        cameraCenter.transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }
}
