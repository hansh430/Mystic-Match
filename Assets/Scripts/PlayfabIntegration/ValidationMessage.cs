using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationMessage : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(DisableValidationMessage), 2f);
    }
    private void DisableValidationMessage()
    {
        gameObject.SetActive(false);
    }
}

