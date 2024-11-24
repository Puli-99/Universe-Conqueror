using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBoard : MonoBehaviour
{
    int health = 100;
    TMP_Text healthText;

    void Start()
    {
        healthText = GetComponent<TMP_Text>();
    }

    public void DecreaseHealth(int amountToDecrease)
    {
        health -= amountToDecrease;
        healthText.text = health.ToString();
    }
}
