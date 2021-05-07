using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMonsterUIManager : MonoBehaviour
{
    [SerializeField]
    private Text _capsuleText;

    [SerializeField]
    private Text _cubeText;

    [SerializeField]
    private Text _sphereText;

    [SerializeField]
    private Text[] _texts;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCount(int shape, int count) {
        _texts[shape].text = count.ToString();
    }
}
