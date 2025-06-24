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

        public static bool CheckTwoValues(float value1, float value2, float threshold = 0.001f)
        {
            if (Mathf.Abs(value1 - value2) < threshold)
                return false;
            return true;
        }

        public static Define.TimeOfDayType GetNextType(Define.TimeOfDayType type)
        {
            return type switch
            {
                Define.TimeOfDayType.Noon => Define.TimeOfDayType.Evening,
                Define.TimeOfDayType.Evening => Define.TimeOfDayType.Night,
                Define.TimeOfDayType.Night => Define.TimeOfDayType.Morning,
                Define.TimeOfDayType.Morning => Define.TimeOfDayType.Noon,
                _ => 0
            };
        }
    }
}