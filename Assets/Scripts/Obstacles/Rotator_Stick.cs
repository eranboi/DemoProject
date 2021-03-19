using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator_Stick : MonoBehaviour
{
  private Rigidbody colliderRb;
  private Vector3 collisionDirection;
  public float force;


  void FixedUpdate(){
    if(colliderRb){
      colliderRb.AddForce(transform.right * force, ForceMode.Impulse);
      colliderRb = null;
    }
  }

  void OnCollisionEnter(Collision collision){
    if(collision.collider.CompareTag("Agents"))    colliderRb = collision.collider.GetComponent<Rigidbody>();
    else if(collision.collider.CompareTag("Player")){
      StartCoroutine(collision.collider.GetComponent<PlayerMovementController>().SetStunned());
      colliderRb = collision.collider.GetComponent<Rigidbody>();
    }
  }


}
