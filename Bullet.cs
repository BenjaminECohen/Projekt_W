using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("bulletLife");
    }

    private IEnumerator bulletLife()
    {
        yield return new WaitForSeconds(2.0f);
        if (this.gameObject)
        {
            Destroy(this.gameObject);
        }
        yield break;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);

    }
}
