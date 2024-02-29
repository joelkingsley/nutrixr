using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Serialization;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject HandSynthetic;
    [SerializeField] private Transform HandAnchor;
    [SerializeField] private SkinnedMeshRenderer MeshRenderer;
    [SerializeField] private GameObject GhostHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Mesh mesh = new Mesh();
        //MeshRenderer.BakeMesh(mesh, true);
        //GhostHand.GetComponentInChildren<MeshFilter>().mesh = mesh;
        //GhostHand.SetActive(true);
    }

    public void StartGrab()
    {
        StartCoroutine(_StartGrab());
    }

    IEnumerator _StartGrab()
    {
        yield return new WaitForSeconds(0.1f);
        Mesh mesh = new Mesh();
        MeshRenderer.BakeMesh(mesh, true);
        GhostHand.GetComponentInChildren<MeshFilter>().mesh = mesh;

        GhostHand.transform.SetParent(transform, true);

        GhostHand.SetActive(true);
        HandSynthetic.SetActive(false);
    }

    public void EndGrab()
    {

        GhostHand.transform.SetParent(HandAnchor);
        GhostHand.transform.localPosition = Vector3.zero;
        GhostHand.transform.localRotation = Quaternion.identity;

        GhostHand.SetActive(false);
        HandSynthetic.SetActive(true);
    }
}
