using System.Collections;
using System.Collections.Generic;
using Mirror;
using Oculus.Interaction;
using UnityEngine;

public class IngSync : NetworkBehaviour
{

    //Remote Left Hand Hook to where spawn the Items
    private Transform leftHand;

    //Remote Right Hand Hook to where spawn the Items
    private Transform rightHand;

    public struct Item
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    private Item NULL_ITEM;

    //-----#####-----
    [SyncVar(hook = nameof(OnLeftHandUpdate))] private Item LeftHandItem;
    [SyncVar(hook = nameof(OnRightHandUpdate))] private Item RightHandItem;
    //What is actually synced

    //-----OWNER-----
    private IngredientItem LeftHandIngredientItem;
    private IngredientItem RightHandIngredientItem;
    //IngredientItem to be synced

    //-----REMOTE-----
    private GameObject LeftHandGameObject;
    private GameObject RightHandGameObject;
    //GameObjects displayed at the remote avatars

    //owner: LeftHandIngredientItem --> LeftHandItem <--network--> LeftHandItem --> LeftHandGameObject :remote

    // Start is called before the first frame update
    void Start()
    {
        NULL_ITEM = new Item();
        NULL_ITEM.name = null;

        //Find hooks
        if (!netIdentity.isOwned)
        {
            StartCoroutine(WaitForAvatarToSpawn());
        }
    }

    IEnumerator WaitForAvatarToSpawn()
    {
        yield return new WaitForSeconds(5.0f);
        leftHand = transform.Find("RemoteAvatar/Joint LeftHandWrist");
        rightHand = transform.Find("RemoteAvatar/Joint RightHandWrist");

        if (rightHand == null)
        {
            Debug.LogWarning("Right Hand is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //##### CLIENT #####
    [Client]
    public void SetLeftHandItem(IngredientItem ingItem)
    {
        if (netIdentity.isOwned)
        {
            if (LeftHandItem.name == null)
            {
                //Setting the SyncVar
                CmdSetLeftHand(IngredientItemToItem(ingItem));
            }
        }
    }

    [Client]
    public void SetRightHandItem(IngredientItem ingItem)
    {
        if (netIdentity.isOwned)
        {
            if (RightHandItem.name == null)
            {
                //Setting the SyncVar
                CmdSetRightHand(IngredientItemToItem(ingItem));
            }
        }
    }

    [Client]
    public void ResetLeftHandItem()
    {
        if (netIdentity.isOwned)
        {
            LeftHandItem = NULL_ITEM;
            CmdResetLeftHand();
        }
    }

    [Client]
    public void ResetRightHandItem()
    {
        if (netIdentity.isOwned)
        {
            RightHandItem = NULL_ITEM;
            CmdResetRightHand();
        }
    }
    //##### #####

    //##### SERVER #####

    [Command]
    public void CmdSetLeftHand(Item item)
    {
        //Set SyncVar Item
        LeftHandItem = item;
    }

    [Command]
    public void CmdSetRightHand(Item item)
    {
        //Set SyncVar Item
        RightHandItem = item;
        Debug.Log("SETTING RIGHT HAND ITEM TO " + item.name);
    }

    [Command]
    public void CmdResetLeftHand()
    {
        //Reset SyncVar Item
        LeftHandItem = NULL_ITEM;
    }

    [Command]
    public void CmdResetRightHand()
    {
        //Reset SyncVar Item
        RightHandItem = NULL_ITEM;
    }
    //##### #####

    //##### CLIENT UPDATER #####
    void OnLeftHandUpdate(Item oldItem, Item newItem)
    {
        if (!netIdentity.isOwned) //only update remote avatars, not the owned one
        {
            if (newItem.name == null)
            {
                //Reset
                Destroy(LeftHandGameObject);
            }
            else
            {
                //Instantiate
                GameObject toSpawn = (GameObject)Resources.Load("Ingredients/Prefabs/" + newItem.name, typeof(GameObject));
                GameObject spawned = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);

                spawned.transform.SetParent(leftHand);
                spawned.transform.position = newItem.position;
                spawned.transform.rotation = newItem.rotation;

                //Deactivate unnessecary gameobjects
                spawned.GetComponent<IngredientItem>().enabled = false;
                spawned.GetComponent<Grabbable>().enabled = false;
                spawned.GetComponent<Rigidbody>().isKinematic = true;

                //Change physics layer
                spawned.layer = LayerMask.NameToLayer("Remote");

                //Store GameObject
                LeftHandGameObject = spawned;
            }
        }
    }

    void OnRightHandUpdate(Item oldItem, Item newItem)
    {
        if (!netIdentity.isOwned) //only update remote avatars, not the owned one
        {
            if (newItem.name == null)
            {
                //Reset
                Destroy(RightHandGameObject);
            }
            else
            {
                //Instantiate
                GameObject toSpawn = (GameObject)Resources.Load("Ingredients/Prefabs/" + newItem.name, typeof(GameObject));
                GameObject spawned = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);

                spawned.transform.SetParent(rightHand);
                spawned.transform.position = newItem.position;
                spawned.transform.rotation = newItem.rotation;

                //Deactivate unnessecary gameobjects
                spawned.GetComponent<IngredientItem>().enabled = false;
                spawned.GetComponent<Grabbable>().enabled = false;
                spawned.GetComponent<Rigidbody>().isKinematic = true;

                //Change physics layer
                spawned.layer = LayerMask.NameToLayer("Remote");

                //Store GameObject
                RightHandGameObject = spawned;
            }
        }
    }
    //##### #####

    //##### HELPER #####
    private Item IngredientItemToItem(IngredientItem ingItem)
    {
        Item item = new Item
        {
            name = ingItem.ingredient.name,
            position = ingItem.gameObject.transform.position,
            rotation = ingItem.gameObject.transform.rotation
        };
        return item;
    }
}
