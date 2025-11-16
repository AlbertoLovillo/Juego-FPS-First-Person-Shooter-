using System;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GestionaMovimiento();
        GestionaRotacion();
        GestionaSalto();
    }


    [Header("Movimiento")]
    private Vector2 inputMovimiento;
    private Vector3 movimientoActual;
    public CharacterController characterController;
    private float velocidad = 7f;

    private void OnMove(InputValue teclasAWSDPulsadas)
    {
        inputMovimiento = teclasAWSDPulsadas.Get<Vector2>();
    }

    private void GestionaMovimiento()
    {
        Vector3 direccionMundo = CalculaDireccionMundo();
        movimientoActual.x = direccionMundo.x * velocidad;
        movimientoActual.z = direccionMundo.z * velocidad;
        // characterController.Move(movimientoActual * Time.deltaTime);
    }

    
    [Header("Vista")]
    private Vector2 inputVista;
    public Camera camera;
    private float upDownVistaRango = 50f;
    private float rotacionVertical;
    private float sensibilidadRaton = 0.4f;

    private void OnLook(InputValue direccionRaton)
    {
        inputVista = direccionRaton.Get<Vector2>();
    }

    private void GestionaRotacion()
    {
        float rotacionRatonX = inputVista.x * sensibilidadRaton;
        float rotacionRatonY = inputVista.y * sensibilidadRaton;

        AplicaRotacionHorizontal(rotacionRatonX);
        AplicaRotacionVertical(rotacionRatonY);
    }

    private void AplicaRotacionHorizontal(float valorRotacion)
    {
        transform.Rotate(0, valorRotacion, 0);
    }

    private void AplicaRotacionVertical(float valorRotacion)
    {
        rotacionVertical = Mathf.Clamp(rotacionVertical - valorRotacion, -upDownVistaRango, upDownVistaRango);
        camera.transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);
    }

    private Vector3 CalculaDireccionMundo()
    {
        Vector3 inputDirection = new Vector3(inputMovimiento.x, 0, inputMovimiento.y);
        Vector3 direccionMundo = transform.TransformDirection(inputDirection);
        return direccionMundo.normalized;
    }

    
    [Header("Salto")]
    private bool inputJump = false;
    private bool estaEnSuelo = true;
    private float fuerzaSalto = 3f;
    private float gravedad = -9.8f;

    private void OnJump(InputValue inputValue)
    {
        if(characterController.velocity.y == 0)
        {
            inputJump = true;
        }
    }

    private void GestionaSalto()
    {
        estaEnSuelo = characterController.isGrounded;
        
        if(estaEnSuelo)
        {
            movimientoActual.y = 0f;
        }

        if(inputJump && estaEnSuelo)
        {
            movimientoActual.y += MathF.Sqrt(fuerzaSalto * -1f * gravedad);
            inputJump = false;
        }

        movimientoActual.y += gravedad * Time.deltaTime;
        characterController.Move(movimientoActual * Time.deltaTime);
    }


    [Header("Disparo")]
    public Transform puntoDisparo;
    public GameObject bala;
    public float fuerzaDisparo = 1500f;
    public float tasaDisparo = 0.5f;
    private float tiempoEntreDisparos = 0;

    private void OnAttack(InputValue inputValue)
    {
        if (Time.time > tiempoEntreDisparos)
        {
            GameObject nuevaBala;
            nuevaBala = Instantiate(bala, puntoDisparo.position, puntoDisparo.rotation);
            nuevaBala.transform.Rotate(90, 0, 0);
            nuevaBala.GetComponent<Rigidbody>().AddForce(puntoDisparo.forward * fuerzaDisparo);

            tiempoEntreDisparos = Time.time + tasaDisparo;
            Destroy(nuevaBala, 2);
        }
    }
}
