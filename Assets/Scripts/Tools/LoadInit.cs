using Const;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools
{
    public class LoadInit : MonoBehaviour
    {
        static public bool loaderUsed = false;

        private void Awake()
        {
#if UNITY_EDITOR
            if (!loaderUsed)
            {
                loaderUsed = true;
                SceneManager.LoadScene(SceneId.INIT);
            }
#endif
        }
    }
}