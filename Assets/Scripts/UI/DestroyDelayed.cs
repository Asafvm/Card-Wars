using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelayed : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    
    private void Start()
    {
        Destroy(gameObject, delay);
    }
    
}
