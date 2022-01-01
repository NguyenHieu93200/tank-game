using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IpField : MonoBehaviour
{
    public GameObject Field;
    // Start is called before the first frame update
    void Start()
    {
        Field.GetComponent<InputField>().text = "127.0.0.1";
    }
}
