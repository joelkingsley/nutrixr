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

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnKitchenSceneItems(List<IngredientItemData> ingredientItems)
    {
        Debug.Log("Kitchen Scene items");
        SpawnItems(ingredientItems);
    }

    void SpawnItems(List<IngredientItemData> ingredientItems)
    {
        int amount = ingredientItems.Count;
        for (int i = 0; i < amount; i++)
        {
            GameObject prefab = LoadPrefab(ingredientItems[i].fdcName);

            int row = i / totalCols;
            int col = i % totalCols;
            SpawnItemAt(row, col, prefab);
        }
    }

    private GameObject LoadPrefab(string fdcName)
    {
        return (GameObject)Resources.Load("IngredientPrefabs/" + fdcName, typeof(GameObject));
    }

    void SpawnItemAt(int row, int col, GameObject prefab)
    {
        float xOffset = (spawnAreaX / totalRows) * row;
        float zOffset = (spawnAreaZ / totalCols) * col;

        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, transform.position.y,
            transform.position.z + zOffset);

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
