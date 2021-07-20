using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 100)] float height = 5;
    [SerializeField] [Range(0f, 5)] float smoothness =0.2f;


    Transform player;
    Vector3 Offset;
    Quaternion rot;
   
    void Start()
    {
        Quaternion rot;
        Transform snakeparent = FindObjectOfType<SnakeMovementController>().transform;
        player = snakeparent.GetChild(0);
        Offset = player.position;

        StartCoroutine(startLookingafter());


    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            rot = transform.rotation;
            Offset = player.position;
            Offset.y = height;
            transform.position = Vector3.Lerp(transform.position, Offset, 0.99999f);
        }

       
    
    }

 


    IEnumerator startLookingafter()
    {
        yield return new WaitForSeconds(0.1f);
        if (player)
        {
        transform.LookAt(player);
        StartCoroutine(startLookingafter());
        }

               
    }

}
