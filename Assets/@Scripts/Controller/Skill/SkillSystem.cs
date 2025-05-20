using System;
using System.Collections.Generic;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //스킬 데이터 리스트
    [SerializeField] List<SkillData> skillDataList;

    //플레이어의 스킬 리스트 - 스킬 슬롯
    List<Skill> _skillList = new List<Skill>();

    // 스킬 슬롯 리스트
    List<SkillSlot> _slotList = new List<SkillSlot>();

    private void Start()
    {
        // skillDataList에서 데이터를 가져와 스킬 슬롯에 등록한다
        foreach(var data in skillDataList)
        {
            if (data.skillPrefab != null)
            {
                GameObject gameObject=new GameObject(data.skillPrefab.name);
                gameObject.transform.SetParent(transform);
                SkillSlot slot = gameObject.AddComponent<SkillSlot>();
                slot.SetSkill(data);
                _slotList.Add(slot);

                ////data에 해당하는 스킬 프리팹을 복제
                //GameObject skillObject = Instantiate(data.skillPrefab, transform);
                //skillObject.SetActive(false);
                //Skill skillComponent = skillObject.GetComponent<Skill>();
                //if (skillComponent != null)
                //{
                //    skillComponent.Initialize(data);
                //    _skillList.Add(skillComponent);
                //}
            }
        }
        //InitializeSkill();
    }

    public void TestAttack()
    {
        //_skillList[0]?.StartAttack();
        //SkillAttack();
    }


    //#region Skill Queue
    //Queue<GameObject> _skillQueue = new Queue<GameObject>();
    //Dictionary<Skill, GameObject> _skillDictionary = new Dictionary<Skill, GameObject>();

    //[SerializeField] SkillData[] skillList;
    //public static Action<GameObject> OnEnqueueSkill;
    //public static Action<SkillTest> OnSkillCoolTime;

    //void InitializeSkill()
    //{
    //    OnEnqueueSkill += EnqueueSkill;
    //    //skillList = Resources.LoadAll<SkillTest>("");
    //    for (int i = 0; i < skillList.Length; i++)
    //    {
    //        var skillObject = Instantiate(skillList[i], transform).skillPrefab;
    //        skillObject.GetComponent<Skill>()?.Initialize(skillList[i]);
    //        _skillQueue.Enqueue(skillObject);
    //    }
    //}

    //void EnqueueSkill(GameObject skill)
    //{
    //    _skillQueue.Enqueue(skill);
    //}

    //void SkillAttack()
    //{
    //    //if (_skillQueue.Count > 0 && !_animator.GetBool(Define.IsAttacking))
    //    if(_skillQueue.Count > 0)
    //    {
    //        _skillQueue.Dequeue().GetComponent<Skill>().StartAttack();
    //    }
    //}

    //public void AddDictionary(GameObject skill)
    //{
    //    _skillDictionary.Add(skill.GetComponent<Skill>(), skill);
    //}

    //public void RemoveDictionary(GameObject skill)
    //{
    //    _skillDictionary.Remove(skill.GetComponent<Skill>());
    //    _skillQueue.Enqueue(skill);
    //}
    //#endregion
}
