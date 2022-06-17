using System;
using System.Collections.Generic;

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
            return list[0];
        }
        else
        {
            throw new ArgumentNullException(nameof(list));
        }
    }
}