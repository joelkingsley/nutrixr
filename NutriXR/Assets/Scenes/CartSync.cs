using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
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
    private GameObject remoteCreationHook;

    public struct Item
    {
        public string fdcName;
        public Vector3 position;
        public Quaternion rotation;
    }

    public readonly SyncList<Item> inventory = new SyncList<Item>();
    private List<IngredientItem> selectedIngredientItems = new List<IngredientItem>();              //items present in owner's cart
    private List<GameObject> visibleItems = new List<GameObject>();                                 //objects present in remote cart
    private Dictionary<IngredientItem, Item> ingToItem = new Dictionary<IngredientItem, Item>();    //used by owner of this cart
    private Dictionary<Item, GameObject> itemToItem = new Dictionary<Item, GameObject>();           //used by remote version of cart
    //owner: IngredientItem <--goToItem--> Item <--network--> Item <--itemToGo--> GameObject :remote

    public override void OnStartServer()
    {
        base.OnStartServer();
        deactivate.ForEach(x => x.SetActive(false));

        inventory.Callback += OnInventoryUpdate;
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
            //e.g. the shopping cart does not belong to player
            deactivate.ForEach(x => x.SetActive(false));

            inventory.Callback += OnInventoryUpdate;
        }
    }

    [Client]
    public void AddItemToCart(IngredientItem ingItem)       //executed by owner
    {
        if (netIdentity.isOwned)
        {
            selectedIngredientItems.Add(ingItem);
            Item item = new Item
            {
                fdcName = ingItem.fdcName,
                position = ingItem.gameObject.transform.localPosition,
                rotation = ingItem.gameObject.transform.rotation
            };
            ingToItem.Add(ingItem, item);
            CmdAddItemToCart(item);
        }
    }

    [Client]
    public void RemoveItemFromCart(IngredientItem ingItem)    //executed by owner
    {
        if (netIdentity.isOwned)
        {
            Item item;
            ingToItem.Remove(ingItem, out item); //retrieve corresponding item
            CmdRemoveItemFromCart(item);
        }
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

    void OnInventoryUpdate(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
    {
        if (!netIdentity.isOwned)   //only update remote carts, not the owned one
        {
            switch (op)
            {
                case SyncList<Item>.Operation.OP_ADD:
                    // index is where it was added into the list
                    // newItem is the new item
                    Debug.Log(newItem.fdcName + " was added to the Cart");
                    break;
                case SyncList<Item>.Operation.OP_INSERT:
                    // index is where it was inserted into the list
                    // newItem is the new item
                    break;
                case SyncList<Item>.Operation.OP_REMOVEAT:
                    // index is where it was removed from the list
                    // oldItem is the item that was removed
                    Debug.Log(oldItem.fdcName + " was removed from the Cart");
                    break;
                case SyncList<Item>.Operation.OP_SET:
                    // index is of the item that was changed
                    // oldItem is the previous value for the item at the index
                    // newItem is the new value for the item at the index
                    break;
                case SyncList<Item>.Operation.OP_CLEAR:
                    // list got cleared
                    break;
            }
        }
    }
}
