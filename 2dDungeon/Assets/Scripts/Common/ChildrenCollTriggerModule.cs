using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenCollTriggerModule : MonoBehaviour
{
    private IChildOnTriggerEnter script;
    private void Awake()
    {
        script = transform.parent.GetComponent<IChildOnTriggerEnter>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (script != null)
        {
            script.ChildOnTriggerEnter(gameObject, other);
        }
    }
}
