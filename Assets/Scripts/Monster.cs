using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

	public delegate void KillHandler(Monster monster);
	public event KillHandler OnKill;

	public float speed = 2f;
	public float horizontalLimit = 2.8f;
	public int tileSize = 32;
	public Monster next;

	private float movingDirection = 1f;

	// Use this for initialization
	void Start()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movingDirection, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (movingDirection > 0 && transform.position.x > horizontalLimit)
		{
			MoveDown();
		}
		else if (movingDirection < 0 && transform.position.x < -horizontalLimit)
		{
			MoveDown();
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (otherCollider.tag == "Box")
		{
			MoveDown();
		}
		else if (otherCollider.tag == "Bullet")
		{
			Destroy(otherCollider.gameObject);
			if (OnKill != null)
			{
				OnKill(this);
			}
		}
	}

	void MoveDown()
	{
		movingDirection *= -1;
		transform.position = new Vector2(transform.position.x, transform.position.y - tileSize / 100f);
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movingDirection, 0);
	}

	public void ChangeDirection()
	{
		movingDirection *= -1;
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movingDirection, 0);
	}
}
