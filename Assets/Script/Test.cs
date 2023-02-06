using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator coco()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        yield return null;
    }
    // Update is called once per frame
    void OnEnable()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(coco());
    }
}
