using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private float speed;
    // Update is called once per frame
    void Update()
    { // goal rotate
        transform.Rotate(Vector3.forward * speed);
    }
}
