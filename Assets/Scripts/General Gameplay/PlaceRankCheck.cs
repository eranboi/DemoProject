using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRankCheck : MonoBehaviour
{

  private List<GameObject> agentsFront;
  public int currentRank = 1;

  public static PlaceRankCheck Instance;

  public Transform playerTransform;

  void Start()
  {
    if (Instance != null && Instance != this)
      Destroy(this.gameObject);
    else
      Instance = this;

    currentRank = 1;

  }

    // Update is called once per frame
    void Update()
    {
    transform.position = new Vector3(0, 0, playerTransform.position.z + 150);
  }

  void OnTriggerEnter(Collider other){
    if(other.CompareTag("Agents")){
      currentRank++;
      other.transform.GetComponent<OpponentController>().AgentDestroyed += AgentDestroyed;
    }
  }

  void OnTriggerExit(Collider other){
    if(other.CompareTag("Agents")){
      currentRank--;
      other.transform.GetComponent<OpponentController>().AgentDestroyed -= AgentDestroyed;

    }
  }

  void AgentDestroyed(){
    currentRank--;
  }
}
