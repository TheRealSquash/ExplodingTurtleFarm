using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleVision : MonoBehaviour
{
    private SphereCollider awareness;
    public float range = 0f;
    [SerializeField] private TurtleAI ai;
    private float timeElapsed = 0f, lerpDuration = 0.5f;


    void Awake() 
    {
        awareness = GetComponent<SphereCollider>();
    }

    void Update()
    {
        if (range < ai.awareness)
        {
            if (timeElapsed < lerpDuration)
            {
                range = Mathf.Lerp(0, ai.awareness, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                awareness.radius = range;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ai.SetFood(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving " + other.gameObject.name);
    }
}
