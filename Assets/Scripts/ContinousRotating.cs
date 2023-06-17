using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousRotating : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private Vector3 axis = Vector3.up;
    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
