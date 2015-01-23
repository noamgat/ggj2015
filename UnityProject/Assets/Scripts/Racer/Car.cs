using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

    public class ExampleClass : MonoBehaviour
    {
        public Transform explosionPrefab;
        public RacerManager manager;

        void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(explosionPrefab, pos, rot);
            Destroy(collision.gameObject);
            
        }
    }
}
