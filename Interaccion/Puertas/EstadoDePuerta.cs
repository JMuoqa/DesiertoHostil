using UnityEngine;
using Scripts.Entidades;

public class EstadoDePuerta : MonoBehaviour
{
    public bool estadoDeLaPuerta = false;
    public Puertas puertas = new Puertas();

    private void Start()
    {
        puertas.PuertaAbierta = estadoDeLaPuerta;
    }

}