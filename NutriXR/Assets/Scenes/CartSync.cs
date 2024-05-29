using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Oculus.Interaction;
using UnityEngine;

public class CartSync : NetworkBehaviour
{
    //Carts
    [Header("Deactivate on Remote")] [SerializeField]
    private List<GameObject> deactivate;

    //Basket UI
    [Header("BasketUI Content View")] [SerializeField]
    private GameObject basket_content;

    //Recipes UI
    [Header("RecipesUI Content View")] [SerializeField]
    private GameObject recipes_content;

    //Basket System
    private BasketSystem basketSystem;

    //Remote Basket Item Creation Hook
    [Header("Remote Creation Hook")] [SerializeField]
    private Transform remoteCreationHook;

    public struct Item
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    //-----#####-----
    public readonly SyncHashSet<Item> inventory = new SyncHashSet<Item>();

    //-----OWNER-----
    private Dictionary<IngredientItem, Item> ingToItem = new Dictionary<IngredientItem, Item>();    //Used by owner of the cart
    //Maps an IngredientItem, which is not synced, to an Item, which is synced

    //-----REMOTE-----
    private Dictionary<Item, GameObject> itemToPrefab = new Dictionary<Item, GameObject>();         //used by remote version of cart
    //Maps an item, which is synced, to a GameObject, which is not synced and spawned in remote cart

    //owner: IngredientItem --> ingToItem --> Item <--network--> Item --itemToItem--> GameObject :remote

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!netIdentity.isOwned)
        {
            SetUpForRemote();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (netIdentity.isOwned)    //e.g. the shopping cart belongs to player
        {
            //Connect Shopping cart to the players local BasketSystem and RecipeSystem
            GameObject.FindWithTag("RecipeSystem").GetComponent<RecipeSystem>().recipeUIScrollViewContent = recipes_content;
            basketSystem = GameObject.FindWithTag("BasketSystem").GetComponent<BasketSystem>();
            basketSystem.basketUIScrollViewContent = basket_content;
        }
        else
        {
            SetUpForRemote();
        }
    }

    /// <summary>
    /// This function just deactivates and sets up all the stuff needed to turn the standard shopping cart into a remote one
    /// </summary>
    private void SetUpForRemote()
    {
        //e.g. the shopping cart does not belong to player
        deactivate.ForEach(x => x.SetActive(false));

        //delete all tags and layers
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("Remote");
        foreach (Transform child in transform)
        {
            child.gameObject.tag = "Untagged";
            child.gameObject.layer = LayerMask.NameToLayer("Remote");
        }

        //Subscribe to updates
        inventory.Callback += OnInventoryUpdate;
    }


    [Client]
    public void AddItemToCart(IngredientItem ingItem)       //executed by owner
    {
        if (netIdentity.isOwned)
        {
            CmdLog("Parent Name: " + ingItem.transform.parent.gameObject.name + ", LocalPos: " + ingItem.gameObject.transform.localPosition + ", GlobPos: " + ingItem.gameObject.transform.position);
            StartCoroutine(ReadAndSend(ingItem));
        }
    }

    private IEnumerator ReadAndSend(IngredientItem ingItem)
    {
        yield return new WaitForSeconds(2.0f);
        Item item = new Item
        {
            name = ingItem.ingredient.name,
            position = ingItem.gameObject.transform.localPosition,
            rotation = ingItem.gameObject.transform.localRotation
        };
        if (!ingToItem.ContainsKey(ingItem))
        {
            ingToItem.Add(ingItem, item);
            CmdAddItemToCart(item);
        }
        else
        {
            Debug.LogWarning("IngredientItem already contained in ingToItem");
        }
    }

    [Client]
    public void RemoveItemFromCart(IngredientItem ingItem)    //executed by owner
    {
        if (netIdentity.isOwned && ingToItem.ContainsKey(ingItem))
        {
            Item item;
            if (ingToItem.Remove(ingItem, out item))        //retrieve corresponding item
            {
                CmdRemoveItemFromCart(item);
            }
        }
    }

    [Command]
    public void CmdLog(string msg)
    {
        Debug.Log("[Client]: " + msg);
    }

    [Command]
    public void CmdAddItemToCart(Item item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
        }
    }

    [Command]
    public void CmdRemoveItemFromCart(Item item)
    {
        inventory.Remove(item);
    }

    void OnInventoryUpdate(SyncHashSet<Item>.Operation op, Item item)
    {
        if (!netIdentity.isOwned)   //only update remote carts, not the owned one
        {
            switch (op)
            {
                case SyncHashSet<Item>.Operation.OP_ADD:
                    Debug.Log(item.name + " was added to the Cart at pos " + item.position);

                    if (itemToPrefab.ContainsKey(item))
                    {
                        //In case this script is run as Host, this method is called twice: Once as client and once as server.
                        //This causes the prefabs to be instantiated twice and causes an exception when trying to add
                        //the key "item" to the itemToPrefab dictionary twice.
                        return;
                    }

                    //Instantiate
                    GameObject toSpawn = (GameObject)Resources.Load("Ingredients/Prefabs/" + item.name, typeof(GameObject));
                    GameObject spawned = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
                    spawned.transform.SetParent(remoteCreationHook);
                    spawned.transform.localPosition = item.position;
                    spawned.transform.localRotation = item.rotation;

                    //Deactivate unnessecary gameobjects
                    spawned.GetComponent<IngredientItem>().enabled = false;
                    spawned.GetComponent<Grabbable>().enabled = false;
                    spawned.GetComponent<Rigidbody>().isKinematic = true;

                    //Change physics layer
                    spawned.layer = LayerMask.NameToLayer("Remote");

                    //Add to the references
                    itemToPrefab.Add(item, spawned);
                    break;

                case SyncHashSet<Item>.Operation.OP_REMOVE:
                    Debug.Log(item.name + " was removed from the Cart");

                    //Find and remove corresponding gameObject
                    GameObject go;
                    if (itemToPrefab.Remove(item, out go))
                    {
                        Destroy(go);
                    }
                    break;

                case SyncHashSet<Item>.Operation.OP_CLEAR:
                    // list got cleared
                    break;
            }
        }
    }
}
