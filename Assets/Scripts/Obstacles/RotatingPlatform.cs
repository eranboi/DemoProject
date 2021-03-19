using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
  [SerializeField] private float angularSpeed;
  private Rigidbody rb;
  Vector3 angularVelocity;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    angularVelocity = new Vector3(0, 0, 5);
  }

  void FixedUpdate(){
    //transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);

    Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Time.fixedDeltaTime);
    rb.MoveRotation(rb.rotation * deltaRotation);

  }

  void OnCollisionEnter(Collision collision){
    Transform other;
    if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Agents")){
      other = collision.collider.transform;

      other.parent = this.transform;
    }
  }

  void OnCollisionExit(Collision collision){
     Transform other;
    if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Agents")){
      other = collision.collider.transform;

      other.parent = null;
    }
  }

 

}
