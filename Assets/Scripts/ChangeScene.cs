﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public void goToGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
