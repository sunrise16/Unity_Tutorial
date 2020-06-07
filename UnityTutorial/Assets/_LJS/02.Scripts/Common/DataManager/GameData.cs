using System.Collections.Generic;

namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        // 사망한 적 캐릭터의 수
        public int killCount = 0;
        // 주인공의 초기 HP
        public float hp = 120.0f;
        // 총알의 데미지
        public float damage = 25.0f;
        // 이동 속도
        public float speed = 6.0f;
        // 취득한 아이템
        public List<Item> equipItem = new List<Item>();
    }

    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP, SPEED, GRENADE, DAMAGE }
        public enum ItemCalc { INC_VALUE, PERCENT }
        public ItemType itemType;
        public ItemCalc itemCalc;
        public string name;
        public string desc;
        public float value;
    }
}