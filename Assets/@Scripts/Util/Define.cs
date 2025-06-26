using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Define : MonoBehaviour
{
    #region Animator
    public readonly static int Idle = Animator.StringToHash("Idle");
    public readonly static int Attack = Animator.StringToHash("Attack");
    public readonly static int Spell = Animator.StringToHash("Spell");
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int Walk = Animator.StringToHash("Walk");

    public readonly static int CloseAttack = Animator.StringToHash("CloseAttack");
    public readonly static int LongAttack = Animator.StringToHash("LongAttack");
    public readonly static int IsInteractionPossible = Animator.StringToHash("IsInteractionPossible");

    public const string EndAttack = "EndAttack";
    public const string WalkSpeed = "WalkSpeed";
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string GroundTag = "Ground";
    public const string MonsterTag = "Monster";
    #endregion

    #region Path
    public const string PlayerPath = "Player/Player";
    public const string MonsterPath = "Monsters/Normal";
    public const string NamedMonsterPath = "Monsters/Named";
    public const string MonsterSkillPath = "Skills/MonsterSkills/SkillResources";
    public const string MonsterSkillHitEffectPath = "Skills/MonsterSkills/HitEffects";
    public const string PlayerSkillPath = "Skills/PlayerSkills/SkillResources";
    public const string PlayerSkillHitEffectPath = "Skills/PlayerSkills/HitEffects";
    public const string ShieldEffectPath = "Dungeon/Object/ShieldEffect";
    public const string DungeonWallPath = "Dungeon/Object/DungeonWall";
    public const string DungeonPortalPath = "Dungeon/Object/DungeonPortal";
    public const string MonsterGatePath = "Dungeon/Object/HellGate";
    public const string SkyBoxPath = "SkyBox";
    public const string BackgroundPath = "Environment/Background";
    public const string DamageTextPath = "UI/Text/DamageText";
    public const string SystemTextPath = "UI/Text/SystemText";
    public const string PlayerVitalCanvasPath = "UI/PlayerVital/UI_Vital";
    public const string PlayerVitalPath = "UI/PlayerVital/PlayerVital";

    public const string UIGamePath = "UI/MainUI/UI_Game";
    public const string PopupUICanvasPath = "UI/PopupUI/Canvas - Popup";
    public const string PopupPanelPath = "UI/PopupUI/Panel - Popup";
    public const string PopupStatusPanelPath = "UI/PopupUI/StatusPanel - Popup";
    public const string PopupInventoryPanelPath = "UI/PopupUI/InventoryPanel - Popup";
    public const string PopupSkillInventoryPath = "UI/PopupUI/UI_SkillInventory - Popup";
    public const string PopupGainedRecordPanelPath = "UI/PopupUI/GainedRecordPanel - Popup";
    public const string PopupAdventureInfoPanelPath = "UI/PopupUI/Panel - AdventureInfo";
    public const string PopupStageInfoPanelPath = "UI/PopupUI/Panel - StageInfo";
    public const string PopupNamedMonsterInfoPanelPath = "UI/PopupUI/Panel - NamedMonsterInfo";

    public const string GoblinKingCutScenePath = "CutScene/GoblinCutScene";
    public const string FollowCameraPath = "Camera/FollowPlayerCamera";
    public const string CutSceneCameraPath = "Camera/CutSceneCamera";
    #endregion

    #region DungeonSpots
    public static Vector3 FirstEnterSpot = new Vector3(0, 3, 0);
    public static Vector3 DungeonEnterSpot = new Vector3(0, 5, -35);
    public static Vector3 DungeonExitSpot = new Vector3(0, 5, 35);
    public static Vector3 DungeonEnterPortalSpot = new Vector3(0, 2.5f, 5);
    public static Vector3 DungeonExitPortalSpot = new Vector3(0, 2.5f, 34f);
    #endregion

    #region SpawnSpots
    public static Vector3 SpawnSpot1 = new Vector3(-7, 7, 0);
    public static Vector3 SpawnSpot2 = new Vector3(7, 7, 0);
    public static Vector3 SpawnSpot3 = new Vector3(-7, 7, 20);
    public static Vector3 SpawnSpot4 = new Vector3(7, 7, 20);
    public static Vector3 SpawnSpot5 = new Vector3(-7, 7, -20);
    public static Vector3 SpawnSpot6 = new Vector3(7, 7, -20);
    public static Vector3 NamedMonsterSpawnSpot = new Vector3(0, 3, 30);
    #endregion

    #region Medal
    //메달 레벨: 메달 레벨의 초기값, Max값
    public static Dictionary<int, Tuple<string, int, int>> MedalList = new()
    {
        { 1, new Tuple<string, int, int>("동메달 1", 0, 10 ) },
        { 2, new Tuple<string, int, int>("동메달 2", 10, 110 ) },
        { 3, new Tuple<string, int, int>("동메달 3", 110, 410 ) },
        { 4, new Tuple<string, int, int>("은메달 1", 410, 1010) },
        { 5, new Tuple<string, int, int>("은메달 2", 1010, 2010) },
        { 6, new Tuple<string, int, int>("은메달 3", 2010, 3510) },
        { 7, new Tuple<string, int, int>("금메달 1", 3510, 5710) },
        { 8, new Tuple<string, int, int>("금메달 2", 5710, 8510) },
        { 9, new Tuple<string, int, int>("금메달 3", 8510, 13010) }
    };
    #endregion

    #region Enum
    public enum SkillType
    {
        RigidbodyTarget,
        TransformTarget,
        AreaTarget,
        AoENonTarget,
        DirectionNonTarget,
        Buff,
    }
    public enum MotionType
    {
        None,
        Sword,
        Spell,
    }
    public enum HandlerType
    {
        Player,
        Monster,
        Skill,
    }
    public enum StatusType
    {
        Atk,
        Def,
        Damage,
        HP,
        HPRecoveryPerSec,
        MP,
        MPRecoveryPerSec,
        CoolTimeDecrease,
        None,
    }
    public enum GoodsType
    {
        None,
        SilverCoin,
        Exp,
        EnhancementStone,
        Gem,
    }
    public enum StageActionStatus
    {
        Challenge,
        AutoChallenge,
        NotChallenge,
        GoFinalStage,
        ExitStage
    }
    public enum TimeOfDayType
    {
        None,
        Morning,
        Noon,
        Evening,
        Night,
    }
    public enum ItemValue
    {
        Normal,
        Rare,
        Epic,
        Unique,
        Legendary
    }
    public enum ItemType
    {
        Equipment,
        Consumable,
        Other
    }

    public enum EquipmentItemType
    {
        Helmet,
        Armor,
        Gloves,
        Shoes,
        Weapon
    }
    #endregion

    #region Layer
    public const string PlayerSkillLayer = "PlayerSkill";
    public const string MonsterSkillLayer = "MonsterSkill";
    public const string MonsterAttackRangeLayer = "MonsterAttackRange";
    #endregion

    #region Constants
    public const float MaxDef = 100f;
    public const float SkillInterval = 0.5f;
    #endregion
}
