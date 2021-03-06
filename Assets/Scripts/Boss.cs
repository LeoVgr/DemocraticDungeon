﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{
    private Character target = null;

    protected override void Start()
    {
        base.Start();
        Exposed = true;
    }

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Action0();
                DropAction(0);
                break;
            case 1:
                Action1();
                DropAction(1);
                break;
            case 2:
                Action2();
                DropAction(2);
                break;
            case 3:
                Action3();
                DropAction(3);
                break;
            default:
                Busy = false;
                Debug.Log("Wrong index " + index);
                break;
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void Action0()
    {
        Busy = false;
    }

    private void Action1()
    {
        Busy = false;
    }

    private void Action2()
    {
        Busy = false;
    }
    private void Action3()
    {
        Busy = false;
    }

    public void Taunt(Character character)
    {
        target = character;
    }

}
