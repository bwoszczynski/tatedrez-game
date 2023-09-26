using System.Collections;
using System.Collections.Generic;
using Const;
using Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Facade
{
    public class SceneTransitionManager : MonoBehaviour
    {
        private string _currentSceneId = SceneId.INIT;
        
        public void LoadScene(string sceneId)
        {
            StartCoroutine(LoadSceneCoroutine(sceneId));   
        }
        
        private IEnumerator LoadSceneCoroutine(string sceneId)
        {
            _currentSceneId = sceneId;
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(_currentSceneId);

            while(!loadSceneAsyncOperation.isDone)
            {
                yield return null;
            }

            Scene loadedScene = SceneManager.GetSceneByName(_currentSceneId);

            List<GameObject> rootObjects = new ();
            loadedScene.GetRootGameObjects(rootObjects);

            foreach (GameObject rootObject in rootObjects)
            {
                ISceneController sceneController = rootObject.GetComponent<ISceneController>();
                if (sceneController != null)
                {
                    sceneController.Init();
                    yield return null; 
                }
            }

        }
    }
}