using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _horizontalSpeed = 1.5f;

    [SerializeField]
    private float _verticalSpeed = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //set Player position on start 
        transform.position = new Vector3(0f,-2f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        //side to side 

        //up down

        Vector3 translateVector = new Vector3(Vector3.left.x * _horizontalSpeed,Vector3.up.y * _verticalSpeed,0);
        transform.Translate(translateVector * Time.deltaTime);
    }
}
