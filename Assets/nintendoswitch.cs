using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nintendoswitch : MonoBehaviour
{
    private bool whiteIsActive = true;
    private List<GameObject> whitePlatforms = new List<GameObject>();
    private List<GameObject> blackPlatforms = new List<GameObject>();

    void Start()
    {
        whitePlatforms.AddRange(GameObject.FindGameObjectsWithTag("White"));
        blackPlatforms.AddRange(GameObject.FindGameObjectsWithTag("Black"));

        foreach (GameObject platform in blackPlatforms)
        {
            if (platform != null)
                platform.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            whiteIsActive = !whiteIsActive;

            foreach (GameObject platform in whitePlatforms)
            {
                if (platform != null)
                    platform.SetActive(whiteIsActive);
            }

            foreach (GameObject platform in blackPlatforms)
            {
                if (platform != null)
                    platform.SetActive(!whiteIsActive);
            }
        }
    }
}
