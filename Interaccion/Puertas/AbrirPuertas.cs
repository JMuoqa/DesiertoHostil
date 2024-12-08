using Scripts.Entidades;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPuertas : MonoBehaviour
{
    public LayerMask mascaraDeLaPuerta;
    public Transform camara;
    private float distanciaDelRayo = 22.5f;
    private Puertas estadoPuerta;
    private Animator animacion;
    private EstadoDePuerta estado;
    private bool animacionEnCurso = false;
    void Update()
    {
        // Para saber cuando la animación terminó y así resetear el flag animacionEnCurso.
        if (animacion != null)
        {
            // Verifica si la animación ha terminado
            if (animacion.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && animacionEnCurso)
            {
                animacionEnCurso = false;
            }
        }
        LanzarRayo();
    }
    private void RotarPuerta()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (estadoPuerta != null && animacion != null && !animacionEnCurso)
            {
                animacionEnCurso = true;
                if (!estadoPuerta.PuertaAbierta)
                {
                    animacion.SetTrigger("AbriendoPuerta");
                    estadoPuerta.PuertaAbierta = true;
                }
                else
                {
                    animacion.SetTrigger("CerrandoPuerta");
                    estadoPuerta.PuertaAbierta = false;
                }
            }
        }
    }

    private void LanzarRayo()
    {
        Vector3 origen = camara.position;
        Vector3 direccion = camara.forward;
        Debug.DrawRay(origen, direccion * distanciaDelRayo, Color.red);

        // Lanzamos un rayo en la dirección hacia donde está mirando la cámara
        if (Physics.Raycast(origen, direccion, out RaycastHit hitInfo, distanciaDelRayo))
        {
            LayerMask mascaraDetectada = hitInfo.collider.gameObject.layer;
            GameObject puertaDetectada = hitInfo.collider.gameObject;

            // Verificamos si el rayo tocó una puerta
            if ((mascaraDeLaPuerta & (1 << mascaraDetectada)) != 0)
            {
                // Obtenemos el estado y el Animator de la puerta
                estado = puertaDetectada.GetComponent<EstadoDePuerta>();
                animacion = puertaDetectada.GetComponent<Animator>();

                if (estado != null && animacion != null)
                {
                    // Asignamos el estado de la puerta y el Animator
                    estadoPuerta = estado.puertas;
                    RotarPuerta(); // Llamamos a RotarPuerta solo si todo está correcto
                }
            }
        }
    }
}