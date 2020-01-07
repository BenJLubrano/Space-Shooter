using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Laser : MonoBehaviour
{
    [SerializeField] float delayTime = 2f;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] GameObject laser;

    // Update is called once per frame
    void Update()
    {
        delayTime -= Time.deltaTime;
        if(delayTime <= 0)
        {
            laser.SetActive(true);
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0)
            {
                Destroy(this);
            }
        }
    }
}
