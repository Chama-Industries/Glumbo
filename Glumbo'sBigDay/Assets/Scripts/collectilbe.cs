using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectalbe : MonoBehaviour
{
    public int itemScoreValue = 0;
    private baselineScore playerScore = new baselineScore();
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "player")
        {
            playerScore.addScore(itemScoreValue);
            Destroy(this.gameObject);
        }
    }
}
