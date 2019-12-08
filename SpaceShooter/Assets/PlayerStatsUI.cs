using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStatsUI : MonoBehaviour
{

    [SerializeField] GameObject bg;
    [SerializeField] GameObject border;
    [SerializeField] Image shipIcon;
    [SerializeField] Image factionIcon;
    [SerializeField] TextMeshProUGUI hullText;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI factionText;
    [SerializeField] TextMeshProUGUI reputationText;

    [SerializeField] List<Image> hyperdriveComps = new List<Image>();

    public void UpdateDisplay(Sprite shipSprite, Sprite factionSprite, string hulltxt, string shieldtxt, string speedtxt, string factiontxt, string reptxt, bool h1, bool h2, bool h3)
    {
        bg.SetActive(true);
        border.SetActive(true);
        shipIcon.sprite = shipSprite;
        factionIcon.sprite = factionSprite;
        hullText.text = hulltxt;
        shieldText.text = shieldtxt;
        speedText.text = speedtxt;
        factionText.text = factiontxt;
        reputationText.text = reptxt;
    }

    public void Hide()
    {
        bg.SetActive(false);
        border.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
