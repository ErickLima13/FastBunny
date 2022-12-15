using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScenery : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D sceneryRb;

    private bool isInstantiated;

    private void Initialization()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        sceneryRb = GetComponent<Rigidbody2D>();

        sceneryRb.velocity = new(gameManager.objectsSpeed, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        AutoDestruction();
        SpawnScenery();
    }

    private void SpawnScenery()
    {
        int index;
        int rand = Random.Range(0, 100);

        if(rand < 50)
        {
            index = 0;
        }
        else
        {
            index = 1;
        }

        if (!isInstantiated)
        {
            if(transform.position.x <= 0)
            {
                isInstantiated = true;
                GameObject temp = Instantiate(gameManager.sceneryPrefab[index]);
                Vector3 pos = new(transform.position.x + gameManager.sizeBridge, transform.position.y, 0);
                temp.transform.position = pos;
            }
        }
    }

    private void AutoDestruction()
    {
        if(transform.position.x < gameManager.limitDestruction)
        {
            Destroy(gameObject);
        }
    }
}
