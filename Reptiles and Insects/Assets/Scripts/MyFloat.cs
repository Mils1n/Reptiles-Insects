using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFloat : MonoBehaviour
{
  private Rigidbody rb;
  private float floatUpSpeedLimit = 1.15f;
  public float floatUpSpeed = 1f;

  private void Start()
{
    rb = GetComponent<Rigidbody>();
}
private void OnTriggerStay(Collider other)
{
    if(other.gameObject.layer == 4)
    {
        float difference = (other.transform.position.y - transform.position.y) * floatUpSpeed;
        GetComponent<Rigidbody>().AddForce(new Vector3(0f, Mathf.Clamp((Mathf.Abs(Physics.gravity.y) * difference), 0, Mathf.Abs(Physics.gravity.y) * floatUpSpeedLimit), 0f), ForceMode.Acceleration); 
        
        GetComponent<Rigidbody>().drag = 0.99f;
        GetComponent<Rigidbody>().angularDrag = 0.8f;
    }
    if(other.gameObject.layer == 4)
    {
        GetComponent<Rigidbody>().drag = 0f;
        GetComponent<Rigidbody>().angularDrag = 0f;
    }
} 

}