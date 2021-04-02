using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Menu Transition", menuName = "Make New Transition", order = 0)]
    public class MenuTransition : ScriptableObject
    {
        public string currentScreen, transitionScreen, selectGameObject;
    }
}