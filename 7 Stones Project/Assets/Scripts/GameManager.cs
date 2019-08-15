using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Text;
using System;

[System.Serializable]
public class Position
{
    public Vector3 stonesPosition;
    public Vector3 ballPosition;
}

public class GameManager : MonoBehaviour
{
    public Position[] position;
    public static int chances = 0;
    public static bool gameRunning = false;
    public GameObject stones;
    public GameObject ball;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject panel;

    private int previousScore = 0;
    private static float timer = 0f;
    private List<string[]> rowData = new List<string[]>();

    private void Start()
    {
        StartCoroutine(InstantiateGameObject(0.5f));
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(timer);

            AddRecord(timer);
            StartCoroutine(SaveScore());
        }

        timer += Time.deltaTime;
    }

    public IEnumerator InstantiateGameObject(float waitTime)
    {
        if (chances < 7)
        {
            yield return new WaitForSeconds(waitTime);
            Instantiate(stones, position[0].stonesPosition, Quaternion.identity);
            Instantiate(ball, position[0].ballPosition, Quaternion.identity);
            gameRunning = true;
        }
        else
        {
            chances = 0;
            StartCoroutine("LoadScene");
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        scoreText.gameObject.SetActive(false);
        panel.SetActive(true);
        panel.GetComponentInChildren<TextMeshProUGUI>().text = "Score: " + score;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Menu");
    }

    public static void AddRecord(float timeStamp)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/" + "TimeStamps.csv", true))
            {
                file.WriteLine(timeStamp);
            }
        }
        catch(Exception ex)
        {
            throw new ApplicationException("This program did an oopsie: ", ex);
        }
    }

    private IEnumerator SaveScore()
    {
        yield return new WaitForSeconds(1.25f);
        int scoreToRecord = score - previousScore;
        previousScore = score;
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/" + "Score.csv", true))
            {
                file.WriteLine(scoreToRecord.ToString());
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("This program did an oopsie: ", ex);
        }
    }
}
