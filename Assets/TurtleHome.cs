using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleHome : MonoBehaviour
{
    [SerializeField] private GameObject turtle;
    [SerializeField] private GameObject turtInst;

    // Start is called before the first frame update
    void Awake()
    {
        turtInst = Instantiate(turtle, transform.position + new Vector3(1, 0, 1), Quaternion.identity, transform);
        turtInst.GetComponent<TurtleAI>().home = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
