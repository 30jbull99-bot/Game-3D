using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EclipseProtocol.Managers
{
    public class GameModeManager : MonoBehaviour
    {
        public static GameModeManager Instance { get; private set; }

        [SerializeField] private string campaignScene = "CampaignHub";
        [SerializeField] private string survivalScene = "SurvivalArena";
        [SerializeField] private string mapEditorScene = "MapEditor";

        private GameMode currentMode;

        public event Action<GameMode> OnGameModeChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadGameMode(GameMode mode)
        {
            currentMode = mode;
            string sceneToLoad = GetSceneName(mode);
            SceneManager.LoadScene(sceneToLoad);
            OnGameModeChanged?.Invoke(mode);
        }

        public GameMode GetCurrentMode() => currentMode;

        private string GetSceneName(GameMode mode)
        {
            return mode switch
            {
                GameMode.Campaign => campaignScene,
                GameMode.Survival => survivalScene,
                GameMode.MapEditor => mapEditorScene,
                _ => campaignScene
            };
        }
    }
}
