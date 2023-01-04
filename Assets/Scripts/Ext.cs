using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace MountainInn
{
    static public class Ext
    {
        static private Canvas _canvas;
        static private Canvas canvas => _canvas ?? (_canvas = GameObject.FindObjectOfType<Canvas>());
           
        static public T GetRandom<T>(this List<T> list)
        {
            int id = UnityEngine.Random.Range(0, list.Count);

            return list[id];
        }

        static public Vector3 MousePositionScaledToCanvas()
        {
            return Input.mousePosition * canvas.transform.lossyScale.x;
        }
    }
}

static public class DoubleExt
{
    static public string BeautifulFormat(this double number)
    {
        double abs = System.Math.Abs(number);

        string str =
            (abs > 9_999_999f)
            ? abs.ToString("e2")
            : abs.ToString("N0", CultureInfo.InvariantCulture);


        return str ;
    }
}
