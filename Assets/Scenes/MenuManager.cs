using System.Collections;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject exitPanel;
    [SerializeField] GameObject goodbyePanel;
    [SerializeField] TMP_Text goodbyeText;
    [SerializeField] float typeSpeed = 0.1f;      // seconds per letter
    [SerializeField] float pauseAfterTyping = 1f; // wait before quitting

    void Start()
    {
        exitPanel.SetActive(false);
        goodbyePanel.SetActive(false);
    }

    public void OpenExitPanel()
    {
        exitPanel.SetActive(true);
        exitPanel.transform.SetAsLastSibling();
    }

    public void CloseExitPanel()
    {
        exitPanel.SetActive(false);
    }

    public void ConfirmExit()
    {
        exitPanel.SetActive(false);
        goodbyePanel.SetActive(true);
        goodbyePanel.transform.SetAsLastSibling();
        StartCoroutine(TypeThenQuit());
    }

    IEnumerator TypeThenQuit()
    {
        goodbyeText.text = "GOODBYE :(";
        goodbyeText.maxVisibleCharacters = 0;

        for (int i = 1; i <= goodbyeText.text.Length; i++)
        {
            goodbyeText.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        yield return new WaitForSecondsRealtime(pauseAfterTyping);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}