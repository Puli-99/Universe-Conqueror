using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PilotExpression : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image pilotImage;  // Referencia al componente Image del piloto
    [SerializeField] private Sprite normalSprite;   // Sprite original del piloto
    [SerializeField] private Sprite damagedSprite;  // Sprite de da�o
    [SerializeField] private float damageDuration = 0.5f;  // Duraci�n del sprite de da�o
    [SerializeField] private float damageCooldown = 1f; // Tiempo de espera antes de restaurar el sprite normal despu�s de recibir da�o

    private bool isDamaged = false;  // Para evitar cambios r�pidos de sprite

    private void Start()
    {
        // Asegurarse de que el piloto comience con el sprite normal
        pilotImage.sprite = normalSprite;
    }

    // M�todo para activar el efecto de da�o
    public void ShowDamage()
    {
        if (!isDamaged)  // Asegurarse de que no se cambie constantemente
        {
            StartCoroutine(DamageEffect());
        }
    }

    // Corutina que cambia el sprite y lo restaura despu�s de un tiempo
    private IEnumerator DamageEffect()
    {
        isDamaged = true;  // Evitar cambios r�pidos de sprite

        // Cambiar al sprite de da�o
        pilotImage.sprite = damagedSprite;

        // Esperar el tiempo de da�o
        yield return new WaitForSeconds(damageDuration);

        // Volver al sprite normal
        pilotImage.sprite = normalSprite;

        // Esperar un tiempo sin recibir da�o antes de permitir otro cambio
        yield return new WaitForSeconds(damageCooldown);

        isDamaged = false;  // Se puede volver a cambiar el sprite si recibe otro da�o
    }
}