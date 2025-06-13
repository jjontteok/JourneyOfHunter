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
    public const string DamageTextPath = "UI/DamageText";
    public const string PlayerVitalCanvasPath = "UI/PlayerVital/UI_Vital";
    public const string PlayerVitalPath = "UI/PlayerVital/PlayerVital";

    public const string UIGamePath = "UI/MainUI/UI_Game";
    public const string PopupUICanvasPath = "UI/PopupUI/Canvas - Popup";
    public const string PopupPanelPath = "UI/PopupUI/Panel - Popup";
    public const string PopupStatusPanelPath = "UI/PopupUI/StatusPanel - Popup";
    public const string PopupInventoryPanelPath = "UI/PopupUI/InventoryPanel - Popup";
    public const string PopupSkillInventoryPath = "UI/PopupUI/UI_SkillInventory - Popup";
    public const string PopupGainedRecordPanelPath = "UI/PopupUI/GainedRecordPanel - Popup";
    public const string PopupStageInfoPath = "UI/PopupUI/StageInfo - Popup";
    public const string PopupNamedMonsterInfoPath = "UI/PopupUI/NamedMonsterInfo - Popup";

    public const string GoblinKingCutScenePath = "CutScene/GoblinCutScene";
    #endregion

    #region DungeonSpots
    public static Vector3 FirstEnterSpot = new Vector3(0, 3, 0);
    public static Vector3 DungeonEnterSpot = new Vector3(0, 5, 30);
    public static Vector3 DungeonExitSpot = new Vector3(0, 5, 100);
    public static Vector3 DungeonEnterPortalSpot = new Vector3(0, 2.5f, 29);
    public static Vector3 DungeonExitPortalSpot = new Vector3(0, 2.5f, 99);
    #endregion

    #region SpawnSpots
    public static Vector3 SpawnSpot1 = new Vector3(-10, 7, 80);
    public static Vector3 SpawnSpot2 = new Vector3(10, 7, 80);
    public static Vector3 SpawnSpot3 = new Vector3(-5, 7, 80);
    public static Vector3 SpawnSpot4 = new Vector3(5, 7, 80);
    public static Vector3 SpawnSpot5 = new Vector3(0, 7, 80);
    //public static Vector3 SpawnSpot6 = new Vector3(10, 7, 70);
    public static Vector3 NamedMonsterSpawnSpot = new Vector3(0, 3, 80);
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
        HP,
        HPRecoveryPerSec,
        Def,
        MP,
        MPRecoveryPerSec,
    }

    public enum GoodsType
    {
        None,
        SilverCoin,
        Exp,
        EnhancementStone,
        Gem,
    }

    public enum TimeOfDayType
    {
        Morning,
        Noon,
        Evening,
        Night,
    }
    #endregion

    #region Layer
    public const string PlayerSkillLayer = "PlayerSkill";
    public const string MonsterSkillLayer = "MonsterSkill";
    public const string MonsterAttackRangeLayer = "MonsterAttackRange";
    #endregion

    #region Constants
    public const float MaxDef = 100f;
    #endregion
}
