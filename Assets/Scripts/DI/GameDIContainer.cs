using UnityEngine;
using UJ.DI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameDIContainer : MonoBehaviour
{
    private void Awake()
    {
        var container = new DIContainer();
        DIContainer.AddContainer(container);

        // 게임 매니저 등록
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            // 모든 의존성 등록
            // 게임 매니저의 의존성 등록
            var scoreText = FindObjectOfType<TextMeshProUGUI>();
            var gameOverText = FindObjectOfType<TextMeshProUGUI>();
            var restartButton = FindObjectOfType<Button>();
            var titleScreen = FindObjectOfType<GameObject>();

            container.Regist(scoreText, "scoreText");
            container.Regist(gameOverText, "gameOverText");
            container.Regist(restartButton, "restartButton");
            container.Regist(titleScreen, "titleScreen");
            
            // 타겟 오브젝트들 찾기
            var targets = FindObjectsOfType<Target>();
            
            // 타겟 오브젝트들 리스트 등록
            List<GameObject> targetList = new List<GameObject>();
            foreach (var target in targets)
            {
                targetList.Add(target.gameObject);
            }
            container.Regist(targetList, "targets");

            // 게임 매니저 등록
            Debug.Log("GameManager을 찾았습니다: " + gameManager.name);
            container.Regist(gameManager, "GameManager");
            Debug.Log("GameManager이 DI 컨테이너에 등록되었습니다");
            
            // 게임 매니저 주입
            DIContainer.Inject(gameManager);
            Debug.Log("GameManager이 DI 컨테이너에서 주입되었습니다");
            
            // DifficultyButton 주입
            var difficultyButtons = FindObjectsOfType<DifficultyButton>();
            Debug.Log("발견된 DifficultyButton 개수: " + difficultyButtons.Length);
            foreach (var button in difficultyButtons)
            {
                Debug.Log("주입할 DifficultyButton: " + button.name);
                DIContainer.Inject(button);
            }
        }
    }

    private void OnDestroy()
    {
        DIContainer.RemoveContainer(DIContainer.diContainers[0]);
    }
}
