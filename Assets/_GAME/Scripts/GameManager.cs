using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviourDontDestroy<GameManager>{
    private const string CONFIG_GAMEPLAY_SCENE = "GAME_PLAY";
    private void Start() {
        LoadScene();
    }
    [ContextMenu(nameof(LoadScene))]
    private void LoadScene() {
        SceneManager.LoadScene(CONFIG_GAMEPLAY_SCENE);
    }
}
