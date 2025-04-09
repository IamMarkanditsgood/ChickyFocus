using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Life());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Life()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
