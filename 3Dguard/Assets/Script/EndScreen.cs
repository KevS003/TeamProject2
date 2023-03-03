using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public GameObject playerReference;
    PlayerMove playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerReference.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.ending>0)
        {
            win();
        }
        else if(playerScript.ending<0)
        {
            lose();
        }
    }
    void win()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        //turn on win UI here change name if needed
        //winUI.SetActive(true);
    }
    void lose()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        //turn on lose UI here change name accordingly 
        //loseUI.SetActive(true);
    } 
}
