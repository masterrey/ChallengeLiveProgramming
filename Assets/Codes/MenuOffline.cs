using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOffline : MonoBehaviour
{
    public void PlayLocalVersus()
    {
        SceneManager.LoadScene("GameLocalVS");
    }
    public void PlayLocalSigle()
    {
        SceneManager.LoadScene("GameLocalSingle");
    }
}
