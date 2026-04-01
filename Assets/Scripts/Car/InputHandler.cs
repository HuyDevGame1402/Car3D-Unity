using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CarHander carHander;

    private void Awake()
    {
        if (!CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        carHander.SetInput(input);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
