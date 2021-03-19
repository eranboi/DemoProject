using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

  public GameObject raceCM, wallPaintCM, victoryCM;
  public static bool gameStarted = false;
  public static Action PlayerWon, PlayerLost;

  #region General UI
  public GameObject PlayerWon_UI;
  public GameObject PlayerLost_UI;
  public GameObject Starting_UI;

  public Text PlaceRank;


  #endregion


  #region Wall Painting 
  [SerializeField] private GameObject WallPaintingUI;
  [SerializeField] private Image percentageImage_UI;

  #endregion


  public static GameManager Instance;
  void Start()
  {
    if (Instance != null && Instance != this)
      Destroy(this.gameObject);
    else
      Instance = this;

    //Subscribe to the events
    RaceController.isPlayerWon += RaceFinished;
    PlayerMovementController.PlayerHitObstacles += RaceFinished;
    PlayerWallPaintController.WallPaintDoneCompletely += PaintingFinished;


    gameStarted = false;

    StartCoroutine(CheckPlaceRank());
    StartCoroutine(MakeStarterInteractable());
  }


  
  void RaceFinished(bool didPlayerWin){
    if(didPlayerWin){
      //TODO: Change to wall painting mode
      ChangeGameModeToPaint();
      PlayerWon?.Invoke();
    }else{

      PlayerLost?.Invoke();
      PlayerLost_UI.SetActive(true);
    }

    RaceController.isPlayerWon -= RaceFinished;
    PlayerMovementController.PlayerHitObstacles -= RaceFinished;


  }

  void PaintingFinished(){
    victoryCM.SetActive(true);
    WallPaintingUI.SetActive(false);
    PlayerWon_UI.SetActive(true);

    PlayerWallPaintController.WallPaintDoneCompletely -= PaintingFinished;

  }
  
  void ChangeGameModeToPaint(){
    PlayerMovementController.Instance.SetStatus(false);
    PlayerWallPaintController.Instance.SetStatus(true);

    raceCM.SetActive(false);
    wallPaintCM.SetActive(true);

    WallPaintingUI.SetActive(true);

    //TODO: Set the camera and wall to paint
  }

  void ChangeGameModeToRace(){
    PlayerMovementController.Instance.SetStatus(true);
    PlayerWallPaintController.Instance.SetStatus(false);

    
  }

  IEnumerator CheckPlaceRank(){
    while(true){
      PlaceRank.text = PlaceRankCheck.Instance.currentRank.ToString();
      yield return new WaitForSeconds(.1f);
    }
  }

  public void RestartTheGame(){
    SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
  }
  
  public void StartTheGame(){
    gameStarted = true;
    Starting_UI.SetActive(false);
  }
  
  public void SetPercentageUI(float percentage){
    
    percentageImage_UI.rectTransform.sizeDelta = new Vector2(Mathf.Round(percentage) * 4, 100);

  }

  IEnumerator MakeStarterInteractable(){
    Starting_UI.SetActive(true);
    yield return new WaitForSeconds(1);
    Starting_UI.transform.GetChild(0).GetComponent<Button>().interactable = true;
    
  }
}
