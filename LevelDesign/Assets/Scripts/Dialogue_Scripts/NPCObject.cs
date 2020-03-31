using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/NPC")]
public class NPCObject : ScriptableObject
{
    public dialogue_lines dialogue;

    public void SwapLines(dialogue_lines newLines)
    {
        dialogue = newLines;
    }
    
}
