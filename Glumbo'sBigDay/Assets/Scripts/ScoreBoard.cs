using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public Transform player;
    public int score;
    // Update is called once per frame
    void Update()
    {
        /*
        if (player.playerAttack.OnCollisionEnter);
        {
            score++;
        }
        */
        Debug.Log(score);
    }
}
