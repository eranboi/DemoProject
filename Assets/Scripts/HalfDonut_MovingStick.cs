using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonut_MovingStick : MonoBehaviour
{

  [SerializeField] private float angularSpeed;
  [SerializeField] private float force;
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame

    
  void FixedUpdate(){
    transform.Rotate(Vector3.right, angularSpeed * Time.deltaTime);
  }

  void OnCollisionEnter(Collision collision){
    if(collision.collider.CompareTag("Agents") || collision.collider.CompareTag("Player")){
      collision.collider.GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
    }
  }
    
}
