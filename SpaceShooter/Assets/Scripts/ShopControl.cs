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
    [SerializeField] GameObject gObject;
    [SerializeField] GameObject Shop;

    public TextMeshProUGUI unitsText;
    public TextMeshProUGUI weaponText;
    [SerializeField] PlayerStats playerStats;

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

    public void buyWeapon()
    {
        /*units -= 5;
        PlayerPrefs.SetInt("IsSold", 1);*/
        playerStats.ModifyUnits(-5);
        weaponText.text = "Already Sold!";
        buy.gameObject.SetActive(false);
    }

    public void exitShop()
    {
        PlayerPrefs.SetInt("Units", units);
        SceneManager.LoadScene("MidtermScene");
    }

    public void resetPlayerPrefs()
    {
        units = 1000;
        buy.gameObject.SetActive(true);
        weaponText.text = "Dual Lasers \n Price: 5 Units";
        PlayerPrefs.DeleteAll();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == gObject)
        {
            Shop.gameObject.SetActive(true);
        }
    }
}
