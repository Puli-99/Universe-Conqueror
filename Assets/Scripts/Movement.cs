using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Basic settings")]
    [Tooltip("Ship Input speed")]
    [SerializeField] float controlSpeed;
    [Tooltip("Ship movement range across the camera")]
    [SerializeField] float xRange;
    [Tooltip("Ship movement range across the camera")]
    [SerializeField] float yRange;

    [Header("Rotation settings")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -15f;
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlRollFactor = -20f;
    [SerializeField] ParticleSystem laserR;
    [SerializeField] ParticleSystem laserL;
    [SerializeField] AudioSource laserSfx;
    [SerializeField] float fireCooldown = 0.25f;
    private float timeSinceLastShot = 0f;
    bool isShooting;

    [Header("Crosshair settings")]
    [SerializeField] GameObject crosshair; // Asumimos que el crosshair es un objeto en la escena
    [SerializeField] float crosshairDistance = 50f; // Distancia a la que queremos ubicar el crosshair

    float xThrow;
    float yThrow;
    void Start()
    {
        laserSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Throw();
        Rotation();
        FireInput();
    }

    void Rotation()
    {
        float pitchDuePosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueControl = yThrow * controlPitchFactor;
        float pitch = pitchDuePosition + pitchDueControl;

        float yawDuePosition = transform.localPosition.x * positionYawFactor;
        float yawDueControl = xThrow * controlPitchFactor;
        float yaw = yawDuePosition + yawDueControl;

        float rollDuePosition = transform.localPosition.x * controlRollFactor;
        //float rollDueControl = xThrow * controlPitchFactor;
        float roll = (-rollDuePosition) + yawDueControl;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void Throw()
    {
        float xThrow = Input.GetAxis("Horizontal");
        float yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float newXPos = xOffset + transform.localPosition.x;
        float clampedXPos = Mathf.Clamp(newXPos, -xRange, xRange);


        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float newYPos = yOffset + transform.localPosition.y;
        float clampedYPos = Mathf.Clamp(newYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void FireInput()
    {
        if (Input.GetButton("Fire1")) // Detecta si el jugador mantiene presionado el botón de disparo
        {
            if (!isShooting)
            {
                StartShooting();
            }
            else
            {
                HandleContinuousShooting();
            }
        }
        else if (Input.GetButtonUp("Fire1")) // Detecta cuando el jugador deja de disparar
        {
            StopShooting();
        }

        if (isShooting)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }
    void Firing()
    {// Reproduce el sonido de disparo
        if (laserSfx != null && !laserSfx.isPlaying)
        {
            laserSfx.loop = false; // Solo queremos un disparo individual
            laserSfx.Play();
        }

        // Activa las partículas
        if (laserL != null && laserR != null)
        {
            laserL.Play();
            laserR.Play();
        }
    }
    void StartShooting()
    {
        isShooting = true;
        timeSinceLastShot = fireCooldown; // Permite disparar inmediatamente al inicio

    }

    void StopShooting()
    {
        isShooting = false;

        // Detén el sistema de partículas (si está activo)
        if (laserR != null && laserR.isPlaying && laserL != null && laserL.isPlaying)
        {
            laserR.Stop();
            laserL.Stop();
        }
        if (laserSfx != null && laserSfx.isPlaying)
        {
            laserSfx.Stop();
        }
    }
    void HandleContinuousShooting()
    {
        // Comprueba si ha pasado suficiente tiempo desde el último disparo
        if (timeSinceLastShot >= fireCooldown)
        {
            Firing();
            timeSinceLastShot = 0f; // Reinicia el temporizador 
        }
    }    
}