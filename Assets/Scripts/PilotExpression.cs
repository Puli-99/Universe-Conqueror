using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PilotExpression : MonoBehaviour
{
    [SerializeField] Image pilotImage;         // Referencia al componente Image del piloto
    [SerializeField] Sprite normalSprite;     // Sprite original del piloto
    [SerializeField] Sprite damagedSprite;    // Sprite del piloto mostrando da�o
    [SerializeField] float damageDuration = 0.5f; // Duraci�n del sprite de da�o en segundos

    private bool isDamaged = false; // Controla si el efecto de da�o ya est� en ejecuci�n

    // M�todo para activar el efecto de da�o
    public void ShowDamage()
    {
        Debug.Log("Image should change");
        if (!isDamaged) // Solo inicia el efecto si no est� en curso
        {
            StartCoroutine(DamageEffect());
        }
    }

    // Corutina que controla el cambio de im�genes
    private IEnumerator DamageEffect()
    {
        isDamaged = true; // Marca que el efecto de da�o est� en curso

        // Cambia al sprite de da�o
        pilotImage.sprite = damagedSprite;

        // Espera el tiempo especificado
        yield return new WaitForSeconds(damageDuration);

        // Vuelve al sprite normal
        pilotImage.sprite = normalSprite;

        isDamaged = false; // Marca que el efecto de da�o ha terminado
    }
}
