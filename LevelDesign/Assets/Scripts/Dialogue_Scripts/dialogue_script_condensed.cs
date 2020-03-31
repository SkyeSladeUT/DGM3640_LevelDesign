using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class dialogue_script_condensed : MonoBehaviour
{
    public Text Dialouge_Text;
    public GameObject Dialouge_Object;
    public NPCObject NPC;
    private bool ConvStart;
    private string _text_to_display;
    public UnityEvent OnFinish;
    private float textScrollSpeed = .01f;
    private int _actionIndex;
    private WaitForSeconds scrollwait, delaywait;
    private WaitUntil interactKeyWait;
    
    private void Awake()
    {
        scrollwait = new WaitForSeconds(textScrollSpeed);
        delaywait = new WaitForSeconds(.01f);
        interactKeyWait = new WaitUntil(() => Input.GetButtonDown("Interact"));
        ConvStart = false;
        Dialouge_Text.text = "";
        Dialouge_Object.SetActive(false);
    }

    public void StartConv()
    {
        if (!ConvStart){
            ConvStart = true;
            Dialouge_Object.SetActive(true);
            StartCoroutine(ScrollText());
        }
    }
    
    public IEnumerator ScrollText()
    {
        for (int i = 0; i < NPC.dialogue.lines.Count; i++)
        {
            _text_to_display = "";
            
            for (int j = 0; j < NPC.dialogue.lines[i].Length; j++)
            {
                _text_to_display += NPC.dialogue.lines[i][j];
                Dialouge_Text.text = _text_to_display;
                yield return scrollwait;
                if (Input.GetButtonDown("Interact"))
                {
                    Dialouge_Text.text = NPC.dialogue.lines[i];
                    break;
                }
            }
            yield return delaywait;
            yield return interactKeyWait;
        }
        Dialouge_Object.SetActive(false);
        OnFinish.Invoke();
        ConvStart = false;
    }

    public void CloseDialogue()
    {
        Dialouge_Object.SetActive(false);
        ConvStart = false;
    }


}
