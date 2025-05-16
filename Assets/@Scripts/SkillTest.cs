using UnityEngine;

public class SkillTest : MonoBehaviour
{
    bool _isCoolTime = false;
    float _currentTime = 0f;
    const float _coolTime = 4f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaitCool();
    }

    void WaitCool()
    {
        if(_isCoolTime)
        {
            _currentTime += Time.deltaTime;
            if(_currentTime>=_coolTime)
            {
                //PlayerTest.enqueue(this);
            }
        }
    }
}
