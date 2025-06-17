using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UJ.Attributes;
using UJ.DI;

public class DifficultyButton : MonoBehaviour  //난이도 조절 스크립트
{
    private Button button;
    private GameManager gameManager;
    public int difficulty;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty); //버튼 클릭 시 SetDifficulty 메서드 실행
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();  //게임 매니저 스크립트 참조
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetDifficulty()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in DifficultyButton");
            return;
        }
        
        Debug.Log(button.gameObject.name + " 선택 ");
        gameManager.StartGame(difficulty); //파라미터로 난이도 조절
    }
}
