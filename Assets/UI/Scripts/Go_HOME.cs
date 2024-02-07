using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HOME : MonoBehaviour
{
    // Start is called before the first frame update
    public void BACK()
    {
        SceneManager.LoadScene("HOME");
    }
}
