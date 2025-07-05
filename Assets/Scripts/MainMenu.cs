using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(true);

            settingsMenu.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void OpenSettings()
    {
        mainMenu.SetActive(false);

        settingsMenu.SetActive(true);
    }
}
