using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTrigger2D : MonoBehaviour
{
    public delegate void TriggerEnter(Collider2D other);
    public TriggerEnter triggerEnterEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerEnterEvent(other);
    }
}
