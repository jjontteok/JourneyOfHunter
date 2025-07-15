using System.Collections.Generic;
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
    public readonly static int UltimateReady = Animator.StringToHash("UltimateReady");
    public readonly static int Die = Animator.StringToHash("Die");
    public readonly static int DieType = Animator.StringToHash("DieType");
    public readonly static int IsDead = Animator.StringToHash("IsDead");

    public readonly static int CloseAttack = Animator.StringToHash("CloseAttack");
    public readonly static int LongAttack = Animator.StringToHash("LongAttack");
    public readonly static int IsInteractionPossible = Animator.StringToHash("IsInteractionPossible");

    public readonly static int Contact = Animator.StringToHash("Contact");
    public readonly static int Open = Animator.StringToHash("Open");

    public const string EndAttack = "EndAttack";
    public const string WalkSpeed = "WalkSpeed";
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string GroundTag = "Ground";
    public const string MonsterTag = "Monster";
    public const string FieldObjectTag = "FieldObject";
    public const string PortalTag = "Portal";
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
    public const string FieldObjectSpawnSpotPath = "Field/SpawnSpots";
    public const string FieldObjectsPath = "Field/Objects";
    public const string TreasureOpenEffectPath = "Field/Effect/TreasureOpenEffect";
    public const string SkyBoxPath = "SkyBox";
    public const string BackgroundPath = "Environment/Background";
    public const string DamageTextPath = "UI/Text/DamageText";
    public const string SystemTextPath = "UI/Text/SystemText/Text - SystemMessage";
    public const string RewardTextPath = "UI/Text/TreasureText/Text - Reward";
    public const string PlayerVitalCanvasPath = "UI/PlayerVital/UI_Vital";
    public const string PlayerVitalPath = "UI/PlayerVital/PlayerVital";
    public const string ItemSlotPath = "UI/PopupUI/IconSlotResources/ItemSlot";
    public const string MerchantSlotPath = "UI/PopupUI/IconSlotResources/MerchantItemSlot/MerchantItemSlot";

    public const string UIMainPath = "UI/MainUI/UI_Main";
    public const string UIGamePath = "UI/MainUI/UI_Game";
    public const string PopupUICanvasPath = "UI/PopupUI/Canvas - Popup";
    public const string PopupPanelPath = "UI/PopupUI/Panel - Popup";
    public const string ToolTipPanelPath = "UI/PopupUI/Panel - ToolTip";
    public const string PopupStatusPanelPath = "UI/PopupUI/StatusPanel - Popup";
    public const string PopupInventoryPanelPath = "UI/PopupUI/InventoryPanel - Popup";
    public const string PopupSkillInventoryPath = "UI/PopupUI/SkillInventoryPanel - Popup";
    public const string PopupJourneyInfoPanelPath = "UI/PopupUI/Panel - JourneyInfo";
    public const string PopupStageInfoPanelPath = "UI/PopupUI/Panel - StageInfo";
    public const string PopupNamedMonsterInfoPanelPath = "UI/PopupUI/Panel - NamedMonsterInfo";
    public const string PopupMerchantPanelPath = "UI/PopupUI/MerchantPanel - Popup";
    public const string PopupMerchantDialoguePanelPath = "UI/PopupUI/MerchantDialogue - Popup";

    public const string PopupStageTextPanelPath = "UI/PopupUI/Panel - StageText";
    public const string PopupTreasureAppearPanelPath = "UI/PopupUI/Panel - TreasureAppearText";
    public const string PopupBuffPanelPath = "UI/PopupUI/Panel - BuffText";
    public const string PopupDungeonAppearPanelPath = "UI/PopupUI/Panel - DungeonAppear";
    public const string PopupDungeonClearPanelPath = "UI/PopupUI/Panel - DungeonClearText";
    public const string PopupGachaPanelPath = "UI/PopupUI/RandomSummonPanel - Popup";
    public const string PopupItemInfoPanelPath = "UI/PopupUI/Panel - ItemInfo";
    public const string PopupMerchantAppearPanelPath = "UI/PopupUI/Panel - MerchantAppear";
    
    public const string GoblinKingCutScenePath = "CutScene/GoblinCutScene";
    public const string StartCameraPath = "Camera/StartCamera";
    public const string FollowCameraPath = "Camera/FollowPlayerCamera";
    public const string CutSceneCameraPath = "Camera/CutSceneCamera";

    public const string JourneyRankPath = "DB/JourneyRankDB";
    #endregion

    #region DungeonSpots
    public static Vector3 FirstEnterSpot = new Vector3(0, 3, 0);
    public static Vector3 DungeonEnterSpot = new Vector3(0, 5, 20);
    public static Vector3 DungeonExitSpot = new Vector3(0, 5, 90);
    public static Vector3 DungeonEnterPortalSpot = new Vector3(0, 2.5f, 50);
    public static Vector3 DungeonExitPortalSpot = new Vector3(0, 2.5f, 89f);
    #endregion

    #region MonsterSpawnSpots
    public static Vector3 SpawnSpot1 = new Vector3(-7, 7, 0);
    public static Vector3 SpawnSpot2 = new Vector3(7, 7, 0);
    public static Vector3 SpawnSpot3 = new Vector3(-7, 7, 20);
    public static Vector3 SpawnSpot4 = new Vector3(7, 7, 20);
    public static Vector3 SpawnSpot5 = new Vector3(-7, 7, -20);
    public static Vector3 SpawnSpot6 = new Vector3(7, 7, -20);
    public static Vector3 NamedMonsterSpawnSpot = new Vector3(0, 3, 30);
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
    public enum SkillAttribute
    {
        None,
        Fire,
        Water,
        Light,
        Dark,
    }
    public enum StatusType
    {
        Atk,
        Def,
        HP,
        HPRecoveryPerSec,
        MP,
        MPRecoveryPerSec,
        CoolTimeDecrease,
        None,
    }
    public enum GoodsType
    {
        SilverCoin,
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

    public enum JourneyRankType
    {
        Bronze,
        Silver,
        Gold,
        Master,
    }

    public enum JourneyType
    {
        Explore,    //구역 통과시
        Dungeon,    //던전 클리어
        TreasureBox,   //보물
        Merchant,
        OtherObject,
    }

    public enum RewardType
    {
        JourneyExp,
        SilverCoin,
        Gem,
    }

    public enum ItemValue
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        None
    }

    public enum DrawItemType
    {
        Equipment,
        Skill
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

    public enum ConsumeTarget
    {
        Player,
        Field,
        Dungeon,
        Goods,
    }

    public enum PendingTaskType
    {
        ItemAddTask,
        ItemRemoveTask,
        ItemUpdateTask,
        SkillTask,
    }
    #endregion

    #region Layer
    public const string PlayerSkillLayer = "PlayerSkill";
    public const string MonsterSkillLayer = "MonsterSkill";
    public const string MonsterAttackRangeLayer = "MonsterAttackRange";
    public const string TreasureBoxLayer = "TreasureBox";
    #endregion

    #region Constants
    public const float MaxDef = 100f;
    public const float SkillInterval = 0.5f;
    public const float TeleportDistance = 108.2f;
    public const int TotalSkillIconSlotNum = 6;
    public const int MinUnlockedSkillSlotCount = 2;
    public const int MaxUnlockedSkillSlotCount = TotalSkillIconSlotNum - 1;
    public readonly static string[] SkillAttributes = { "", "불", "물", "빛", "암" };
    public readonly static Vector2 SkillImagePosOffset = new Vector2(3f, -2.4f);

    public const string Morning = "MorningSkyBox";
    public const string Noon = "NoonSkyBox";
    public const string Evening = "EveningSkyBox";
    public const string Night = "NightSkyBox";
    #endregion

    #region Dictionary

    #region SkyBoxTime
    public static Dictionary<float, Define.TimeOfDayType> ColorChangeTimeList = new()
    {
        { 86f, Define.TimeOfDayType.Noon }, //7초에 낮 색 변경
        { 176f, Define.TimeOfDayType.Evening },
        { 265f, Define.TimeOfDayType.Night },
        { 358f, Define.TimeOfDayType.Morning }
    };
    #endregion

    #region Environment
    public static Dictionary<string, Color> ColorList = new()
    {
        { Morning,  new Color(0.5f, 0.5f, 0.5f, 1) },
        { Noon, new Color(0.5f, 0.5f, 0.5f, 1) },
        { Evening, new Color(0, 0, 0, 1) },
        { Night, new Color(0.26f, 0.34f, 0.415f, 1) }
        //{ Night, new Color(0.426f, 0.611f, 0.896f, 1) }
    };

    public static Dictionary<string, Color> TargetColorList = new()
    {
         { Morning,  new Color(0.36f, 0.446f, 0.5f, 1) },
        { Noon, new Color(0.29f, 0.221f, 0.221f, 1) },
        { Evening, new Color(0.011f, 0.08f, 0.08f, 1) },
        { Night, new Color(0.9f, 0.9f, 0.9f, 1)}
    };

    public static Dictionary<string, float> TargetLightList = new()
    {
        { Morning,  1f },
        { Noon, 0.5f },
        { Evening, 0.1f },
        { Night, 0.5f}
    };

    public static Dictionary<string, float> TargetMountainList = new()
    {
        { Morning,  0.9f },
        { Noon, 0.5f },
        { Evening, 0.33f },
        { Night, 0.66f}
    };

    public static Dictionary<string, float> RotateSkyBoxList = new()
    {
        { Morning,  270 },
        { Noon, 0 },
        { Evening, 50 },
        { Night, 312}
    };

    public static Dictionary<string, float> RotateLightList = new()
    {
        { Morning,  20 },
        { Noon, 40 },
        { Evening, 20 },
        { Night, 40 }
    };

    public static Dictionary<string, float> SkyBoxDurationList = new()
    {
        { Morning,  90f },
        { Noon, 90f },
        { Evening, 90f },
        { Night, 90f }
    };
    #endregion

    #region TreasureBoxColor
    public static Dictionary<Define.ItemValue, Color> EffectColorList = new()
    {
        { Define.ItemValue.Common, new Color(1, 1, 1, 1) },
        { Define.ItemValue.Uncommon, new Color(0.2f, 1, 0, 1) },
        { Define.ItemValue.Rare, new Color(0, 0.35f, 1, 1) },
        { Define.ItemValue.Epic, new Color(0.5f, 0, 1, 1) },
        { Define.ItemValue.Legendary, new Color(1, 0.11f, 0, 1) }
    };
    #endregion

    #endregion

    #region List
    public static List<string> MerchantDialogue = new()
    {
        "후후.. 좋은 물건이 들어왔지.",
        "오랜만이군, 여행자",
        "오늘은 자네가 운이 좋군"
    };
    #endregion
}
