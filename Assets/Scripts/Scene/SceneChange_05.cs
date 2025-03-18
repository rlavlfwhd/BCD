using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



    // Start is called before the first frame update
    public class SceneChange_05 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void Change()
        {
            SceneManager.LoadScene("04.Choicestory&record Scene");

        }
    }
