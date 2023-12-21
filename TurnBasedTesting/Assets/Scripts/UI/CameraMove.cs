using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{

    public int cameraMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse2)|| Input.GetKey(KeyCode.Mouse1))
        {
            transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraMoveSpeed, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraMoveSpeed, 0f);
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, .2f * Time.deltaTime * cameraMoveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, .2f * Time.deltaTime * cameraMoveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(.2f * Time.deltaTime * cameraMoveSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(.2f * Time.deltaTime * cameraMoveSpeed, 0 , 0);
        }
    }
}
