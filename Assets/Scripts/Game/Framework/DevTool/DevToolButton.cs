using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevToolButton : MonoBehaviour, IPointerEnterHandler {
    public Action action;
    
    public void OnPointerEnter(PointerEventData eventData) {
        action?.Invoke();
    }
}