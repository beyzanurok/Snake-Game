using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Player player;
    public GameObject boxPrefab;
    public int tileSize = 32;
    public float boxChance = 0.1f;
    public GameObject monsterPrefab;
    public int monsterSize = 5;

    private List<Monster> monsters;
    private bool killedMonster; 

    // Start is called before the first frame update
    void Start()
    {
        monsters = new List<Monster>();

       // GameObject boxInstance = Instantiate(boxPrefab);
       // objeye bakmak için ekledim

        //dikey olarak bölme yaptım
        for (int y = Screen.height / 2 - tileSize; y > -Screen.height / 2 + tileSize * 4; y -= tileSize)
        {
            //aynı işlemde yatayda da yapmam gerek
            for (int x = Screen.width / 2 - tileSize; x > -Screen.width / 2 + tileSize; x -= tileSize)
            {
                //kutu somutlaştırma
                if (Random.value < boxChance)
                {
                    GameObject boxInstance = Instantiate(boxPrefab);
                    boxInstance.transform.SetParent(transform);
                    boxInstance.transform.position = new Vector2((x - tileSize / 2) / 100f, (y - tileSize / 2) / 100f);
                }
            }
        }

        Monster previousMonster = null;
        for (int i =0; i < monsterSize; i++)
        {
            GameObject monsterInstance = Instantiate(monsterPrefab);
            monsterInstance.transform.SetParent(transform);
            monsterInstance.transform.position = new Vector2(-i * (tileSize / 100f), (Screen.height / 2 - tileSize / 2) / 100f);

            Monster monster = monsterInstance.GetComponent<Monster>();
            monster.OnKill += OnMonsterKill;

            if (previousMonster != null)
            {
                previousMonster.next = monster;
            }
            else
            {
                monsters.Add(monster);
            }

            previousMonster = monster;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            SceneManager.LoadScene("Game");
        }
        if (monsters.Count == 0)
        {
            SceneManager.LoadScene("Game");
        }

        killedMonster = false;
    }

    void OnMonsterKill(Monster monster)
    {
        if (killedMonster == true)
        {
            return;
        }

        killedMonster = true;

        Monster currentMonster = monster;
        if (monster.next != null)
        {
            List<Monster> monsterString = new List<Monster>();
            while (currentMonster.next != null)
            {
                monsterString.Add(currentMonster);
                currentMonster.ChangeDirection();
                currentMonster = currentMonster.next;
            }
            monsterString.Add(currentMonster);

            for (int i = monsterString.Count - 1; i > 0; i--)
            {
                monsterString[i].next = monsterString[i - 1];
            }
            monsterString[0].next = null;

            currentMonster.ChangeDirection();
            monsters.Add(currentMonster);
        }

        if (monsters.IndexOf(monster) != -1)
        {
            monsters.Remove(monster);
        }

        Destroy(monster.gameObject);
    }
}
