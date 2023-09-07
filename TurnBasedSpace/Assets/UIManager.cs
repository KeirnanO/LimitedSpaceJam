using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextPopUp energyTextPopUpPrefab;

    public TextMeshProUGUI playerEnergyText;

    private void Awake()
    {
        instance = this;
    }

    public void CreateEnergyText(Vector2 position, int amount)
    {
        var go = Instantiate(energyTextPopUpPrefab);
        go.transform.position = position;
        go.energyText.SetText("+" + amount.ToString());
    }
}
