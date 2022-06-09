using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        if (this.gameObject.name == "ExitGameButton")
        {
            btn.onClick.AddListener(ExitGame);
        }
        if (this.gameObject.name == "StartGameButton")
        {
            btn.onClick.AddListener(StartGame);
        }
        Debug.Log(this.tag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        PlayerControl.NewGame = true;
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
