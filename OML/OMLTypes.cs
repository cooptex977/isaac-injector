﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OML
{
    public class Entity
    {
        private IntPtr handle = IntPtr.Zero;

        public IntPtr Handle
        {
            get { return handle; }
        }

        public Entity(IntPtr handle)
        {
            this.handle = handle;
        }
    }

    public class Player : Entity
    {
        public Player(IntPtr handle)
            : base(handle)
        {

        }

        public int Keys
        {
            get { return new API_GetKeysCall(OML.Connection, Handle).Call(); }

            set { new API_SetKeysCall(OML.Connection, Handle, value).Call(); }
        }

        public int Bombs
        {
            get { return new API_GetBombsCall(OML.Connection, Handle).Call(); }

            set { new API_SetBombsCall(OML.Connection, Handle, value).Call(); }
        }

        public int Coins
        {
            get { return new API_GetCoinsCall(OML.Connection, Handle).Call(); }

            set { new API_SetCoinsCall(OML.Connection, Handle, value).Call();  }
        }

        public PointF Position
        {
            get { return new API_GetPositionCall(OML.Connection, Handle).Call(); }
            set { new API_SetPositionCall(OML.Connection, Handle, value).Call(); }
        }

        public void HpUp(int amount)
        {
            new API_HpUpCall(OML.Connection, Handle, amount).Call();
        }

        public void HpDown(int amount)
        {
            new API_HpUpCall(OML.Connection, Handle, -amount).Call();
        }

        public int GetStat(PlayerStat stat)
        {
            return new API_GetStatCall(OML.Connection, Handle, stat).Call();
        }

        public void SetStat(PlayerStat stat, int amount)
        {
            new API_SetStatCall(OML.Connection, Handle, amount, stat).Call();
        }

        public void AddCollectible(int itemid)
        {
            new API_AddCollectibleCall(OML.Connection, Handle, itemid).Call();
        }

        public void AddCollectible(string itemname)
        {
            int id = OML.GetItemID(itemname);
            new API_AddCollectibleCall(OML.Connection, Handle, id).Call();
        }

        public bool HasItem(int itemID)
        {
            return new API_HasItemCall(OML.Connection, Handle, itemID).Call();
        }

        public bool HasItem(string name)
        {
            int id = OML.GetItemID(name);
            return new API_HasItemCall(OML.Connection, Handle, id).Call();
        }
    }
    public class API
    {
        public static Entity SpawnItem(Player player, int itemID)
        {
            if (!OML.IsValidItemID(itemID))
            {
                Console.WriteLine("\r\n[ERROR] invalid item id");
                return null;
            }
            IntPtr entityHandle = new API_SpawnEntityCall(OML.Connection, 5, 100, itemID, NormalizePointF(player.Position).x, NormalizePointF(player.Position).y - 1, IntPtr.Zero).Call();
            return new Entity(entityHandle);
        }
        public static Entity SpawnItem(Player player, string itemname)
        {
            int itemID = OML.GetItemID(itemname);
            if (itemID == -1)
            {
                Console.WriteLine("\r\n[ERROR] invalid item name");
                return null;
            }
            IntPtr entityHandle = new API_SpawnEntityCall(OML.Connection, 5, 100, itemID, NormalizePointF(player.Position).x, NormalizePointF(player.Position).y - 1, IntPtr.Zero).Call();
            return new Entity(entityHandle);
        }
        public static Entity SpawnEntity(int entityID, int variant, int subtype, float x, float y, IntPtr parentHandle)
        {
            IntPtr entityHandle = new API_SpawnEntityCall(OML.Connection, entityID, variant, subtype, x, y, parentHandle).Call();
            return new Entity(entityHandle);
        }
        public static void GotoFloor(Floor floor)
        {
            new API_GotoFloorCall(OML.Connection, (uint)floor).Call();
        }
        public static void Teleport(int roomid)
        {
            new API_TeleportCall(OML.Connection, roomid).Call();
        }
        public static PointF NormalizePointF(PointF pf)
        {
            PointF result = new PointF();
            result.x = (pf.x - 120) / 40;
            result.y = (pf.y - 160) / 40;
            return result;
        }
    }
}
