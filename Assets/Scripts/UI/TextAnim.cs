using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    public Animation Anim;
    public Text Text;

    internal void ChangeText(string text)
    {
        var last = Text.text;
        if (last != text)
            Anim.Play();
        Text.text = text;
    }
}
