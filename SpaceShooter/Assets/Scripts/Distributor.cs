using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class Distributor : MonoBehaviour
{
    Vector2 BLCorner;
    Vector2 BRCorner;
    Vector2 TRCorner;
    Vector2 TLCorner;
    [SerializeField] BoxCollider2D zone;
    public int objectCount = 10;
    public int minSpacing = 2;
    public GameObject[] prefabs;
    List<Vector2> spawnPositions = new List<Vector2>();
    // Start is called before the first frame update
    void Awake()
    {
        BLCorner = new Vector2(-(zone.size.x / 2), -(zone.size.y / 2));
        BRCorner = new Vector2((zone.size.x / 2), -(zone.size.y / 2));
        TLCorner = new Vector2(-(zone.size.x / 2), (zone.size.y / 2));
        TRCorner = new Vector2((zone.size.x / 2), (zone.size.y / 2));

        BLCorner += (Vector2)transform.position;
        BRCorner += (Vector2)transform.position;
        TLCorner += (Vector2)transform.position;
        TRCorner += (Vector2)transform.position;
        GeneratePositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneratePositions()
    {
        for(int i = 0; i < objectCount; i++)
        {
            int j = 0;
            Vector2 newPos = GeneratePosition();
            for(; j < 1000; j++)
            {
                if (!CheckPositions(newPos))
                {
                    newPos = GeneratePosition();
                }
                else break;
                
            }
            Debug.Log("J: "+ j);
            spawnPositions.Add(newPos);
        }
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        foreach (Vector2 position in spawnPositions)
        {
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360))), gameObject.transform);
        }
    }


    Vector2 GeneratePosition()
    {
        float xPos = Random.Range(BLCorner.x, TRCorner.x);
        float yPos = Random.Range(BLCorner.y, TRCorner.y);
        return new Vector2(xPos, yPos);
    }

    //returns whether or not the position complies with the spacing requirements
    bool CheckPositions(Vector2 newPos)
    {
        foreach(Vector2 position in spawnPositions)
        {
            if(Vector2.Distance(newPos, position) < minSpacing)
            {
                return false;
            }
        }

        return true;
    }
}
