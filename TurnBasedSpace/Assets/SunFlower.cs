using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Flower
{
    public int power;

    //Time in seconds for energy
    public float energyTime;
    private float energyTimeDelta;

    private void Start()
    {
        energyTimeDelta = Time.time + energyTime;
    }

    private void Update()
    {
        if(Time.time > energyTimeDelta)
        {
            GameManager.instance.IncreaseMana(power);
            UIManager.instance.CreateEnergyText(transform.position, power);

            energyTimeDelta = Time.time + energyTime;
        }
    }
}
