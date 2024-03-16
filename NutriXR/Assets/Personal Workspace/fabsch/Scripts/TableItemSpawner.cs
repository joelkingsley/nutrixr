using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableItemSpawner : MonoBehaviour
{
    [SerializeField]
    public float spawnArea_x = 1.0f;
    [SerializeField]
    public float spawnArea_z = 1.0f;

    [SerializeField]
    public int total_rows = 5;

    [SerializeField]
    public int total_cols = 4;

    [SerializeField] public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnItems(20);
    }

    void SpawnItems(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int row = i / total_cols;
            int col = i % total_cols;
            SpawnItemAt(row, col, itemPrefab);
        }
    }

    void SpawnItemAt(int row, int col, GameObject prefab)
    {
        float xOffset = (spawnArea_x / total_rows) * row;
        float zOffset = (spawnArea_z / total_cols) * col;

        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y,
            transform.position.z + zOffset);

        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
