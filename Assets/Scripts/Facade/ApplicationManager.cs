using Const;
using Facade;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private static ApplicationManager _instance;
    private static bool _inited;
    
    [SerializeField]
    private SceneTransitionManager _sceneTransitionManager;
    [SerializeField]
    private CustomActionManager _customActionManager;
    [SerializeField]
    private SoundManager _soundManager;
    
    public static ApplicationManager Instance
    {
        get
        {
            if (!_inited)
            {
                _instance = FindObjectOfType<ApplicationManager>();
                _inited = true;
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (!_inited)
        {
            _instance = GetComponent<ApplicationManager>();
        }

        DontDestroyOnLoad(gameObject);
        _inited = true;
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
        
       _sceneTransitionManager.LoadScene(SceneId.MAIN_MENU);
    }

    public SceneTransitionManager SceneTransitionManager
    {
        get
        {
            return _sceneTransitionManager;
        }
    }

    public CustomActionManager CustomActionManager
    {
        get
        {
            return _customActionManager;
        }
    }

    public SoundManager SoundManager
    {
        get
        {
            return _soundManager;
        }
    }
}
