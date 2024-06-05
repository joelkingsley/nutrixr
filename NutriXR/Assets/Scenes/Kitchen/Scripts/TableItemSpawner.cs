using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TableItemSpawner : MonoBehaviour
{
    [SerializeField]
    public float spawnAreaX = 1.0f;
    [SerializeField]
    public float spawnAreaZ = 1.0f;

    [SerializeField]
    public int totalRows = 5;

    [SerializeField]
    public int totalCols = 4;

    private int currentPos = 0;

    public void SpawnKitchenSceneItems(List<Ingredient> ingredientItems)
    {
        Debug.Log("Kitchen Scene items");
        SpawnItems(ingredientItems);
    }

    void SpawnItems(List<Ingredient> ingredientItems)
    {
        int amount = ingredientItems.Count;
        for (int i = 0; i < amount; i++)
        {
            GameObject prefab = LoadPrefab(ingredientItems[i].name);

            int row = i / totalCols;
            int col = i % totalCols;
            SpawnItemAt(row, col, prefab);
        }
        currentPos = amount;
    }

    private GameObject LoadPrefab(string prefabName)
    {
        return (GameObject)Resources.Load("Ingredients/Prefabs/" + prefabName, typeof(GameObject));
    }

    void SpawnItemAt(int row, int col, GameObject prefab)
    {
        float xOffset = (spawnAreaX / totalRows) * row;
        float zOffset = (spawnAreaZ / totalCols) * col;

        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y,
            transform.position.z + zOffset);

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    public void addItem(GameObject item)
    {
        int row = currentPos / totalCols;
        int col = currentPos % totalCols;
        float xOffset = (spawnAreaX / totalRows) * row;
        float zOffset = (spawnAreaZ / totalCols) * col;
        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y,
            transform.position.z + zOffset);

        item.transform.position = spawnPosition;
        currentPos++;
    }
}
