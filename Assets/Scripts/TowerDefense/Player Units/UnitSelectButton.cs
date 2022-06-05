using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSelectButton : MonoBehaviour
{
    public GameObject unitPrefab;
    public GameObject selection;
    public GameObject selectedUnitSprite;
    public GameObject buttonImage;
    public TextMeshProUGUI mpCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mpCost.text = unitPrefab.GetComponent<UnitBase>().MPCost.ToString();
    }

    public void UnitSelected()
    {
        if (!TD_GameManager.Instance.unitSelected)
        {
            TD_GameManager.Instance.placingUnit = true;
            TD_GameManager.Instance.selectedUnit = gameObject;
            selectedUnitSprite.SetActive(true);
            selection.SetActive(true);
            buttonImage.SetActive(false);
        }
    }

    public void Unselected()
    {
        selectedUnitSprite.SetActive(false);
        selection.SetActive(false);
        buttonImage.SetActive(true);
    }
}
