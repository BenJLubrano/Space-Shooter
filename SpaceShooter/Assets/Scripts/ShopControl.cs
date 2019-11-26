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

    public Button buy;

    // Start is called before the first frame update
    void Start()
    {
        units = playerStats.GetUnits();
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
        shop.gameObject.SetActive(false);
        shop2.gameObject.SetActive(true);
    }

    public void back()
    {
        shop.gameObject.SetActive(false);
        shop2.gameObject.SetActive(true);
    }

    public void resetPlayerPrefs()
    {
        playerStats.ResetUnits(1000);
        //buy.gameObject.SetActive(true);
        //weaponText.text = "Dual Lasers \n Price: 5 Units";
        //PlayerPrefs.DeleteAll();
    }
   
}
