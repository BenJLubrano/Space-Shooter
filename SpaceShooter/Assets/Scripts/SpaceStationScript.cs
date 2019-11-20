using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStationScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject playerUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            shop.gameObject.SetActive(true);
            playerUI.gameObject.SetActive(false);
            //playerUI.SetActive(false);

        }
    }

}
