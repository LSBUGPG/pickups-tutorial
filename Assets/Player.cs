using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public Rigidbody body;
    Vector3 move;

    void Start()
    {
        Application.targetFrameRate = 30;
    }

    void Update()
    {
        move = Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        body.transform.Translate(move * speed * Time.deltaTime);
    }
}
