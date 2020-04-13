using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVectors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 3, Color.red);
        Debug.DrawRay(transform.position, transform.forward * -3, Color.green);
        //Debug.Log(this.gameObject.name + ": Vector forward " + transform.TransformVector(transform.forward));
        //Debug.Log(this.gameObject.name + ": Vector forward * -1 " + transform.TransformVector(transform.forward * -1));
    }
}
