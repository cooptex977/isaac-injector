﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OML
{
    public abstract class OMLPlugin
    {
        public string PluginName;
        public string PluginVersion;
        public string PluginAuthor;
        public List<Item> CustomItemList = new List<Item>();

        #region Events for Binding
        public event EventHandler<OnPlayerAddCollectibleEventArgs> eOnPlayerAddCollectible;
        public event EventHandler<OnEntitySpawnEventArgs> eOnEntitySpawn;
        public event EventHandler<OnPlayerCardUseEventArgs> eOnPlayerCardUse;
        public event EventHandler<OnPlayerPillUseEventArgs> eOnPlayerPillUse;
        public event EventHandler<OnPlayerHealthModifiedEventArgs> eOnPlayerHealthModified;
        public event EventHandler<OnSoulHeartsAddedEventArgs> eOnSoulHeartsAdded;
        public event EventHandler<OnRoomChangeEventArgs> eOnRoomChange;
        public event EventHandler<OnEnemyTearShotEventArgs> eOnEnemyTearShot;
        public event EventHandler<OnGameUpdateEventArgs> eOnGameUpdate;
        public event EventHandler<OnGotoFloorEventArgs> eOnGotoFloor;
        public event EventHandler<OnPlayerUpdateEventArgs> eOnPlayerUpdate;
        #endregion

        public virtual void PluginInit()
        {
        }

        public virtual void OnPlayerAddCollectible(Player player, int a2, int id, int a4)
        {
            if(eOnPlayerAddCollectible != null)
            {
                OnPlayerAddCollectibleEventArgs e = new OnPlayerAddCollectibleEventArgs(player, a2, id, a4);
                eOnPlayerAddCollectible(this, e);
            }
        }

        public virtual void OnEntitySpawn(PointF velocity, PointF position, int entityID, int variant, int subtype, Entity parent)
        {
            if(eOnEntitySpawn != null)
            {
                OnEntitySpawnEventArgs e = new OnEntitySpawnEventArgs(velocity, position, entityID, variant, subtype, parent);
                eOnEntitySpawn(this, e);
            }
        }

        public virtual void OnPlayerCardUse(Player player, int cardID, ref bool handled)
        {
            // TODO: Implement a Card Dictionary similar to PillDictionary

            if(eOnPlayerCardUse != null)
            {
                OnPlayerCardUseEventArgs e = new OnPlayerCardUseEventArgs(player, cardID);
                eOnPlayerCardUse(this, e);
            }
        }

        public virtual void OnPlayerPillUse(Player player, int pillID, ref bool handled)
        {
            Pill p = PillDictionary.GetPill(pillID);

            if (p != null)
            {
                p.OnUse(player);
                handled = true;
            }

            else
                handled = false;

            if(eOnPlayerPillUse != null)
            {
                OnPlayerPillUseEventArgs e = new OnPlayerPillUseEventArgs(player, pillID);
                eOnPlayerPillUse(this, e);
            }
        }

        public virtual void OnPlayerHealthDown(Player player, ref int amount)
        {
            if(eOnPlayerHealthModified != null)
            {
                OnPlayerHealthModifiedEventArgs e = new OnPlayerHealthModifiedEventArgs(player, amount * -1);
                eOnPlayerHealthModified(this, e);
            }
        }

        public virtual void OnPlayerHealthUp(Player player, ref int amount)
        {
            if (eOnPlayerHealthModified != null)
            {
                OnPlayerHealthModifiedEventArgs e = new OnPlayerHealthModifiedEventArgs(player, amount);
                eOnPlayerHealthModified(this, e);
            }
        }

        public virtual void OnSoulHeartsAdded(Player player, ref int amount)
        {
            if(eOnSoulHeartsAdded != null)
            {
                OnSoulHeartsAddedEventArgs e = new OnSoulHeartsAddedEventArgs(player, amount);
                eOnSoulHeartsAdded(this, e);
            }
        }

        public virtual void OnRoomChange(int newRoomIndex)
        {
            if(eOnRoomChange != null)
            {
                OnRoomChangeEventArgs e = new OnRoomChangeEventArgs(newRoomIndex);
                eOnRoomChange(this, e);
            }
        }

        public virtual void OnEnemyTearShot(PointF velocity, PointF position, Entity sourceEntity, int pattern, TearInfo tearInfo)
        {
            if(eOnEnemyTearShot != null)
            {
                OnEnemyTearShotEventArgs e = new OnEnemyTearShotEventArgs(velocity, position, sourceEntity, pattern, tearInfo);
                eOnEnemyTearShot(this, e);
            }
        }

        public virtual void OnGameUpdate()
        {
            if(eOnGameUpdate != null)
            {
                OnGameUpdateEventArgs e = new OnGameUpdateEventArgs();
                eOnGameUpdate(this, e);
            }
        }

        public virtual void OnGotoFloor(Floor nextFloor)
        {
            if(eOnGotoFloor != null)
            {
                OnGotoFloorEventArgs e = new OnGotoFloorEventArgs(nextFloor);
                eOnGotoFloor(this, e);
            }
        }

        public virtual void OnPlayerUpdate(Player player)
        {
            if(eOnPlayerUpdate != null)
            {
                OnPlayerUpdateEventArgs e = new OnPlayerUpdateEventArgs(player);
                eOnPlayerUpdate(this, e);
            }
        }
    }

    public enum ResourceType
    {
        Item
    }

    public class OMLResource
    {
        public ResourceType resourceType;
        public string resourceName;

        public OMLResource(ResourceType type, string name)
        {
            resourceType = type;
            resourceName = name;
        }
    }

    public static class _OML
    {
        public static API_ConnectionInfo Connection = null;

        public static Dictionary<int, string> items = new Dictionary<int, string>();

        public const int PLAYER_EVENT_TAKEPILL       = 0x00;
        public const int PLAYER_EVENT_ADDCOLLECTIBLE = 0x01;
        public const int GAME_EVENT_SPAWNENTITY      = 0x02;
        public const int PLAYER_EVENT_HPUP           = 0x03;
        public const int PLAYER_EVENT_HPDOWN         = 0x04;
        public const int PLAYER_EVENT_ADDSOULHEARTS  = 0x05;
        public const int ENEMY_EVENT_SHOOTTEARS      = 0x06;
        public const int PLAYER_EVENT_CHANGEROOM     = 0x07;
        public const int GAME_EVENT_UPDATE           = 0x08;
        public const int PLAYER_EVENT_UPDATE         = 0x09;
        public const int PLAYER_EVENT_USECARD        = 0x0A;
        public const int GAME_EVENT_GOTOFLOOR        = 0x0B;
        public const int PLAYER_EVENT_HITBYENEMY     = 0x0C;

        public const uint APICALL_NULL               = 0x00;

        public const uint APICALL_HPUP               = 0x01;
        public const uint APICALL_ADDSOULHEARTS      = 0x02;
        public const uint APICALL_GETKEYS            = 0x03;
        public const uint APICALL_SETKEYS            = 0x04;
        public const uint APICALL_GETBOMBS           = 0x05;
        public const uint APICALL_SETBOMBS           = 0x06;
        public const uint APICALL_GETCOINS           = 0x07;
        public const uint APICALL_SETCOINS           = 0x08;
        public const uint APICALL_GETTRINKET         = 0x09;
        public const uint APICALL_SETTRINKET         = 0x0A;
        public const uint APICALL_ADDCOLLECTIBLE     = 0x0B;
        public const uint APICALL_HASITEM            = 0x0C;
        public const uint APICALL_GETPOSITION        = 0x0D;
        public const uint APICALL_SETPOSITION        = 0x0E;
        public const uint APICALL_TELEPORT           = 0x0F;
        public const uint APICALL_GETSTAT            = 0x10;
        public const uint APICALL_SETSTAT            = 0x11;
        public const uint APICALL_SPAWNENTITY        = 0x12;
        public const uint APICALL_GOTOFLOOR          = 0x13;
        public const uint APICALL_GETCUSTOMITEMS     = 0x14;
        public const uint APICALL_ADDCUSTOMITEM      = 0x15;
        public const uint APICALL_ADDCOSTUME         = 0x16;

        public const uint APICALL_END                = 0xFFFFFFFF;

        public static Player GetPlayer(IntPtr handle)
        {
            return new Player(handle);
        }
        public static string GetItemName(int itemID)
        {
            if (!IsValidItemID(itemID))
                return "";
            if (itemID == 235 || itemID == 263 || itemID == 59)
                return "";
            return items[itemID];
        }
        public static bool IsValidItemID(int itemID)
        {
            return itemID <= items.Count + 1;
        }
        public static int GetItemID(string itemname)
        {
            if (items.ContainsValue(itemname))
                return items.First(x => x.Value.ToLower() == itemname.ToLower()).Key;
            else
                return 0;
        }
        public static void Init()
        {
            #region items
            items[1] = "The Sad Onion";
            items[2] = "The Inner Eye";
            items[3] = "Spoon Bender";
            items[4] = "Cricket's Head";
            items[5] = "My Reflection";
            items[6] = "Number One";
            items[7] = "Blood of the Martyr";
            items[8] = "Brother Bobby";
            items[9] = "Skatole";
            items[10] = "Halo of Flies";
            items[11] = "1up!";
            items[12] = "Magic Mushroom";
            items[13] = "The Virus";
            items[14] = "Roid Rage";
            items[15] = "<3";
            items[16] = "Raw Liver";
            items[17] = "Skeleton Key";
            items[18] = "A Dollar";
            items[19] = "Boom!";
            items[20] = "Transcendence";
            items[21] = "The Compass";
            items[22] = "Lunch";
            items[23] = "Dinner";
            items[24] = "Dessert";
            items[25] = "Breakfast";
            items[26] = "Rotten Meat";
            items[27] = "Wooden Spoon";
            items[28] = "The Belt";
            items[29] = "Mom's Underwear";
            items[30] = "Mom's Heels";
            items[31] = "Mom's Lipstick";
            items[32] = "Wire Coat Hanger";
            items[33] = "The Bible";
            items[34] = "The Book of Belial";
            items[35] = "The Necronomicon";
            items[36] = "The Poop";
            items[37] = "Mr. Boom";
            items[38] = "Tammy's Head";
            items[39] = "Mom's Bra";
            items[40] = "Kamikaze!";
            items[41] = "Mom's Pad";
            items[42] = "Bob's Rotten Head";
            items[43] = "Pills here!";
            items[44] = "Teleport!";
            items[45] = "Yum Heart";
            items[46] = "Lucky Foot";
            items[47] = "Doctor's Remote";
            items[48] = "Cupid's Arrow";
            items[49] = "Shoop da Whoop!";
            items[50] = "Steven";
            items[51] = "Pentagram";
            items[52] = "Dr. Fetus";
            items[53] = "Magneto";
            items[54] = "Treasure Map";
            items[55] = "Mom's Eye";
            items[56] = "Lemon Mishap";
            items[57] = "Distant Admiration";
            items[58] = "Book of Shadows";
            items[60] = "The Ladder";
            items[61] = "Tarot Card";
            items[62] = "Charm of the Vampire";
            items[63] = "The Battery";
            items[64] = "Steam Sale";
            items[65] = "Anarchist Cookbook";
            items[66] = "The Hourglass";
            items[67] = "Sister Maggy";
            items[68] = "Technology";
            items[69] = "Chocolate Milk";
            items[70] = "Growth Hormones";
            items[71] = "Mini Mush";
            items[72] = "Rosary";
            items[73] = "Cube of Meat";
            items[74] = "A Quarter";
            items[75] = "PHD";
            items[76] = "X-Ray Vision";
            items[77] = "My Little Unicorn";
            items[78] = "Book of Revelations";
            items[79] = "The Mark";
            items[80] = "The Pact";
            items[81] = "Dead Cat";
            items[82] = "Lord of the Pit";
            items[83] = "The Nail";
            items[84] = "We Need To Go Deeper!";
            items[85] = "Deck of Cards";
            items[86] = "Monstro's Tooth";
            items[87] = "Loki's Horns";
            items[88] = "Little Chubby";
            items[89] = "Spider Bite";
            items[90] = "The Small Rock";
            items[91] = "Spelunker Hat";
            items[92] = "Super Bandage";
            items[93] = "The Gamekid";
            items[94] = "Sack of Pennies";
            items[95] = "Robo-Baby";
            items[96] = "Little C.H.A.D.";
            items[97] = "The Book of Sin";
            items[98] = "The Relic";
            items[99] = "Little Gish";
            items[100] = "Little Steven";
            items[101] = "The Halo";
            items[102] = "Mom's Bottle of Pills";
            items[103] = "The Common Cold";
            items[104] = "The Parasite";
            items[105] = "The D6";
            items[106] = "Mr. Mega";
            items[107] = "The Pinking Shears";
            items[108] = "The Wafer";
            items[109] = "Money = Power";
            items[110] = "Mom's Contacts";
            items[111] = "The Bean";
            items[112] = "Guardian Angel";
            items[113] = "Demon Baby";
            items[114] = "Mom's Knife";
            items[115] = "Ouija Board";
            items[116] = "9 Volt";
            items[117] = "Dead Bird";
            items[118] = "Brimstone";
            items[119] = "Blood Bag";
            items[120] = "Odd Mushroom";
            items[121] = "Odd Mushroom";
            items[122] = "Whore of Babylon";
            items[123] = "Monster Manual";
            items[124] = "Dead Sea Scrolls";
            items[125] = "Bobby-Bomb";
            items[126] = "Razor Blade";
            items[127] = "Forget Me Now";
            items[128] = "Forever alone";
            items[129] = "Bucket of Lard";
            items[130] = "A Pony";
            items[131] = "Bomb Bag";
            items[132] = "A Lump of Coal";
            items[133] = "Guppy's Paw";
            items[134] = "Guppy's Tail";
            items[135] = "IV Bag";
            items[136] = "Best Friend";
            items[137] = "Remote Detonator";
            items[138] = "Stigmata";
            items[139] = "Mom's Purse";
            items[140] = "Bobs Curse";
            items[141] = "Pageant Boy";
            items[142] = "Scapular";
            items[143] = "Speed Ball";
            items[144] = "Bum Friend";
            items[145] = "Guppy's Head";
            items[146] = "Prayer Card";
            items[147] = "Notched Axe";
            items[148] = "Infestation";
            items[149] = "Ipecac";
            items[150] = "Tough Love";
            items[151] = "The Mulligan";
            items[152] = "Technology 2";
            items[153] = "Mutant Spider";
            items[154] = "Chemical Peel";
            items[155] = "The Peeper";
            items[156] = "Habit";
            items[157] = "Bloody Lust";
            items[158] = "Crystal Ball";
            items[159] = "Spirit of the Night";
            items[160] = "Crack the Sky";
            items[161] = "Ankh";
            items[162] = "Celtic Cross";
            items[163] = "Ghost Baby";
            items[164] = "The Candle";
            items[165] = "Cat-o-nine-tails";
            items[166] = "D20";
            items[167] = "Harlequin Baby";
            items[168] = "Epic Fetus";
            items[169] = "Polyphemus";
            items[170] = "Daddy Longlegs";
            items[171] = "Spider Butt";
            items[172] = "Sacrificial Dagger";
            items[173] = "Mitre";
            items[174] = "Rainbow Baby";
            items[175] = "Dad's Key";
            items[176] = "Stem Cells";
            items[177] = "Portable Slot";
            items[178] = "Holy Water";
            items[179] = "Fate";
            items[180] = "The Black Bean";
            items[181] = "White Pony";
            items[182] = "Sacred Heart";
            items[183] = "Tooth Picks";
            items[184] = "Holy Grail";
            items[185] = "Dead Dove";
            items[186] = "Blood Rights";
            items[187] = "Guppy's Hairball";
            items[188] = "Abel";
            items[189] = "SMB Super Fan";
            items[190] = "Pyro";
            items[191] = "3 Dollar Bill";
            items[192] = "Telepathy For Dummies";
            items[193] = "MEAT!";
            items[194] = "Magic 8 Ball";
            items[195] = "Mom's Coin Purse";
            items[196] = "Squeezy";
            items[197] = "Jesus Juice";
            items[198] = "Box";
            items[199] = "Mom's Key";
            items[200] = "Mom's Eyeshadow";
            items[201] = "Iron Bar";
            items[202] = "Midas' Touch";
            items[203] = "Humbleing Bundle";
            items[204] = "Fanny Pack";
            items[205] = "Sharp Plug";
            items[206] = "Guillotine";
            items[207] = "Ball of Bandages";
            items[208] = "Champion Belt";
            items[209] = "Butt Bombs";
            items[210] = "Gnawed Leaf";
            items[211] = "Spiderbaby";
            items[212] = "Guppy's Collar";
            items[213] = "Lost Contact";
            items[214] = "Anemic";
            items[215] = "Goat Head";
            items[216] = "Ceremonial Robes";
            items[217] = "Mom's Wig";
            items[218] = "Placenta";
            items[219] = "Old Bandage";
            items[220] = "Sad Bombs";
            items[221] = "Rubber Cement";
            items[222] = "Anti-Gravity";
            items[223] = "Pyromaniac";
            items[224] = "Cricket's Body";
            items[225] = "Gimpy";
            items[226] = "Black Lotus";
            items[227] = "Piggy Bank";
            items[228] = "Mom's Perfume";
            items[229] = "Monstro's Lung";
            items[230] = "Abaddon";
            items[231] = "Ball of Tar";
            items[232] = "Stop Watch";
            items[233] = "Tiny Planet";
            items[234] = "Infestation 2";
            items[236] = "E. Coli";
            items[237] = "Death's Touch";
            items[238] = "Key Piece 1";
            items[239] = "Key Piece 2";
            items[240] = "Experimental Treatment";
            items[241] = "Contract from Below";
            items[242] = "Infamy";
            items[243] = "Trinity Shield";
            items[244] = "Tech.5";
            items[245] = "20/20";
            items[246] = "Blue Map";
            items[247] = "BFFS!";
            items[248] = "Hive Mind";
            items[249] = "There's Options";
            items[250] = "BOGO Bombs";
            items[251] = "Starter Deck";
            items[252] = "Little Baggy";
            items[253] = "Magic Scab";
            items[254] = "Blood Clot";
            items[255] = "Screw";
            items[256] = "Hot Bombs";
            items[257] = "Fire Mind";
            items[258] = "Missing No.";
            items[259] = "Dark Matter";
            items[260] = "Black Candle";
            items[261] = "Proptosis";
            items[262] = "Missing Page 2";
            items[264] = "Smart Fly";
            items[265] = "Dry Baby";
            items[266] = "Juicy Sack";
            items[267] = "Robo-Baby 2.0";
            items[268] = "Rotten Baby";
            items[269] = "Headless Baby";
            items[270] = "Leech";
            items[271] = "Mystery Sack";
            items[272] = "BBF";
            items[273] = "Bob's Brain";
            items[274] = "Best Bud";
            items[275] = "Lil' Brimstone";
            items[276] = "Isaac's Heart";
            items[277] = "Lil' Haunt";
            items[278] = "Dark Bum";
            items[279] = "Big Fan";
            items[280] = "Sissy Longlegs";
            items[281] = "Punching Bag";
            items[282] = "How to Jump";
            items[283] = "D100";
            items[284] = "D4";
            items[285] = "D10";
            items[286] = "Blank Card";
            items[287] = "Book of Secrets";
            items[288] = "Box of Spiders";
            items[289] = "Red Candle";
            items[290] = "The Jar";
            items[291] = "Flush!";
            items[292] = "Satanic Bible";
            items[293] = "Head of Krampus";
            items[294] = "Butter Bean";
            items[295] = "Magic Fingers";
            items[296] = "Converter";
            items[297] = "Pandora's Box";
            items[298] = "Unicorn Stump";
            items[299] = "Taurus";
            items[300] = "Aries";
            items[301] = "Cancer";
            items[302] = "Leo";
            items[303] = "Virgo";
            items[304] = "Libra";
            items[305] = "Scorpio";
            items[306] = "Sagittarius";
            items[307] = "Capricorn";
            items[308] = "Aquarius";
            items[309] = "Pisces";
            items[310] = "Eve's Mascara";
            items[311] = "Judas' Shadow";
            items[312] = "Maggy's Bow";
            items[313] = "Holy Mantle";
            items[314] = "Thunder Thighs";
            items[315] = "Strange Attractor";
            items[316] = "Cursed Eye";
            items[317] = "Mysterious Liquid";
            items[318] = "Gemini";
            items[319] = "Cain's Other Eye";
            items[320] = "???'s Only Friend";
            items[321] = "Samson's Chains";
            items[322] = "Mongo Baby";
            items[323] = "Isaac's Tears";
            items[324] = "Undefined";
            items[325] = "Scissors";
            items[326] = "Breath of Life";
            items[327] = "The Polaroid";
            items[328] = "The Negative";
            items[329] = "The Ludovico Technique";
            items[330] = "Soy Milk";
            items[331] = "Godhead";
            items[332] = "Lazarus' Rags";
            items[333] = "The Mind";
            items[334] = "The Body";
            items[335] = "The Soul";
            items[336] = "Dead Onion";
            items[337] = "Broken Watch";
            items[338] = "The Boomerang";
            items[339] = "Safety Pin";
            items[340] = "Caffeine Pill";
            items[341] = "Torn Photo";
            items[342] = "Blue Cap";
            items[343] = "Latch Key";
            items[344] = "Match Book";
            items[345] = "Synthoil";
            items[346] = "A Snack";
            #endregion
        }

    }
    
    public enum PlayerStat
    {
        Speed,
        Range,
        FireRate,
        ShotSpeed,
        Damage,
        Luck
    }

    public enum PlayerInv
    {
        Coins,
        Bombs, 
        Keys
    }

    public enum Floor
    {
        Basement1 = 1,
        Basement2 = 2,
        Caves1    = 3,
        Caves2    = 4,
        Depths1   = 5,
        Depths2   = 6,
        Womb1     = 7,
        Womb2     = 8,
        Sheol     = 9,
        DarkRoom  = 11,
        Cathedral = unchecked((int)(9 | 0x80000000)),
        Chest     = unchecked((int)(11 | 0x80000000))
    }
}


