using UnityEngine;

public class PlayerControl {
    private GameObject player;
    private Rigidbody2D rigidbody2D;
    
    public PlayerControl() {
        player = GameObject.Instantiate(Resources.Load<GameObject>("Player"));
        rigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    public void Update() {
        float h = Input.GetAxis("Horizontal");
        float y = rigidbody2D.velocity.y;
        rigidbody2D.velocity = new Vector2(h * 10, y);

        if (Input.GetKeyDown(KeyCode.Space)) {
            float jumpPower = 10;
            float x = rigidbody2D.velocity.x;
            rigidbody2D.velocity = new Vector2(x, jumpPower);
        }
    }

    public void Destory() {
        GameObject.Destroy(player);
    }
}
