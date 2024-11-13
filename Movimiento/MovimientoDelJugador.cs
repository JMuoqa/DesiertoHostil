using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovimientoDelJugador : MonoBehaviour
{
    public CharacterController jugadorM;
    private float velocidad = 1.4f;
    private float correr = 2.5f;
    //Gravedad
    private float gravedad = -9.81f;
    private Vector3 velocidadDeCaida;
    private float distancia = 0.1f;
    //Salto
    private float salto = 1f;
    public Transform chequearSuelo;
    private float radioDeLaEsfera = 0.4f;
    [SerializeField]
    private LayerMask mascaraDelSuelo;
    
    //Funcion para chequear si el radio de la esfera de los pies toca el suelo
    private bool EsSuelo()
    {
        return Physics.CheckSphere(chequearSuelo.position, radioDeLaEsfera, mascaraDelSuelo);
    }


    //Para chequear con que 'layer' choca
    private bool ChequearDistancia()
    {
        Vector3 origen = chequearSuelo.position;
        Vector3 direccion = Vector3.down;
        Debug.DrawRay(origen, direccion * distancia, Color.red);
        if(Physics.Raycast(origen, direccion, out RaycastHit hitInfo, distancia))
        {
            LayerMask colisionDeMascara = hitInfo.collider.gameObject.layer;
            if((mascaraDelSuelo & (1 << colisionDeMascara))!=0)
            {
                Debug.Log("hola");
                return true; 
            }
        }
        return false;
    }

    void Update()
    {
        //Para mover el jugador
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        //Para correr
        if (Input.GetKey(KeyCode.LeftShift)) correr = 4f;
        if (Input.GetKeyUp(KeyCode.LeftShift)) correr = 2.5f;
        if (Input.GetKey(KeyCode.LeftAlt)) correr = 1f;
        if (Input.GetKeyUp(KeyCode.LeftAlt)) correr = 2.5f;
        //Aplicar movimiento
        jugadorM.Move(move * velocidad * correr * Time.deltaTime);
        //Saltar
        if (Input.GetKeyDown(KeyCode.Space) && EsSuelo())
            velocidadDeCaida.y = Mathf.Sqrt(salto * -2f * gravedad);
        //Gravedad
        if (ChequearDistancia() && velocidadDeCaida.y < 0)
            velocidadDeCaida.y = -2f;
        else
        {
            velocidadDeCaida.y += gravedad * Time.deltaTime;
            if (velocidadDeCaida.y < -20f)
                velocidadDeCaida.y = -20f;
        }
        velocidadDeCaida.y += gravedad * Time.deltaTime;
        jugadorM.Move(velocidadDeCaida * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        if(chequearSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(chequearSuelo.position, radioDeLaEsfera);
        }
    }
}
