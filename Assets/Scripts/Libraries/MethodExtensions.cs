﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MethodExtensions
{
    public static T Random<T>(this IList<T> list)
    {
        if (list.Count > 1)
        {
            System.Random r = new System.Random();

            int randomIndex = r.Next(list.Count);

            var returnValue = list[randomIndex];

            if (returnValue == null)
            {
                return list[0];
            }
        
            return returnValue;
        }
        else if(list.Count == 1)
        {
            return list.FirstOrDefault();
        }
        else
        {
            throw new ArgumentNullException(nameof(list));
        }
    }
    
    public static IEnumerator WaitForAnimation (this Animator animator)
    {
        while(animator.AnimatorIsPlaying())
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public static bool AnimatorIsPlaying(this Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }

}