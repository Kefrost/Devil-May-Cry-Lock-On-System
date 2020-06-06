using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllEnemies : MonoBehaviour
{
    public List<GameObject> Enemies { get; set; }

    private void Start()
    {
        Enemies = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy spoted");

        if (other != null && other.gameObject.CompareTag("Enemy"))
        {
            Enemies.Add(other.gameObject);
            Debug.Log("Enemy added");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Enemy"))
        {
            Enemies.Remove(other.gameObject);
        }
    }
}
