using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class GhostHand : MonoBehaviour
{

    [SerializeField] private GameObject SyntheticHand;
    [SerializeField] private HandGrabInteractor handGrabInteractor;

    private Transform GhostHandAnchor;
    private SkinnedMeshRenderer SyntheticHandMeshRenderer;

    private bool grabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        SyntheticHandMeshRenderer = SyntheticHand.GetComponentInChildren<SkinnedMeshRenderer>();
        GhostHandAnchor = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!grabbing && handGrabInteractor.IsGrabbing && handGrabInteractor.SelectedInteractable != null)
        {
            //Start of Grab
            grabbing = true;

            StartCoroutine(_StartGrab(handGrabInteractor.SelectedInteractable.transform));
        }

        if (grabbing && !handGrabInteractor.IsGrabbing)
        {
            //End of Grab
            grabbing = false;

            //Put Ghost Hand in original Position
            transform.SetParent(GhostHandAnchor);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            //Swap Visuals
            GetComponentInChildren<MeshRenderer>().enabled = false;
            SyntheticHand.SetActive(true);
        }
    }

    IEnumerator _StartGrab(Transform anchor)
    {
        yield return new WaitForSeconds(0.1f);
        Mesh mesh = new Mesh();
        SyntheticHandMeshRenderer.BakeMesh(mesh, true);
        GetComponentInChildren<MeshFilter>().mesh = mesh;

        transform.SetParent(anchor, true);

        GetComponentInChildren<MeshRenderer>().enabled = true;
        SyntheticHand.SetActive(false);
    }
}
