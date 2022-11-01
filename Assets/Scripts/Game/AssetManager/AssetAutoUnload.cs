using UnityEngine;

public class AssetAutoUnload : MonoBehaviour
{
    [SerializeField]
    private float time = 3f;
    private float useTime;

    private void OnEnable()
    {
        useTime = 0;
    }

    private void Update()
    {
        useTime += Time.deltaTime;
        if (useTime >= time)
        {
            gameObject.UnLoadPrefab();
        }
    }
}
