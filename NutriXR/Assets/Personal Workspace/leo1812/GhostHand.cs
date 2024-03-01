using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class GhostHand : MonoBehaviour
{
    [Tooltip("The SyntheticHand this GhostHand should copy.")]
    [SerializeField] private GameObject SyntheticHand;

    [Tooltip("The HandGrabInteractor the GhostHand should listen to. GhostHand is activated when the HandGrabInteractor is triggered.")]
    [SerializeField] private HandGrabInteractor handGrabInteractor;

    //Initial Anchor of the GhostHand. Default is LeftHandAnchor/RightHandAchor
    private Transform GhostHandAnchor;

    //Get the SkinnedMeshRenderer from the SyntheticHand as a field for easier access
    private SkinnedMeshRenderer SyntheticHandMeshRenderer;

    //Is true while GhostHand is active. False otherwise
    private bool grabbing = false;

    void Start()
    {
        //Get values
        SyntheticHandMeshRenderer = SyntheticHand.GetComponentInChildren<SkinnedMeshRenderer>();
        GhostHandAnchor = transform.parent;

        //GhostHand should not be visible at the start of the application
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        //GhostHand is not active but HandgrabInterActor is triggered => Activate GhostHand
        if (!grabbing && handGrabInteractor.IsGrabbing && handGrabInteractor.SelectedInteractable != null)
        {
            //Start of Grab
            grabbing = true;
            StartCoroutine(_StartGrab(handGrabInteractor.SelectedInteractable.transform));
        }

        //GhostHand is currently active but HandGrabInteractor is not => Deactivate GhostHand
        if (grabbing && !handGrabInteractor.IsGrabbing)
        {
            //End of Grab
            grabbing = false;
            _EndGrab();

        }
    }

    /// <summary>
    /// Positions the GhostHand according to the current position of the MainHand
    /// and sets it as child of the anchor provided as parameter, so that the GhostHand
    /// follows the Interactable
    /// </summary>
    /// <param name="anchor"></param>
    /// <returns></returns>
    IEnumerator _StartGrab(Transform anchor)
    {
        //The Hand-Grabbable Snaps to Hand position. Need to wait for this to be complete:
        yield return new WaitForSeconds(0.01f);

        //Copy the current hand mesh and apply it to the GhostHand
        Mesh mesh = new Mesh();
        SyntheticHandMeshRenderer.BakeMesh(mesh, true); //Copy
        GetComponentInChildren<MeshFilter>().mesh = mesh;       //Apply

        //The parent is now the interactable, so that the GhostHand stays locked
        transform.SetParent(anchor, true);

        //Deactiavte the MainHand and activate the GhostHand
        GetComponentInChildren<MeshRenderer>().enabled = true;
        SyntheticHand.SetActive(false);
    }


    /// <summary>
    /// Resets the GhostHand to the intial position.
    /// </summary>
    private void _EndGrab()
    {
        //Put Ghost Hand back in original Position
        transform.SetParent(GhostHandAnchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //Deactivate GhostHand and activate MainHand
        GetComponentInChildren<MeshRenderer>().enabled = false;
        SyntheticHand.SetActive(true);
    }
}
