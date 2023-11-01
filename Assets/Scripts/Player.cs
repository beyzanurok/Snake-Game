using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;
    public float horizontalLimit = 2.8f;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shootingCooldown;

    private float shootingTimer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //x üzerinde hareket etmesini istiyorum burada y üzerine hareket 0 olarak aldım
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * speed, 0);

        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        }
        else if (transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
        }

        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            //ateş etmek için
            if (Input.GetAxis("Fire1") == 1f)
            {
                shootingTimer = shootingCooldown;

                GameObject bulletInstance = Instantiate(bulletPrefab);
                bulletInstance.transform.SetParent(transform.parent);
                bulletInstance.transform.position = transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                Destroy(bulletInstance, 5f); // 5 saniyede bir mermi atabilir
            }
        }
    }
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Monster")
        {
            Destroy(gameObject);
        }
    }

}
