using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public float bounceDistance = 5f;
    public float bounceSpeed = 4f;
    private Vector3 basePosition;
    
    void Start()
    {
        basePosition = transform.position;
    }
    
    public void UpdateBasePosition(Vector3 newBasePosition)
    {
        basePosition = newBasePosition;
    }
    
    void Update()
    {
        float offset = Mathf.Sin(Time.time * bounceSpeed) * bounceDistance;
        transform.position = new Vector3(basePosition.x + offset, basePosition.y, basePosition.z);
    }
}