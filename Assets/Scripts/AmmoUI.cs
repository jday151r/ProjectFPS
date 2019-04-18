﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 imageDimensions;
    private GameManager GM;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rectTransform = GetComponent<RectTransform>();
        imageDimensions = new Vector2(GetComponent<Image>().sprite.texture.width, GetComponent<Image>().sprite.texture.height);
    }
    
    void Update()
    {
        rectTransform.sizeDelta = new Vector2(imageDimensions.x * GM.clip, imageDimensions.y);
    }
}