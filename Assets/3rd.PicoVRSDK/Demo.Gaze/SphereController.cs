using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SphereController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    Material mMaterial;
    void Awake()
    {
        mMaterial = GetComponent<Renderer>().material;
    }
    void Start()
    {
        mMaterial.color = Color.yellow;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mMaterial.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mMaterial.color = Color.yellow;
    }
}
