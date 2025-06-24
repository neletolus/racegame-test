using UnityEngine;

public class CheckPointChecker : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if(other.tag == "CheckPoint")
    {
      Debug.Log("Checkpoint reached: " + other.GetComponent<CheckPoints>().cpNumber);
    }
  }
}
