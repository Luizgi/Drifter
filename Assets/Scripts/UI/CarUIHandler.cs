using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUIHandler : MonoBehaviour
{
    [Header("Car Details")]
    public Image carImg;

    Animator anim = null;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetupCar(CarData carData)
    {
        carImg.sprite = carData.CarUISprite;
    }

    public void StartCarEntranceAnimation(bool isAppearingOnRightSide)
    {
        if (isAppearingOnRightSide)
        {
            anim.Play("Appear From Right");
        }
        else
        {
            anim.Play("Appear From Left");
        }
    }

    public void StartCarExitAnimation(bool isExitingOnRightSide)
    {
        if(isExitingOnRightSide)
        {
            anim.Play("Disappear To Right");
        }
        else
        {
            anim.Play("Disappear To Left");
        }
    }

    //Events
    public void OnCarExitAnimationCompleted()
    {
        Destroy(gameObject);
    }
}
