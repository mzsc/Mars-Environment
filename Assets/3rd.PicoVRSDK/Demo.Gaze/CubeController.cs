using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour
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
    public void GazeEnter()
    {
        mMaterial.color = Color.red;
    }

    public void GazeExit()
    {
        mMaterial.color = Color.yellow;
    }
}
