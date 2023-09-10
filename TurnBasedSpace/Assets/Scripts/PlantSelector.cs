using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantSelector : MonoBehaviour
{
    public PlaceableScriptableObject plantObject;

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public string unitName;


    private void Start()
    {
        icon.sprite = plantObject.icon;
        nameText.SetText(plantObject.Name);
        costText.SetText(plantObject.cost + " Energy");
    }

    private void OnMouseDown()
    {
        GameManager.instance.SelectPlant(plantObject);
        Grid.instance.previewName = unitName;
    }
}
