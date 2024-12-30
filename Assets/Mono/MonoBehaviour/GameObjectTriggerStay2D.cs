using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTriggerStay2D : MonoBehaviour
{
    public delegate void TriggerStay(Collider2D other);
    public TriggerStay triggerStay;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        triggerStay(other);
    }
}
