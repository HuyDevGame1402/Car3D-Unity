using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] carAIPrefabs;
    [SerializeField] private GameObject[] carAIPool = new GameObject[20];
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    [SerializeField] private Transform playerCarTransform;
    [SerializeField] private float timeLastCarSpawned = 0;

    [SerializeField] private LayerMask otherCarsLayerMask;
    [SerializeField] private Collider[] overlappedCheckCollider = new Collider[1];

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
        int prefabIndex = 0;
        for(int i = 0; i < carAIPool.Length; i++)
        {
            carAIPool[i] = Instantiate(carAIPrefabs[prefabIndex]);
            carAIPool[i].SetActive(false);
            prefabIndex++;
            if(prefabIndex > carAIPrefabs.Length - 1)
            {
                prefabIndex = 0;
            }
        }
        StartCoroutine(UpdateLessOFftenCO());
    }

    private IEnumerator UpdateLessOFftenCO()
    {
        while(true)
        {
            CleanUpCarsBeyondView();
            SpawnNewCars();
            yield return wait;
        }
    }
    private void SpawnNewCars()
    {
        if(Time.time - timeLastCarSpawned < 2)
        {
            return;
        }
        GameObject carToSpawn = null;
        foreach(GameObject aiCar in carAIPool)
        {
            if (aiCar.activeInHierarchy) continue;
            carToSpawn = aiCar;
            break;
        }
        if(carToSpawn == null) { return; }

        Vector3 spawnPosition = new Vector3(0, 0, playerCarTransform.transform.position.z + 100);

        if (Physics.OverlapBoxNonAlloc(spawnPosition, Vector3.one * 2, overlappedCheckCollider,Quaternion.identity,otherCarsLayerMask) > 0) return;

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

        timeLastCarSpawned = Time.time;
    }
    private void CleanUpCarsBeyondView()
    {
        foreach(GameObject aiCar in carAIPool)
        {
            if(!aiCar.activeInHierarchy) continue;

            if(aiCar.transform.position.z - playerCarTransform.transform.position.z > 200)
            {
                aiCar.SetActive(false);
            }

            if(aiCar.transform.position.z - playerCarTransform.transform.position.z < -50)
            {
                aiCar.SetActive(false);
            }
        }
    }
}
