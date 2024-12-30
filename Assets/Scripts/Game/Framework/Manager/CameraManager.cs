using UnityEngine;

public class CameraManager {
    public Camera camera;

    private Vector3 playerLookPos;

    private Transform myTransform;

    public Transform MyTransform {
        get => myTransform;
    }

    public CameraManager() {
        camera = AssetManager.LoadPrefab("Prefabs/Camera").GetComponent<Camera>();
        myTransform = camera.transform;
        playerLookPos = myTransform.position;
    }

    public void LookPlayer(Vector3 playerPos) {
        myTransform.position = new Vector3(playerLookPos.x + playerPos.x, playerLookPos.y, playerLookPos.z + playerPos.z);
    }
}