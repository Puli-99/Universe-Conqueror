using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PilotExpression : MonoBehaviour
{
    [SerializeField] Image pilotImage;         // Referencia al componente Image del piloto
    [SerializeField] Sprite normalSprite;     // Sprite original del piloto
    [SerializeField] Sprite damagedSprite;    // Sprite del piloto mostrando daño
    [SerializeField] float damageDuration = 0.5f; // Duración del sprite de daño en segundos

    private bool isDamaged = false; // Controla si el efecto de daño ya está en ejecución

    // Método para activar el efecto de daño
    public void ShowDamage()
    {
        Debug.Log("Image should change");
        if (!isDamaged) // Solo inicia el efecto si no está en curso
        {
            StartCoroutine(DamageEffect());
        }
    }

    // Corutina que controla el cambio de imágenes
    private IEnumerator DamageEffect()
    {
        isDamaged = true; // Marca que el efecto de daño está en curso

        // Cambia al sprite de daño
        pilotImage.sprite = damagedSprite;

        // Espera el tiempo especificado
        yield return new WaitForSeconds(damageDuration);

        // Vuelve al sprite normal
        pilotImage.sprite = normalSprite;

        isDamaged = false; // Marca que el efecto de daño ha terminado
    }
}
