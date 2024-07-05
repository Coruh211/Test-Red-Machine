using System;
using Events;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviourSingleton<LevelsManager>
    {
        public static event Action LevelCompleteEvent;
        private const string LevelNamePattern = "Level{0}";

        private int _currentLevelIndex;
        
        private void Start()
        {
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));

            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            LevelCompleteEvent?.Invoke();
            _currentLevelIndex += 1;
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
        }

        public void SkipLevel()
        {
            LevelCompleteEvent?.Invoke();
            _currentLevelIndex += 1;
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
        }
    }
}