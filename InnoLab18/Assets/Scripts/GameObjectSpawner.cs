using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal class SpawnTableEntry
{
    [SerializeField]
    internal GameObject gameObject;
    [SerializeField]
    internal int spawnCount = -1;
    [SerializeField]
    internal bool useStandardSpawnOffset = true;
    [SerializeField]
    internal Vector3 spawnOffset = Vector3.zero;
}

public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private bool spawnRandomEntry = true;
    [SerializeField]
    private bool autoSpawn = false;
    [SerializeField]
    private string spawnInput = "Fire3";
    [SerializeField]
    private bool canSpawn = false;
    [SerializeField]
    private float spawnRate = 1;
    [SerializeField]
    private float spawnCooldown = 0;
    [SerializeField]
    private Vector3 standardSpawnOffset = new Vector3(0,10,0);
    [SerializeField]
    private SpawnTableEntry[] SpawnTable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!autoSpawn)
        {
            canSpawn = Input.GetAxis(spawnInput) > 0;
        }

        if(spawnCooldown > 0)
        {
            spawnCooldown -= Time.deltaTime;
        }
        else if (canSpawn && SpawnTable.Length > 0)
        {
            spawnCooldown = spawnRate;

            SpawnTableEntry spawnTableEntry;
            if (spawnRandomEntry)
            {
                spawnTableEntry = getRandomSpawnTableEntry();
            }
            else
            {
                spawnTableEntry = getFirstSpawnTableEntry();
            }

            if(spawnTableEntry != null)
            {
                Vector3 position = transform.position;
                if (!spawnTableEntry.useStandardSpawnOffset)
                {
                    position += spawnTableEntry.spawnOffset;
                }
                else
                {
                    position += standardSpawnOffset;
                }
                Spawn(spawnTableEntry.gameObject, position);
            }
        }
    }

    protected virtual void Spawn(GameObject gameObject, Vector3 position)
    {
        Instantiate(gameObject, position, Quaternion.identity);
    }

    private SpawnTableEntry getRandomSpawnTableEntry()
    {
        if (SpawnTable.Length == 0)
        {
            return null;
        }

        int rnd = UnityEngine.Random.Range(0, SpawnTable.Length);
        while (SpawnTable[rnd].spawnCount == 0)
        {
            SpawnTable = SpawnTable.Where(val => val != SpawnTable[rnd]).ToArray();

            if (SpawnTable.Length == 0)
            {
                return null;
            }

            rnd = UnityEngine.Random.Range(0, SpawnTable.Length);
        }

        SpawnTableEntry spawnTableEntry = SpawnTable[rnd];

        if (SpawnTable[rnd].spawnCount > 0)
        {
            SpawnTable[rnd].spawnCount -= 1;
            if (SpawnTable[rnd].spawnCount == 0)
            {
                SpawnTable = SpawnTable.Where(val => val != SpawnTable[rnd]).ToArray();
            }
        }

        return spawnTableEntry;
    }

    private SpawnTableEntry getFirstSpawnTableEntry()
    {
        if (SpawnTable.Length == 0)
        {
            return null;
        }

        while (SpawnTable[0].spawnCount == 0)
        {
            SpawnTable = SpawnTable.Where(val => val != SpawnTable[0]).ToArray();

            if (SpawnTable.Length == 0)
            {
                return null;
            }
        }

        SpawnTableEntry spawnTableEntry = SpawnTable[0];

        if (SpawnTable[0].spawnCount > 0)
        {
            SpawnTable[0].spawnCount -= 1;
            if (SpawnTable[0].spawnCount == 0)
            {
                SpawnTable = SpawnTable.Where(val => val != SpawnTable[0]).ToArray();
            }
        }

        return spawnTableEntry;
    }
}
