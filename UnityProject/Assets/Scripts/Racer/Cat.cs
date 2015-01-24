using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

    public Animator catAnimation;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lismor")
        {
            catAnimation.SetBool("Scared", true);
        }
    }
}
