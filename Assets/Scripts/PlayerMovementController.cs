using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour
{
  [SerializeField] private float moveSpeed;
  private bool isPlayerTouching, isPlayerLost, isPlayerStunned=false;
  private Vector3 inputPositionStart, inputPositionCurrent;
  private Animator animator;

  private Rigidbody rigidbody;
  public enum STATUS { DISABLED, ACTIVATED };
  public STATUS currentStatus;

  public static PlayerMovementController Instance;

  public GameObject cmRight, cmForward;

  void Start()
  {
    //Singleton
    if (Instance != null && Instance != this)
      Destroy(this.gameObject);
    else
      Instance = this;

    animator = GetComponent<Animator>();

    currentStatus = STATUS.ACTIVATED;
    if(!rigidbody) rigidbody = GetComponent<Rigidbody>();

    GameManager.PlayerWon += PlayerWon;
    GameManager.PlayerLost += PlayerLost;


  }

  void Update()
  {
    if(currentStatus == STATUS.ACTIVATED && !isPlayerLost && GameManager.gameStarted && !isPlayerStunned){
      GetInputs();

      CalculateAndRotate();
    }
  }

  void FixedUpdate(){
    if(currentStatus == STATUS.ACTIVATED && !isPlayerLost && GameManager.gameStarted && !isPlayerStunned)
      Move();

  }
  
  private void GetInputs(){
    //Check if there are any touches detected.
    
    if(Input.GetKeyDown(KeyCode.F))
      GetComponent<Rigidbody>().AddForce(Vector3.right * 5000, ForceMode.Impulse);

    isPlayerTouching = Input.GetMouseButton(0);

    if(Input.GetMouseButtonDown(0)){
      //Get the starting point of the input
      inputPositionStart = Input.mousePosition;
    }

    //Get the current location of the input if there are any
    if(isPlayerTouching){
      inputPositionCurrent = Input.mousePosition;
    }else{
      //StopEverything();
    }

  }

  private void Move(){
    if(isPlayerTouching && Vector2.Distance(inputPositionCurrent, inputPositionStart) > 30f){
      Vector3 moveVector = new Vector3(0,0,0);

      while(moveVector != transform.forward * moveSpeed) moveVector += (transform.forward * moveSpeed * Time.deltaTime);

      rigidbody.velocity = moveVector;

      animator.SetBool("moving", true);
    }
    else{
      animator.SetBool("moving", false);
      transform.position = transform.position;
    }
  }

  private void CalculateAndRotate(){

    if(isPlayerTouching){
      //Calculate the distance between starting point and current point of the touch.
      Vector3 deltaTouchPos = inputPositionCurrent - inputPositionStart;


      //Clamp the magnitude of the deltaVector so it creates a circular shape with r = 1.
      Vector3 clampedDeltaTouchPos = Vector3.ClampMagnitude(deltaTouchPos, 1);

      // Using the clampedDeltaTouchPos's tangent, calculate the angle which the player has to rotate to.
      //Mathf.Atan2 returns in Radiants so Rad2Deg converts it to angles.
      float shouldRotateTo = Mathf.Rad2Deg * Mathf.Atan2(clampedDeltaTouchPos.y, clampedDeltaTouchPos.x);


      //Set the rotation of the player object.
      transform.rotation =  Quaternion.Euler(0,  90 - shouldRotateTo, 0);

    }

  }

  private void PlayerWon(){
    if(!rigidbody) rigidbody = GetComponent<Rigidbody>();
    rigidbody.velocity = Vector3.zero;
    rigidbody.angularVelocity = Vector3.zero;
    animator.SetBool("moving", false);
    animator.SetBool("dancing", true);
    inputPositionCurrent = Vector3.zero;
    inputPositionStart = Vector3.zero;

        GameManager.PlayerWon -= PlayerWon;

  }
  
  private void PlayerLost(){
    if(!rigidbody) rigidbody = GetComponent<Rigidbody>();
    rigidbody.velocity = Vector3.zero;
    rigidbody.angularVelocity = Vector3.zero;
    animator.SetBool("moving", false);
    animator.SetBool("lost", true);
    inputPositionCurrent = Vector3.zero;
    inputPositionStart = Vector3.zero;
    isPlayerLost = true;

    GameManager.PlayerLost -= PlayerLost;
  }
  
  public void SetStatus(bool activate){
    if(activate) currentStatus = STATUS.ACTIVATED;
    else currentStatus = STATUS.DISABLED;

    //StopEverything();

  }

  public IEnumerator SetStunned(){
    isPlayerStunned = true;
    animator.SetBool("moving", false);
    yield return new WaitForSeconds(.5f);
    isPlayerStunned = false;
  }
}
