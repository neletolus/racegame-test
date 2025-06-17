using UnityEngine;

public class GameController : MonoBehaviour
{
  public Rigidbody theRB;
  public float maxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
    {
        theRB.transform.parent = null;
    }

  // Update is called once per frame
  void Update()
  {
    theRB.AddForce(new Vector3(0f, 0f, 100f));
    transform.position = theRB.position;
    }
}
