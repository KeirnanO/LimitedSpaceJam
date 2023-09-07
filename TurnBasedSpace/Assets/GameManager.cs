using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int mana;
    
    private void Awake()
    {
        instance = this;
    }

    public void IncreaseMana(int amount)
    {
        mana += amount;

        UIManager.instance.playerEnergyText.SetText("Energy: " + mana.ToString());
    }

}
