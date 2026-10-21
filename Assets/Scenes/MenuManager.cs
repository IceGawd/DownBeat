using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject exitPanel;
    [SerializeField] GameObject goodbyePanel;
    [SerializeField] float goodbyeDuration = 2f;

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
        StartCoroutine(QuitAfterDelay());
    }

    IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSecondsRealtime(goodbyeDuration);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
