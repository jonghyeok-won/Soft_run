using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Click_Sound : MonoBehaviour
{
    public AudioSource clickSound;

    public void ClickSound()
    { 
        clickSound.Play(); 
    }

}
