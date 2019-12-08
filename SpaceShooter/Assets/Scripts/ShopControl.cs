using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopControl : MonoBehaviour
{
    int units;
    int isSold;

    public TextMeshProUGUI unitsText;
    public TextMeshProUGUI weaponText;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject shop2;
    [SerializeField] private List<GameObject> upgradeShopUI;
    [SerializeField] private List<GameObject> shipShopUI;
    [SerializeField] private List<GameObject> itemDescriptions;

    public Button buy;

    // Start is called before the first frame update
    void Start()
    {
        units = playerStats.GetUnits();

        foreach (var obj in shipShopUI)
            obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        unitsText.text = "Units: " + playerStats.GetUnits().ToString();

        isSold = PlayerPrefs.GetInt("IsSold");

        if (units >= 5 && isSold == 0)
            buy.interactable = true;
        else
            buy.interactable = false;
    }

    public void buyUpgrade(ShopButton clickedButton)
    {
        playerStats.ModifyUnits(-clickedButton.upgrade.cost);
        playerStats.ApplyUpgrade(clickedButton.upgrade);
        /*units -= 5;
        PlayerPrefs.SetInt("IsSold", 1);*/
        //weaponText.text = "Already Sold!";
        //buy.gameObject.SetActive(false);
    }

    public void buyWeapon(ShopButton clickedButton)
    {

    }

    public void buyShip(ShopButton clickedButton)
    {

    }

    public void exitShop()
    {
        shop.gameObject.SetActive(false);

        //PlayerPrefs.SetInt("Units", units);
        //SceneManager.LoadScene("MidtermScene");
    }

    public void shipShop()
    {
        //shop.gameObject.SetActive(false);
        //shop2.gameObject.SetActive(true);
        foreach (var obj in shipShopUI)
            obj.SetActive(true);

        foreach (var obj in upgradeShopUI)
            obj.SetActive(false);
    }

    public void back()
    {
        shop.gameObject.SetActive(true);
        //shop2.gameObject.SetActive(true);
        foreach (var obj in shipShopUI)
            obj.SetActive(false);

        foreach (var obj in upgradeShopUI)
            obj.SetActive(true);
    }

    public void resetPlayerPrefs()
    {
        playerStats.ResetUnits(1000);
        //buy.gameObject.SetActive(true);
        //weaponText.text = "Dual Lasers \n Price: 5 Units";
        //PlayerPrefs.DeleteAll();
    }

    public void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        itemDescriptions[0].SetActive(true);
        Debug.Log("Mouse is over GameObject.");

    }

    public void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        itemDescriptions[0].SetActive(false);
        Debug.Log("Mouse is no longer on GameObject.");
    }

}
