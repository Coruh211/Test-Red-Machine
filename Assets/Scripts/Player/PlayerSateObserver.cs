using System;
using Events;


namespace Player
{
    public class PlayerSateObserver
    {
        private readonly Action<PlayerState> _setStateAction;
        
        
        public PlayerSateObserver(Action<PlayerState> setStateAction)
        {
            _setStateAction = setStateAction;
        }

        public void Subscribe()
        {
            EventsController.Subscribe<EventModels.Game.NodeTapped>(this, OnNodeTapped);
            EventsController.Subscribe<EventModels.Game.PlayerFingerRemoved>(this, OnPlayerFingerRemoved);
            EventsController.Subscribe<EventModels.Game.PlayerScrolled>(this, OnPlayerScrolled);
        }
        
        public void Unsubscribe()
        {
            EventsController.Unsubscribe<EventModels.Game.NodeTapped>(OnNodeTapped);
            EventsController.Unsubscribe<EventModels.Game.PlayerFingerRemoved>(OnPlayerFingerRemoved);
            EventsController.Unsubscribe<EventModels.Game.PlayerScrolled>(OnPlayerScrolled);
        }

        private void OnNodeTapped(EventModels.Game.NodeTapped e)
        {
            _setStateAction?.Invoke(PlayerState.Connecting);
        }

        private void OnPlayerFingerRemoved(EventModels.Game.PlayerFingerRemoved e)
        {
            _setStateAction?.Invoke(PlayerState.None);
        }
        
        private void OnPlayerScrolled(EventModels.Game.PlayerScrolled obj)
        {
           _setStateAction?.Invoke(PlayerState.Scrolling);
        }
    }
}
