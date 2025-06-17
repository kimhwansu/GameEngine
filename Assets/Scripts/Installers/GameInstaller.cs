using UnityEngine;
using UJ.DI;
using TMPro;
using UnityEngine.UI;
using UJ.Data;

namespace UJ.Installers
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button restartButton;
        [SerializeField] private GameObject titleScreen;
        [SerializeField] private DifficultyButton[] difficultyButtons;
        [SerializeField] public TargetDataList targetDataList;

        private void Awake()
        {
            var container = new DIContainer();
            DIContainer.AddContainer(container);

            // Register core game components
            RegisterCoreComponents(container);
            RegisterUIComponents(container);
            RegisterDifficultyButtons(container);
        }

        private void RegisterCoreComponents(DIContainer container)
        {
            if (gameManager != null)
            {
                container.Regist(gameManager);
            }

            if (targetDataList != null)
            {
                foreach (var data in targetDataList.targets)
                {
                    container.Regist(data, data.prefabName);
                }
            }
        }

        private void RegisterUIComponents(DIContainer container)
        {
            if (scoreText != null)
            {
                container.Regist(scoreText, "scoreText");
            }
            if (gameOverText != null)
            {
                container.Regist(gameOverText, "gameOverText");
            }
            if (restartButton != null)
            {
                container.Regist(restartButton, "restartButton");
            }
            if (titleScreen != null)
            {
                container.Regist(titleScreen, "titleScreen");
            }
        }

        private void RegisterDifficultyButtons(DIContainer container)
        {
            if (difficultyButtons != null)
            {
                foreach (var button in difficultyButtons)
                {
                    if (button != null)
                    {
                        container.Regist(button, $"difficultyButton_{button.difficulty}");
                    }
                }
            }
        }

        private void OnDestroy()
        {
            DIContainer.RemoveContainer(DIContainer.diContainers[0]);
        }
    }
}
