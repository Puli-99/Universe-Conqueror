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
    bool canShoot = true;

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
        HandleShootingInput();
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

    void HandleShootingInput()
    {
        if (Input.GetButton("Fire1") && canShoot) // Si el jugador presiona disparar y puede hacerlo
        {
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        canShoot = false; // Deshabilita disparar hasta que termine la corutina

        // Reproduce el sonido de disparo
        if (laserSfx != null)
        {
            laserSfx.Play();
        }

        // Activa las partículas
        if (laserL != null && laserR != null)
        {
            laserL.Play();
            laserR.Play();
        }

        // Espera a que termine el sonido del disparo
        if (laserSfx != null)
        {
            yield return new WaitForSeconds(laserSfx.clip.length);
        }

        // Detén las partículas después del sonido
        if (laserL != null && laserR != null)
        {
            laserL.Stop();
            laserR.Stop();
        }

        canShoot = true; // Vuelve a habilitar disparar
    }
}