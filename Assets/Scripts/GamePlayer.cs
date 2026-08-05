using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePlayer : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed_x = 150.0f;
    public float speed_y = 30.0f;
    public PhotonView photonView;
    public GameObject gun;
    public Transform bulletSpawnPoint;
    public Rigidbody rbody;
    public float bulletSpeed = 40f;
    private Collider playerCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (photonView.IsMine)
        {
            
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(0, horizontalInput * speed_x * Time.deltaTime, 0);
        
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * speed_y * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("Fire", RpcTarget.All);
        }
    }
    }
    [PunRPC]
    public void Fire(PhotonMessageInfo info)
{
    Debug.Log("Fogo hehe!");
    
    GameObject bullet = Instantiate(gun, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

    Collider bulletCollider = bullet.GetComponent<Collider>();
    if (playerCollider != null && bulletCollider != null)
    {
        Physics.IgnoreCollision(bulletCollider, playerCollider);
    }

    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = -transform.forward * bulletSpeed;
    }

    Destroy(bullet, 3f);
}
}
