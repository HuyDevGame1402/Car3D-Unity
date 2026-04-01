using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    [SerializeField] private CarHander carHander;
    [SerializeField] private LayerMask otherCarsLayerMask;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private RaycastHit[] raycastHits = new RaycastHit[1];
    [SerializeField] private bool isCarAhead = false;
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    int drivingInLane = 0;

    private void Awake()
    {
        if (CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }
    private void Start()
    {
        StartCoroutine(UpdateLessOftenCO());
    }
    private void Update()
    {
        float accelerationInput = 1.0f;
        float steerInput = 0.0f;
        if (isCarAhead) accelerationInput = -1f;

        float desiredPositionX = Utils.CarLanes[drivingInLane];
        float difference = desiredPositionX - transform.localPosition.x;
        if(Mathf.Abs(difference) > 0.05f)
        {
            steerInput = 1.0f * difference;
        }

        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);
        carHander.SetInput(new Vector2(steerInput, accelerationInput));
    }
    private IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarsIsAhead();
            yield return wait;
        }
    }
    private bool CheckIfOtherCarsIsAhead()
    {
        meshCollider.enabled = false;
        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f,
            transform.forward, raycastHits, Quaternion.identity, 2, otherCarsLayerMask);
        meshCollider.enabled = true;
        if(numberOfHits > 0) return true;

        return false;
    }
    private void OnEnable()
    {
        carHander.SetMaxSpeed(Random.Range(2, 4));
        drivingInLane = Random.Range(0, Utils.CarLanes.Length);
    }
}
