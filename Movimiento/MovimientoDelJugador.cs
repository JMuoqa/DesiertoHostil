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
    //Audios
    private AudioSource audioDeMovimiento;
    public AudioSource reproducirCaminar;
    public AudioSource reproducirCorrer;

    private void Start()
    {
        audioDeMovimiento = reproducirCaminar;
    }

    void Update()
    {

        MoverJugador();
        Saltar();
    }


    private void MoverJugador()
    {
        //Para mover el jugador
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        //Para correr


        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            ChangeAudio(reproducirCorrer);
            correr = 4f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            ChangeAudio(reproducirCaminar);
            correr = 2.5f; 
        }
        //Aplicar movimiento
        jugadorM.Move(move * velocidad * correr * Time.deltaTime);
        ReproducirAudio(move);
    }
    private void Saltar()
    {
        //Saltar
        if (Input.GetKeyDown(KeyCode.Space) && EsSuelo())
        {
            if (audioDeMovimiento.isPlaying)
                audioDeMovimiento.Stop();
            velocidadDeCaida.y = Mathf.Sqrt(salto * -2f * gravedad);
        }
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

    private void ReproducirAudio(Vector3 move)
    {
        if ((move.x != 0 || move.z != 0) && !audioDeMovimiento.isPlaying)
        {
            audioDeMovimiento.Play();
        }
        else if ((move.x == 0 || move.z == 0) && audioDeMovimiento.isPlaying)
            audioDeMovimiento.Stop();
    }

    private void ChangeAudio(AudioSource audio)
    {
        if(audioDeMovimiento != audio)
        {
            audioDeMovimiento = audio;
        }
    }

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
        if (Physics.Raycast(origen, direccion, out RaycastHit hitInfo, distancia))
        {
            LayerMask colisionDeMascara = hitInfo.collider.gameObject.layer;
            if ((mascaraDelSuelo & (1 << colisionDeMascara)) != 0)
            {
                return true;
            }
        }
        return false;
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
