using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{
    [SerializeField] private GameObject orignalObject;
    [SerializeField] private GameObject model;
    [SerializeField] private Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = model.GetComponentsInChildren<Rigidbody>(true);
    }
    private void Start()
    {
        //Explode(Vector3.forward);
    }
    public void Explode(Vector3 externalForce)
    {
        orignalObject.SetActive(false);
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.transform.parent = null;
            rigidbody.GetComponent<MeshCollider>().enabled = true;
            rigidbody.gameObject.SetActive(true);
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.AddForce(Vector3.up * 200 + externalForce, ForceMode.Force);
            rigidbody.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);
            rigidbody.gameObject.tag = "CarPart";
        }
    }

}
