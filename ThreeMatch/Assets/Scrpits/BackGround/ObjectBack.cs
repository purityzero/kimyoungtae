using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBack : MonoBehaviour
{
    public bool IsEmpty { get; private set; }
    
    public void Start()
    {
        StartCoroutine(coEmptyCheck());
    }

    private IEnumerator coEmptyCheck()
    {
        while (true)
        {
            yield return null;
            if (!GetComponentInChildren<MatchBlock>())
            {
                IsEmpty = true;
            }
            else IsEmpty = false;
        }
    }
}
