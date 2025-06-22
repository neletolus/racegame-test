using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
  public Rigidbody theRB;
  public float maxSpeed;

  public float forwardAccel = 8f;
  public float reverseAccel = 4f;
  private float speedInput;

  public float turnStrength = 180f;
  private float turnInput;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    theRB.transform.parent = null;
  }

  // Update is called once per frame
  void Update()
  {
    speedInput = 0f;
    turnInput = 0f;

    // 新しいInput Systemを使用
    float verticalInput = 0f;
    float horizontalInput = 0f;

    if (Keyboard.current != null)
    {
      // 前進・後進の入力
      if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
      {
        verticalInput = 1f;
      }
      else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
      {
        verticalInput = -1f;
      }

      // 左右回転の入力
      if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
      {
        horizontalInput = -1f;
      }
      else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
      {
        horizontalInput = 1f;
      }
    }

    if (verticalInput > 0)
    {
      speedInput = verticalInput * forwardAccel;
    }
    else if (verticalInput < 0)
    {
      speedInput = verticalInput * reverseAccel;
    }
    turnInput = horizontalInput;

    transform.position = theRB.position;

    // 車が動いている時のみ回転を適用（より現実的な車の動作）
    if (Mathf.Abs(speedInput) > 0.1f)
    {
      transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * theRB.linearVelocity.magnitude / maxSpeed, 0f));
      theRB.rotation = transform.rotation; // Rigidbodyの回転も同期
    }
  }

  void FixedUpdate()
  {
    // 前進・後進の力を加える
    theRB.AddForce(transform.forward * speedInput * 1000f);

    // 最大速度の制限
    if (theRB.linearVelocity.magnitude > maxSpeed)
    {
      theRB.linearVelocity = theRB.linearVelocity.normalized * maxSpeed;
    }
  }
}
