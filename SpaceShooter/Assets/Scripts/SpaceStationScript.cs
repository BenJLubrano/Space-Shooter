using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject playerUI;
    [SerializeField] ShopControl shopControl;

    bool canAppear = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            shopControl.SetPlayerStats(player.GetComponent<PlayerStats>());
            shop.gameObject.SetActive(true);
            playerUI.gameObject.SetActive(false);
            canAppear = false;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            shop.gameObject.SetActive(false);
            playerUI.gameObject.SetActive(true);
            canAppear = true;
        }
    }

}
