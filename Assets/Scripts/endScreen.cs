using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScreen : MonoBehaviour
{
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        color.a += 0.25f * Time.deltaTime;
        GetComponent<SpriteRenderer>().color = color;
    }
}
