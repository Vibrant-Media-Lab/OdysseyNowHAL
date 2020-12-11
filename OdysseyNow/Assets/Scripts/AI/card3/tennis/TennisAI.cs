using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TennisAI : MonoBehaviour {
    public GameObject p1;
    public GameObject p2;
    public GameObject ball;
    public int p1Level;
    public int p2Level;
    public int players;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (players) {
            case 1:
                // player 1 is AI
                updatePlayer(p1, p2, ball, p1Level);
                break;
            case 2:
                // player 2 is AI
                updatePlayer(p2, p1, ball, p2Level);
                break;
            case 3:
                // both players are AI
                updatePlayer(p1, p2, ball, p1Level);
                updatePlayer(p2, p1, ball, p2Level);
                break;
            default:
                Debug.Log("Unknown players value: " + players);
                break;
        }
    }

    private void updatePlayer(GameObject player, GameObject opponent, GameObject ball, int level) {
        GameObject pTarget = player.transform.Find("PlayerTarget").gameObject;
        GameObject oTarget = opponent.transform.Find("PlayerTarget").gameObject;
        switch (level) {
            case 1:
                // base player
                pTarget.transform.position = new Vector3(pTarget.transform.position.x, Random.Range(-4, 4), 0);
                //pTarget.GetComponent<NavMeshAgent>().SetDestination(new Vector3(0, Random.Range(-4, 4), 0));
                break;
            case 2:
                // AI moves slightly faster
                break;
            case 3:
                // AI uses engish but doesn't try to trick the player
                break;
            case 4:
                // AI moves even faster
                break;
            case 5:
                // AI tries to trick player with english
                break;
            default:
                Debug.Log("Unknown level value: " + level);
                break;
        }
    }
}
