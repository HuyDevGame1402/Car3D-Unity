using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeObject : MonoBehaviour
{
    [SerializeField] private Vector3 localRotationMin = Vector3.zero;
    [SerializeField] private Vector3 localRotationMax = Vector3.zero;
    [SerializeField] private float localscaleMultiplierMin = 0.8f;
    [SerializeField] private float localscaleMultiplierMax = 1.5f;

    [SerializeField] private Vector3 localScaleOriginal = Vector3.one;

    private void Start()
    {
        localScaleOriginal = transform.localScale;
    }

    private void OnEnable()
    {
        // random góc quay rotation cho các cái cây
        transform.localRotation = Quaternion.Euler(Random.Range(localRotationMin.x,
            localRotationMax.x), Random.Range(localRotationMin.y,
            localRotationMax.y), Random.Range(localRotationMin.z,
            localRotationMax.z));
        // tính lại scale khi sd lại section từ pool
        transform.localScale = localScaleOriginal * Random.Range(localscaleMultiplierMin, localscaleMultiplierMax);
    }
}
