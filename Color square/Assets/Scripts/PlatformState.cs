using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformState : MonoBehaviour
{
    [SerializeField] private int platformState;

    private ChangePlatformSprite[] platformComponents;

    void Start()
    {
        setPlatformState(platformState);
    }

    void getPlatformComponents()
    {
        platformComponents = GetComponentsInChildren<ChangePlatformSprite>();
    }

    public int getPlatformState()
    {
        return platformState;
    }

    public void setPlatformState(int state)
    {
        platformState = state;
        getPlatformComponents();
        setPlatformColorFromState();
    }

    void setPlatformColorFromState()
    {
        foreach (ChangePlatformSprite component in platformComponents)
        {
            component.setPlatformComponentColor(platformState);
        }
    }
}
