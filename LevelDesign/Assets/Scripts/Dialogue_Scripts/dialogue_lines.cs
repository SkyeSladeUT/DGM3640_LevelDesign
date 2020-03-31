using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/Lines")]
public class dialogue_lines : ScriptableObject
{   
    [TextArea(3, 10)]
    public List<string> lines;
    
}
