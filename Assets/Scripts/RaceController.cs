using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{

  public static RaceController Instance;

  public static Action isRaceFinished;
  public static Action<bool> isPlayerWon;

  void Start()
  {
    if (Instance != null && Instance != this)
      Destroy(this.gameObject);
    else
      Instance = this;

  }

  void OnTriggerEnter(Collider other){
    if(other.CompareTag("Player")){
      isPlayerWon?.Invoke(true);
      isRaceFinished?.Invoke();
    }
    else if(other.CompareTag("Agents")){
      isPlayerWon?.Invoke(false);
      isRaceFinished?.Invoke();
    }
  }
}
