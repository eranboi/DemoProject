using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
  [SerializeField] private float angularSpeed;

  void Start()
  {
    //GetComponent<Rigidbody>().AddTorque(transform.forward * angularSpeed, ForceMode.Acceleration);
  }

  void FixedUpdate(){
    transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
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
