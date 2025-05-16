using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    Queue<SkillTest> skillQueue = new Queue<SkillTest>();
    Dictionary<SkillTest,GameObject> skillDictionary = new Dictionary<SkillTest,GameObject>();
    [SerializeField] List<SkillTest> skillList;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        if(skillQueue.Count > 0)
        {
            //skillQueue.Dequeue().
        }
    }
}
