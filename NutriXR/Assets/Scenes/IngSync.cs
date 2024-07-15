using System.Collections;
using System.Collections.Generic;
using Mirror;
using Oculus.Avatar2;
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
    //[SyncVar(hook = nameof(OnLeftHandUpdate))] private Item LeftHandItem;
    //[SyncVar(hook = nameof(OnRightHandUpdate))] private Item RightHandItem;
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
            CmdSetLeftHand(IngredientItemToItem(ingItem));
        }
    }

    [Client]
    public void SetRightHandItem(IngredientItem ingItem)
    {
        if (netIdentity.isOwned)
        {
            CmdSetRightHand(IngredientItemToItem(ingItem));
        }
    }

    [Client]
    public void ResetLeftHandItem()
    {
        if (netIdentity.isOwned)
        {
            CmdResetLeftHand();
        }
    }

    [Client]
    public void ResetRightHandItem()
    {
        if (netIdentity.isOwned)
        {
            CmdResetRightHand();
        }
    }
    //##### #####

    //##### SERVER #####

    [Command]
    public void CmdSetLeftHand(Item item)
    {
        UpdateLeftHand(item);
    }

    [Command]
    public void CmdSetRightHand(Item item)
    {
        UpdateRightHand(item);
    }

    [Command]
    public void CmdResetLeftHand()
    {
        UpdateLeftHand(NULL_ITEM);
    }

    [Command]
    public void CmdResetRightHand()
    {
        UpdateRightHand(NULL_ITEM);
    }
    //##### #####

    //##### CLIENT UPDATER #####
    [ClientRpc(includeOwner = false)] //only update remote avatars, not the owned one
    void UpdateLeftHand(Item item)
    {
        if (item.name == null)
        {
            //Reset
            Destroy(LeftHandGameObject);
        }
        else
        {
            //Instantiate
            GameObject toSpawn = (GameObject)Resources.Load("Ingredients/Prefabs/" + item.name, typeof(GameObject));
            GameObject spawned = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);

            if (leftHand == null)
            {
                leftHand = transform.Find("RemoteAvatar").GetComponent<SampleAvatarEntity>().GetSkeletonTransform(CAPI.ovrAvatar2JointType.LeftHandWrist);
            }

            spawned.transform.SetParent(leftHand);
            spawned.transform.position = item.position;
            spawned.transform.rotation = item.rotation;

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

    [ClientRpc(includeOwner = false)] //only update remote avatars, not the owned one
    void UpdateRightHand(Item item)
    {
        if (item.name == null)
        {
            //Reset
            Destroy(RightHandGameObject);
        }
        else
        {
            //Instantiate
            GameObject toSpawn = (GameObject)Resources.Load("Ingredients/Prefabs/" + item.name, typeof(GameObject));
            GameObject spawned = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);

            if (rightHand == null)
            {
                rightHand = transform.Find("RemoteAvatar").GetComponent<SampleAvatarEntity>().GetSkeletonTransform(CAPI.ovrAvatar2JointType.RightHandWrist);
            }

            spawned.transform.SetParent(rightHand);
            spawned.transform.position = item.position;
            spawned.transform.rotation = item.rotation;

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
