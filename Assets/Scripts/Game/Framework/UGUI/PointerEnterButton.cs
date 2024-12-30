using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEnterButton : MonoBehaviour, IPointerEnterHandler {
    public Action action;
    
    public void OnPointerEnter(PointerEventData eventData) {
        action?.Invoke();
    }
}