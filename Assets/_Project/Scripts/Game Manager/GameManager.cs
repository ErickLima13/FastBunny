using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //private PlayerPhysics playerPhysics;

    [Header("Player Settings")]
    [Range(1, 5)] public float speed;
    public float[] limits;
    public int munition;

    [Header("Scenery Settings")]
    public float objectsSpeed;
    public float limitDestruction;
    public float sizeBridge;
    public GameObject[] sceneryPrefab;

    [Header("Globals")]
    public int score;
    public float posXPlayer;
    public TextMeshProUGUI scoreText;

    [Header("Sfx")]
    public AudioSource audioSource;
    public AudioClip scoreSound;

    private void Initialization()
    {
        //playerPhysics = FindObjectOfType(typeof(PlayerPhysics)) as PlayerPhysics;
    }

    // Start is called before the first frame update
    void Start()
    {

        Initialization();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //posXPlayer = playerPhysics.transform.position.x;
    }

    public void AddScore(int value)
    {
        score += value;
        //scoreText.text = "Score: " + score.ToString();
        //audioSource.PlayOneShot(scoreSound);
    }

    public void ManagerMunition(int value)
    {
        munition += value;
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
