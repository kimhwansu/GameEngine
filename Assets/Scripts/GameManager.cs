using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   //UI TextMeshPro 사용 시 필요함
using UnityEngine.SceneManagement;  //씬관리 시 필요함
using UnityEngine.UI; //버튼과 상호작용
using UJ.Attributes;
using UJ.DI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    public bool isGameActive;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject titleScreen;
    private int score;
    private float spawnRate = 1f;

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int Index = Random.Range(0, targets.Count);
            Instantiate(targets[Index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score : " + score;
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty) //파라미터로 난이도 조절
    {
        isGameActive = true;
        score = 0;

        spawnRate /= difficulty;  //easy(1) midium(2) hard(3)  ->  1/1=1초간격   1/2=0.5초간격  1/3=0.33초간격

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
    }
}
