using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextDisplayer : MonoBehaviour
{
    [SerializeField]
    float secondsBetweenCharacters = .05f;

    TMP_Text textComponent;
    string nextText;
    bool readyToDisplay = false;

    // Start is called before the first frame update
    void Start()
    {
        //Capture the text and remove it before it is displayed so we can display one character at a time
        textComponent = GetComponent<TMP_Text>();
        nextText = textComponent.text;
        textComponent.text = "";
        readyToDisplay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToDisplay)
        {
            StartCoroutine(DisplayText());
            readyToDisplay = false;
        }
    }

    IEnumerator DisplayText()
    {
        foreach(char c in nextText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(secondsBetweenCharacters);
        }
    }
}
