using Levels;
using UnityEngine;

namespace UI
{
    public class SkipLevelButton: MonoBehaviour
    {
        public void SkipLevel()
        {
            LevelsManager.Instance.SkipLevel();
        }
    }
}