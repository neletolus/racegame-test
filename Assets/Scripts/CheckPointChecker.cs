using UnityEngine;

public class CheckPointChecker : MonoBehaviour
{
    public CarController carController;
  private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //Debug.Log("Checkpoint reached: " + other.GetComponent<CheckPoints>().cpNumber);
            carController.CheckPointHit(other.GetComponent<CheckPoints>().cpNumber);
        }
    }
}
