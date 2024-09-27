using System;
using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

namespace Field
{
    public class BuilderBubleData
    {
        public IReadOnlyList<BubleData> GetData(string text)
        {
            var values = text.Split(' ');
            for (var i = 0; i < values.Length; i++)
            {
                Debug.Log(values[i]);
            }

            Debug.Log(text);
            return null;
        }
    }
}