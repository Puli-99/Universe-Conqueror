using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PilotExpression : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image pilotImage;  // Referencia al componente Image del piloto
    [SerializeField] private Sprite normalSprite;   // Sprite original del piloto
    [SerializeField] private Sprite damagedSprite;  // Sprite de daño
    [SerializeField] private float damageDuration = 0.5f;  // Duración del sprite de daño
    [SerializeField] private float damageCooldown = 1f; // Tiempo de espera antes de restaurar el sprite normal después de recibir daño

    private bool isDamaged = false;  // Para evitar cambios rápidos de sprite

    private void Start()
    {
        // Asegurarse de que el piloto comience con el sprite normal
        pilotImage.sprite = normalSprite;
    }

    // Método para activar el efecto de daño
    public void ShowDamage()
    {
        if (!isDamaged)  // Asegurarse de que no se cambie constantemente
        {
            StartCoroutine(DamageEffect());
        }
    }

    // Corutina que cambia el sprite y lo restaura después de un tiempo
    private IEnumerator DamageEffect()
    {
        isDamaged = true;  // Evitar cambios rápidos de sprite

        // Cambiar al sprite de daño
        pilotImage.sprite = damagedSprite;

        // Esperar el tiempo de daño
        yield return new WaitForSeconds(damageDuration);

        // Volver al sprite normal
        pilotImage.sprite = normalSprite;

        // Esperar un tiempo sin recibir daño antes de permitir otro cambio
        yield return new WaitForSeconds(damageCooldown);

        isDamaged = false;  // Se puede volver a cambiar el sprite si recibe otro daño
    }
}