using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
  public float angularSpeed;

  // Update is called once per frame
  void FixedUpdate()
    {
        transform.Rotate(Vector3.up, angularSpeed * Time.deltaTime);

    }
}
