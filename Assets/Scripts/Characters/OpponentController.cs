using System;
using UnityEngine;
using UnityEngine.AI;

public class OpponentController : MonoBehaviour
{
  public NavMeshAgent agent;
  public Transform destinationPoint;
  private Vector3 startingPos, randomDestination;
  private bool  isAgentGetToFinalDestination, agentLost;
  private Animator animator;
  [SerializeField] private GameObject P_girl;

  public Action AgentDestroyed;
  
  void Awake(){
    destinationPoint = GameObject.Find("Objective").transform;
    if(destinationPoint)
      startingPos = transform.position;
  
    P_girl = this.gameObject;

  }
  
  void Start()
  {
    animator = GetComponent<Animator>();
    GameManager.PlayerWon += AgentLost;
  }

  void Update()
  {
    if(agent.velocity.magnitude > .5f && !animator.GetBool("moving")) animator.SetBool("moving", true);
    else if(agent.velocity.magnitude <.5f && animator.GetBool("moving")) animator.SetBool("moving", false);


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
