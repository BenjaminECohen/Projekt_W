using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Target : MonoBehaviour
{
    public Material hurtMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Bullet_Player")
        {
            this.GetComponent<MeshRenderer>().material = hurtMat;
        }
    }
}
