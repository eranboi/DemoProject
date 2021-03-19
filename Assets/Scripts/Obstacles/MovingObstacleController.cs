using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingObstacleController : MonoBehaviour
{
  [SerializeField] Transform[] wayPoints;
  private int wayPointIndex = 0;
  private Transform currentDestination;
  [SerializeField] private float moveSpeed;

  [SerializeField] private NavMeshObstacle navMeshObstacle;

  private int currentAgentsInDistance;


  void Start()
  {
    navMeshObstacle = GetComponent<NavMeshObstacle>();
    SetDestination();
    currentAgentsInDistance = 0;
  }

  void Update()
  {
    Move();
  }

  private void Move(){
    transform.position = Vector3.MoveTowards(transform.position, currentDestination.position, moveSpeed * Time.deltaTime);

    //Set a new destination if reached the current one.
    if((int) transform.position.x == (int) currentDestination.position.x) SetDestination();

  }

  private void SetDestination(){
    wayPointIndex++;
    currentDestination = wayPoints[wayPointIndex % 2];
  }

  private IEnumerator CheckDistance(){
    while(true){

      //TODO: Get the closest agent and check the distance
      // if it's close activate else disable the obstacle component.

      yield return new WaitForSeconds(.5f);
    }
  }

  public void OnTriggerEnter(Collider other){

    if(other.CompareTag("Agents")){
      currentAgentsInDistance++;
      ShouldActivateNavMeshObstacle(true);
    }
  }

  public void OnTriggerExit(Collider other){
    if(other.CompareTag("Agents"))
    {
      currentAgentsInDistance--;
    }
    
    if(currentAgentsInDistance == 0){
      ShouldActivateNavMeshObstacle(false);
    }
  }
  private void ShouldActivateNavMeshObstacle(bool activate){
    if(activate){
      if(navMeshObstacle.enabled) return;
      else navMeshObstacle.enabled = true;
    }else{

      if(!navMeshObstacle.enabled) return;
      else navMeshObstacle.enabled = false;
    }
  }
}
