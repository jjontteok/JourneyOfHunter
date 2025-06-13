using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace extension
{
    public static class Extension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

        // * GameObject 배열을 주어진 리스트에 복사 (주의 : 기존 데이터는 삭제)
        public static void ToList<T>(this T[] goArr, Dictionary<string, T> list) where T : UnityEngine.Object
        {
            list.Clear();
            foreach (T go in goArr)
            {
                list.Add(go.name, go);
            }
        }

        public static bool NullCheck<T>(params T[] tArr)
        {
            foreach(T t in tArr)
            {
                if (t == null)
                {
                    Debug.Log(typeof(T) + " : Null Reference Exception");
                    return true;
                }
            }
            return false;
        }

        //public static bool CheckSimilarColor(Color color1, Color color2, float threshold = 0.35f)
        //{
        //    Vector3 vec1 = new Vector3(color1.r, color1.g, color1.b);
        //    Vector3 vec2 = new Vector3(color2.r, color2.g, color2.b);

        //    if (Vector3.Distance(vec1, vec2) < 0.001f)
        //        return false;
        //    return true;
        //}

        public static bool CheckSimilarColor(float color1, float color2, float threshold = 0.001f)
        {
            if (Mathf.Abs(color1 - color2) < 0.001f)
                return false;
            return true;
        }
    }
}