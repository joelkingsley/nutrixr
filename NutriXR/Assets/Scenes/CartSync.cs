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
        public string fdcName;
        public Vector3 position;
        public Quaternion rotation;
    }

    //-----#####-----
    public readonly SyncHashSet<Item> inventory = new SyncHashSet<Item>();
    //-----#####-----

    //-----OWNER-----
    //private List<IngredientItem> selectedIngredientItems = new List<IngredientItem>();              //IngredientItems present in owner's cart
    private Dictionary<IngredientItem, Item> ingToItem = new Dictionary<IngredientItem, Item>();    //used by owner of this cart

    //-----REMOTE-----
    //private List<Item> visibleItems = new List<Item>();                                             //Items present in remote cart
    private Dictionary<Item, GameObject> itemToPrefab = new Dictionary<Item, GameObject>();           //used by remote version of cart

    //owner: IngredientItem --> ingToItem --> Item <--network--> Item --itemToItem--> GameObject :remote

    public override void OnStartServer()
    {
        base.OnStartServer();
        SetUpForRemote();
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
        foreach (GameObject child in transform)
        {
            child.tag = "Untagged";
            child.layer = LayerMask.NameToLayer("Remote");
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
            fdcName = ingItem.fdcName,
            position = ingItem.gameObject.transform.localPosition,
            rotation = ingItem.gameObject.transform.localRotation
        };
        ingToItem.Add(ingItem, item);
        CmdAddItemToCart(item);
    }

    [Client]
    public void RemoveItemFromCart(IngredientItem ingItem)    //executed by owner
    {
        if (netIdentity.isOwned)
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
        inventory.Add(item);
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
                    Debug.Log(item.fdcName + " was added to the Cart at pos " + item.position);

                    //Instantiate
                    GameObject toSpawn = (GameObject)Resources.Load("IngredientPrefabs/" + item.fdcName, typeof(GameObject));
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
                    Debug.Log(item.fdcName + " was removed from the Cart");

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
