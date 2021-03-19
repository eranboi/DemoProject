using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallPaintController : MonoBehaviour
{
  public LayerMask whatIsWall, whatIsPaint;
  private Vector3 mousePos;
  public GameObject redPaint;
  public GameObject redPaint_Instantiated;
  public Camera cam;

  public RenderTexture paintedWallRT;
  public Texture2D paintedWallTexture;
  private float calculatePercentageTimer;


  Ray screenToPointRay;
  RaycastHit rayWall, rayPaint;
  public enum STATUS { DISABLED, ACTIVATED };
  public STATUS currentStatus;

  public static PlayerWallPaintController Instance;

  public static Action ObjectiveDone;
  public static Action WallPaintDoneCompletely;


  void Start()
  {
   //Singleton
    if (Instance != null && Instance != this)
      Destroy(this.gameObject);
    else
      Instance = this;


    currentStatus = STATUS.DISABLED;

    //cam = Camera.main;

    paintedWallTexture = new Texture2D(288, 288, TextureFormat.RGB24, false);
    
  }

  void Update()
  {
    if(currentStatus == STATUS.ACTIVATED){
      if(Input.GetMouseButton(0)){
        GetInputs();
        PaintTheWall();
     }
    }
  }

  void GetInputs(){
    //Check if there is a mouse button input.
    

      mousePos = Input.mousePosition;
      
      screenToPointRay = cam.ScreenPointToRay(mousePos);
  }


  void PaintTheWall(){
    //Raycast to see if there is a wall
      Physics.Raycast(screenToPointRay, out rayWall, 50, whatIsWall);
      
      //or paint
      Physics.Raycast(screenToPointRay, out rayPaint, 50, whatIsPaint);

    //if ray hits a paint, simply return to not over-do it.
     if(rayPaint.collider) return;

     //if ray hits the wall, instantiate a red object to fake a painting.
    else if(rayWall.collider){
      redPaint_Instantiated = Instantiate(redPaint, rayWall.point, Quaternion.identity);

      if (calculatePercentageTimer > .2f)
      {
        renderTextureToTexture2D();
        CalculateThePercentage();
        calculatePercentageTimer = 0;
      }
      else
      {
        calculatePercentageTimer += Time.deltaTime;
      }
    }
  }

  public void SetStatus(bool activate){
    if(activate) currentStatus = STATUS.ACTIVATED;
    else currentStatus = STATUS.DISABLED;
  } 


  void CalculateThePercentage(){

    int m_width = paintedWallTexture.width;
    int m_height = paintedWallTexture.height;

    float totalAmountOfRedPixels = 0;

    for (int i = 0; i < m_width + 1; i ++){
       for (int j = 0; j < m_height + 1; j ++){
        if(paintedWallTexture.GetPixel(i, j) == Color.red) totalAmountOfRedPixels++;
      }
    }

    float percentage = (totalAmountOfRedPixels / (m_width * m_height)) * 100;

    GameManager.Instance.SetPercentageUI(percentage);

    if(Mathf.Round(percentage) > 80){
      ObjectiveDone?.Invoke();
    }

     if(Mathf.Round(percentage) > 99){
      WallPaintDoneCompletely?.Invoke();
    }


  }
  
  void renderTextureToTexture2D(){
    RenderTexture.active = paintedWallRT;

    paintedWallTexture.ReadPixels(new Rect(0, 0, paintedWallRT.width, paintedWallRT.height), 0, 0);

    paintedWallTexture.Apply();

  }
  
  
  
  /* private void OnDrawGizmos(){
    Gizmos.DrawRay(screenToPointRay);

  } */
}
