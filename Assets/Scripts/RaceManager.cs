using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public CheckPoints[] allCheckPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < allCheckPoints.Length; i++)
        {
            allCheckPoints[i].cpNumber = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
