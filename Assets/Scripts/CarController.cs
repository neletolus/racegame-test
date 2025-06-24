using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
  public Rigidbody theRB;
  public float maxSpeed;

  public float forwardAccel = 8f;
  public float reverseAccel = 4f;
  private float speedInput;

  public float turnStrength = 180f;
  private float turnInput;

  private bool grounded;

  public Transform groundRayPoint;
  public Transform groundRayPoint2;

  public LayerMask whatIsGround;
  public float groundRayLength = 0.75f;

  private float dragOnGround;
  public float gravityMod = 10f;

  public Transform leftFrontWheel;
  public Transform rightFrontWheel;
  public float maxWheelTurn = 25f;

  public ParticleSystem[] dustTrails;

  public float maxEmission = 25f;
  public float emissionFadeSpeed = 20f;

  private float emissionRate;

  public AudioSource engineSound;
  public AudioSource skidSound;
  public float skidFadeSpeed;

  private int nextCheckPoint = 0;
  
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    theRB.transform.parent = null;
    dragOnGround = theRB.linearDamping;
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

    // 車輪の回転
    leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
    rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

    // パーティクルのエミッション制御
    emissionRate = Mathf.MoveTowards(emissionRate, 0f, Time.deltaTime * emissionFadeSpeed);

    if (grounded && (Mathf.Abs(turnInput) > 0.5f || (theRB.linearVelocity.magnitude < maxSpeed * 0.5f && theRB.linearVelocity.magnitude != 0)))
    {
      emissionRate = maxEmission;
    }

    if (theRB.linearVelocity.magnitude <= 0.5f)
    {
      emissionRate = 0f;
    }

    foreach (var dustTrail in dustTrails)
    {
      var emission = dustTrail.emission;
      emission.rateOverTime = emissionRate;
    }

    if (engineSound != null)
    {
      engineSound.pitch = 1f + ((theRB.linearVelocity.magnitude / maxSpeed) * 2f);
    }

    if (skidSound != null)
    {
      if (Mathf.Abs(turnInput) > 0.5f)
      {
        skidSound.volume = 1f;
      }
      else
      {
        skidSound.volume = Mathf.MoveTowards(skidSound.volume, 0f, Time.deltaTime * skidFadeSpeed);
      }
    }
  }

  void FixedUpdate()
  {
    grounded = false;

    RaycastHit hit;
    Vector3 normalTarget = Vector3.zero;

    if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
    {
      grounded = true;
      normalTarget = hit.normal;
    }

    if (Physics.Raycast(groundRayPoint2.position, -transform.up, out hit, groundRayLength, whatIsGround))
    {
      grounded = true;
      normalTarget = (normalTarget + hit.normal) / 2f;
    }

    if (grounded)
    {
      transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;
    }

    if (grounded)
    {
      theRB.linearDamping = dragOnGround;
      theRB.AddForce(transform.forward * speedInput * 1000f);
    }
    else
    {
      theRB.linearDamping = 0.1f;
      theRB.AddForce(-Vector3.up * gravityMod * 100f);
    }

    // 最大速度の制限
    if (theRB.linearVelocity.magnitude > maxSpeed)
    {
      theRB.linearVelocity = theRB.linearVelocity.normalized * maxSpeed;
    }

    transform.position = theRB.position;

    // 車が動いている時のみ回転を適用（より現実的な車の動作）
    if (grounded && Mathf.Abs(speedInput) > 0.1f)
    {
      transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * theRB.linearVelocity.magnitude / maxSpeed, 0f));
      theRB.rotation = transform.rotation; // Rigidbodyの回転も同期
    }
  }

  public void CheckPointHit(int cpNumber)
  {
    if (cpNumber == nextCheckPoint)
    {
      nextCheckPoint++;
      if (nextCheckPoint > RaceManager.instance.allCheckPoints.Length - 1) // Assuming there are 4 checkpoints numbered 0 to 3
      {
        nextCheckPoint = 0; // Reset to the first checkpoint after completing all
      }
    }
  }
}
