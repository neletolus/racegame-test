using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwicher : MonoBehaviour
{
    public GameObject[] cameras;
    private int currentCam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the first camera as active
        if (cameras.Length > 0)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(i == currentCam);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            currentCam++;
            if (currentCam >= cameras.Length)
            {
                currentCam = 0;
            }

            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(i == currentCam);
            }
        }
    }
}
