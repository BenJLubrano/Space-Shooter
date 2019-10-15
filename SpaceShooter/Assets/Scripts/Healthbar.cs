using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public float speed;
    public RectTransform healthTransform;
    private float cachedY;
    private float minXValue;
    private float maxXValue;
    private int currentHealth;

    private int CurrentHealth
    {
        get { return currentHealth; }
        set {
            currentHealth = value;
            HandleHealth();
        }
    }
    public float maxHealth;
    public Text healthText;
    public Image visualHealth;
    public float coolDown;
    private bool onCD;

    // Start is called before the first frame update
    void Start()
    {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = maxXValue - healthTransform.rect.width;
        currentHealth = maxHealth;
        onCD = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthTransform.GetComponent<CanvasRenderer>().hideIfInvisible = true)
        {
            healthTransform.GetComponent<CanvasRenderer>().hideIfInvisible = false;
        }
    }

    private void HandleHealth()
    {
        healthText.text = "Heath: " + currentHealth;

        float currentXValue = MapValues(currentHealth, 0, maxHealth, minXValue, maxXValue);

        healthTransform.position = new Vector3(currentXValue, cachedY);

        if (currentHealth > maxHealth/2) // over 50%
        {
            visualHealth.color = new Color32((byte)MapValues(currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
        }
        else // <= 50%
        {
            visualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth / 2, maxHealth, 0, 255), 0, 255);
        }
    }

    IEnumerator CoolDownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDown);
        onCD = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Damage")
        {
            if (!onCD && currentHealth > 0)
            {
              StartCoroutine(CoolDownDmg());
              CurrentHealth -= 1;
            }
        }

        if (other.name == "Health")
        {
            if (!onCD && currentHealth < maxHealth)
            {
              StartCoroutine(CoolDownDmg());
              CurrentHealth += 1;
            }
        }
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
