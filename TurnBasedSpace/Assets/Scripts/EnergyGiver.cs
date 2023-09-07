using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyGiver : Entity
{
    public int power;

    //Time in seconds for energy
    public int turnDelay = 3;
    private int turnsLeft = 1;

    public TextMeshProUGUI turnText;

    //Gives energy the turn after its placed
    public bool haste = false;

    private void Start()
    {
        if(!haste)
            turnsLeft = turnDelay;


        turnText.SetText("Turns Left: " + turnsLeft);
    }

    public override void OnRoundStart()
    {
        turnsLeft -= 1;

        if(turnsLeft <= 0)
        {
            GameManager.instance.IncreaseMana(power);
            UIManager.instance.CreateEnergyText(transform.position, power);

            turnsLeft = turnDelay;
        }

        turnText.SetText("Turns Left: " + turnsLeft);
    }
}
