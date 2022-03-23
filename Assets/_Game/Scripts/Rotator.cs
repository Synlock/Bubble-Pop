using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    Transform myTransform;
    [SerializeField] float speed = 25f;

    void Start()
    {
        myTransform = transform;
    }
    void Update()
    {
        myTransform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}
