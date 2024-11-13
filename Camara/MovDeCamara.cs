using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovDeCamara : MonoBehaviour
{
    public float sensibilidadDeLaCamara = 80f;
    public Transform jugadorPrincipal;
    private float rotacionX = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadDeLaCamara * Time.deltaTime;        
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadDeLaCamara * Time.deltaTime;
        //Para el movimiento 'Y'
        jugadorPrincipal.Rotate(Vector3.up * mouseX);
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);
        //Para el movimiento 'X'
        transform.localRotation= Quaternion.Euler(rotacionX, 0, 0);
    }
}
