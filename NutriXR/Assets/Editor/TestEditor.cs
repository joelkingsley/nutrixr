using UnityEngine;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEditor;

[CustomEditor(typeof(IngredientItem))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IngredientItem myTarget = (IngredientItem)target;

        DrawDefaultInspector();

        if(GUILayout.Button("JUST CLICK ME!"))
        {

            Rigidbody rb = null;
            Grabbable gr = null;

            if (!myTarget.gameObject.GetComponent<Rigidbody>())
            {
                rb = myTarget.gameObject.AddComponent<Rigidbody>();
                rb.mass = 0.05f;
            }
            if (!myTarget.gameObject.GetComponent<Grabbable>())
            {
                gr = myTarget.gameObject.AddComponent<Grabbable>();
            }

            if (!myTarget.gameObject.GetComponent<HandGrabInteractable>())
            {
                HandGrabInteractable hgi = myTarget.gameObject.AddComponent<HandGrabInteractable>();
                hgi.InjectOptionalPointableElement((IPointableElement)gr);
                hgi.InjectRigidbody(rb);
            }
        }
    }
}
