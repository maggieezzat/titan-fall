using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitFallScript : MonoBehaviour
{
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("hola");
        if(other.gameObject.tag.Equals("Player")){
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
