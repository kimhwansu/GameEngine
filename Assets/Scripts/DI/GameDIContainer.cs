using UnityEngine;
using UJ.DI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UJ.Data;
using UJ.Installers;

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
            
            // 게임 매니저 등록
            Debug.Log("GameManager을 찾았습니다: " + gameManager.name);
            container.Regist(gameManager, "GameManager");
            Debug.Log("GameManager이 DI 컨테이너에 등록되었습니다");
            
            // 게임 매니저 주입
            DIContainer.Inject(gameManager);
            Debug.Log("GameManager이 DI 컨테이너에서 주입되었습니다");
            
            // TargetDataList 등록
            var gameInstaller = FindObjectOfType<GameInstaller>();
            if (gameInstaller != null)
            {
                if (gameInstaller.targetDataList != null)
                {
                    Debug.Log("TargetDataList이 찾았습니다: " + gameInstaller.targetDataList.targets.Count + "개의 타겟 데이터");
                    container.Regist(gameInstaller.targetDataList, "TargetDataList");
                }
                else
                {
                    Debug.LogError("TargetDataList이 할당되지 않았습니다");
                }
            }
            
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

    private void Start()
    {
        // 게임 시작 시점에 타겟 오브젝트들 찾기
        var targets = FindObjectsOfType<Target>();
        
        // 타겟 오브젝트들 리스트 등록
        List<GameObject> targetList = new List<GameObject>();
        
        // DI 컨테이너 가져오기
        var container = DIContainer.diContainers[0];

        // 타겟 오브젝트들에 대해 TargetData 주입
        foreach (var target in targets)
        {
            // 타겟 프리팹의 이름으로 TargetData 찾기
            var targetData = FindTargetData(target.gameObject.name);
            if (targetData != null)
            {
                container.Regist(targetData, target.gameObject.name);
                
                // GameManager 주입
                var gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    container.Regist(gameManager, "GameManager");
                }
                
                // DIContainer.Inject 호출
                DIContainer.Inject(target);
                targetList.Add(target.gameObject);
            }
            else
            {
                Debug.LogError($"TargetData not found for prefab: {target.gameObject.name}");
            }
        }
        
        container.Regist(targetList, "targets");
    }

    private TargetData FindTargetData(string prefabName)
    {
        // TargetDataList에서 해당 이름의 TargetData 찾기
        var gameInstaller = FindObjectOfType<GameInstaller>();
        if (gameInstaller != null)
        {
            var targetDataList = gameInstaller.targetDataList;
            if (targetDataList != null)
            {
                // 프리팹 이름에서 (Clone) 제거
                string cleanPrefabName = prefabName.Replace("(Clone)", "");
                
                foreach (var data in targetDataList.targets)
                {
                    if (data.prefabName == cleanPrefabName)
                    {
                        Debug.Log($"Found TargetData for prefab: {prefabName} -> {data.prefabName}");
                        return data;
                    }
                }
            }
        }
        Debug.LogError($"TargetData not found for prefab: {prefabName}");
        return null;
    }

    private void OnDestroy()
    {
        DIContainer.RemoveContainer(DIContainer.diContainers[0]);
    }
}
