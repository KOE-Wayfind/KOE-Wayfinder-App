using System;
using System.Collections;
using System.Collections.Generic;
using LottiePlugin.UI;
using UnityEngine;

public class MyLottieAnimatedController : MonoBehaviour
{
    private AnimatedImage _animatedImage;

    private void Awake()
    {
        _animatedImage = GetComponent<AnimatedImage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _animatedImage.Play();
    }

    private void OnEnable()
    {
        _animatedImage.Play();
    }

    private void OnDisable()
    {
        _animatedImage.Stop();
    }
}
