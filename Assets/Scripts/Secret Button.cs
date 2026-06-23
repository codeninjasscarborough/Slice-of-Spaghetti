using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretButton : MonoBehaviour
{
    public bool eatEggs = false;
    public bool eatBacon = false;
    public bool onMouseClicked = false;

    // Start is called before the first frame update
    void Start()
    { if (onMouseClicked == true) {
            if (eatEggs == false)
            {
                eatEggs = true;
                Debug.Log("Hi");
            }

            if (eatEggs == true)
            {
                eatEggs = false;
                Debug.Log("Bye");
            }

            if (eatBacon == false)
            {
                eatBacon = true;
                Debug.Log(":)");
            }

            if (eatBacon == true)
            {
                eatBacon = false;
                Debug.Log(":(");
            }

        }

    }

    public float speed = 5.0f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(moveX, moveY);

        // Move the transform along the X and Y axes
        transform.Translate(direction * speed * Time.deltaTime);

    }

    public void OnClick()
    {
        transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime;
    }
}
