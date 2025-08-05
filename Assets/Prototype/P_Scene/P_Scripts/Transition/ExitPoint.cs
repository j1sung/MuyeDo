using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    [SerializeField]
    private string transferSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_instance_JS.instance.saveSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(transferSceneName);
        }
    }
}
