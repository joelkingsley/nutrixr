using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class CartSync : NetworkBehaviour
{
    //Carts
    [Header("Local Cart")] [SerializeField]
    private GameObject localCart;
    [Space] [Header("Remote Cart")] [SerializeField]
    private GameObject remoteCart;

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


    public override void OnStartServer()
    {
        base.OnStartServer();
        localCart.SetActive(false);
        remoteCart.SetActive(true);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (netIdentity.isOwned)    //e.g. the shopping cart belongs to player
        {
            localCart.SetActive(true);
            remoteCart.SetActive(false);

            //Connect Shopping cart to the players local BasketSystem and RecipeSystem
            GameObject.FindWithTag("RecipeSystem").GetComponent<RecipeSystem>().recipeUIScrollViewContent = recipes_content;
            basketSystem = GameObject.FindWithTag("BasketSystem").GetComponent<BasketSystem>();
            basketSystem.basketUIScrollViewContent = basket_content;
        }
        else
        {
            localCart.SetActive(false);
            remoteCart.SetActive(true);
        }
    }

    [Command]   //Invoked by Client, executed on Server
    public void CmdSendCartToServer(Vector3[] positions, Quaternion[] rotations, string[] fdcNames)
    {
        //Forward to all the clients
        RpcSendCartToClient(positions, rotations, fdcNames);
    }

    [ClientRpc] //Invoked by Server, executed on Client
    public void RpcSendCartToClient(Vector3[] positions, Quaternion[] rotations, string[] fdcNames)
    {
        if (!netIdentity.isOwned)
        {
            //This is another players cart

            //Update:
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < positions.Length; i++)
            {

            }
        }
    }

    //Steaming Loop
    private IEnumerator StreamCartData()
    {
        while (true)
        {
            if (isLocalPlayer)
            {
                //collect basketSystem and turn into sync-able data array
                Vector3[] positions = new Vector3[basketSystem.selectedItems.Count];
                Quaternion[] rotations = new Quaternion[basketSystem.selectedItems.Count];
                string[] fdcNames = new string[basketSystem.selectedItems.Count];

                for (int i = 0; i < basketSystem.selectedItems.Count; i++)
                {
                    IngredientItem ingItem = basketSystem.selectedItems[i];
                    positions[i] = ingItem.gameObject.transform.position;
                    rotations[i] = ingItem.gameObject.transform.rotation;
                    fdcNames[i] = ingItem.fdcName;
                }
                CmdSendCartToServer(positions, rotations, fdcNames);
            }

            yield return new WaitForSeconds(1f);
        }
    }








    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
