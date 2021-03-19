using System;
using UnityEngine;
using UnityEngine.AI;

public class OpponentController : MonoBehaviour
{
  // Start is called before the first frame update
  public NavMeshAgent agent;
  public Transform destinationPoint;
  private Vector3 startingPos, randomDestination;
  private bool pathCalculated;
  [SerializeField] private GameObject P_girl;

  private bool  isAgentGetToFinalDestination, agentLost;

  public Action AgentDestroyed;
  void Awake(){
    destinationPoint = GameObject.Find("Objective").transform;
    if(destinationPoint)
      //destinationPosition = new Vector3(transform.position.x, destinationPoint.position.y, destinationPoint.position.z);
    startingPos = transform.position;
    P_girl = this.gameObject;

  }
  void Start()
  {
    if(GameManager.gameStarted)
      FindRandomPathBetweenAgentAndFinalDestination();

    GameManager.PlayerWon += AgentLost;
  }

  void Update()
  {

    if(agentLost || !GameManager.gameStarted){
      return;
    }else{
      if(isAgentGetToFinalDestination){
        return;
      }else{
        if(Vector3.Distance(transform.position, destinationPoint.position)<5){
        isAgentGetToFinalDestination = true;
        return;
      }
      else if(agent.pathStatus == NavMeshPathStatus.PathComplete  &&  agent.remainingDistance < 2)
        FindRandomPathBetweenAgentAndFinalDestination(); 
      }
    }
  }

  void StartFromBeginning(){
    Instantiate(P_girl, startingPos, Quaternion.identity);
    Destroy(gameObject);
  }

  void AgentLost(){
    if(agent){
      agent.ResetPath();
    }
    agentLost = true;
    GameManager.PlayerWon -= AgentLost;

  }
  
  void FindRandomPathBetweenAgentAndFinalDestination(){
    if(Vector3.Distance(transform.position, destinationPoint.position)<50){
      agent.SetDestination(destinationPoint.position);
    }else{
      float deltaX = destinationPoint.position.z - transform.position.z;

      float randomZPoint = UnityEngine.Random.Range(transform.position.z + 5, transform.position.z + deltaX);
      float randomXPoint = UnityEngine.Random.Range(-12, 12);

      randomDestination = new Vector3(randomXPoint, 0, randomZPoint);

      agent.SetDestination(randomDestination);

    }

  }

  private void OnCollisionEnter(Collision collision)
  {
    if(collision.collider.CompareTag("Obstacle")){
      AgentDestroyed?.Invoke();
      StartFromBeginning();
    }
  }

  
}
