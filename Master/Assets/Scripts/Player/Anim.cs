using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    private SpriteRenderer sprite;
    private PlayerMovement moveScript;
    private int JumpsLeft;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        moveScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpsLeft = moveScript.GetJumpsLeft();

        if (moveScript.GetGrounded())
        {
            sprite.color = Color.red;
        }
        else
        {
            if (JumpsLeft == 1)
            {
                sprite.color = Color.magenta;
            }
            else
            {
                sprite.color = Color.blue;
            }
        }

    }
}
