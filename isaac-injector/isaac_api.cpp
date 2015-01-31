#include "stdafx.h"
#include "Externs.h"
#include "hooks.h"
#include "isaac_api.h"

PlayerManager* API_GetPlayerManager()
{
	return Hooks_GetPlayerManager();
}

void API_Effect_GoodPill(Player* player)
{
	GoodPillEffectFunc(player);
}

void API_HPUp(Player* player, int amount)
{
	_asm
	{
		mov eax, player
		mov ecx, amount
		call HpUpEvent_Original
	}
}

void API_AddSoulHearts(Player* player, int amount)
{
	_asm
	{
		mov eax, player
		mov ecx, amount
		call AddSoulHeartsEvent_Original
	}
}

Entity* API_SpawnEntity(int entityID, int variant, int subtype, float x, float y, Entity* parent)
{
	// zero
	PointF* velocity = new PointF();
	velocity->x = 0;
	velocity->y = 0;
	// position
	PointF* pos = new PointF();
	pos->x = (x * 40) + 80;
	pos->y = (y * 40) + 160;
	// manager
	PlayerManager* playerMan = Hooks_GetPlayerManager();

	unsigned int seed = IsaacRandomFunc();

	if (playerMan)
	{
		_asm
		{
			push seed
			push subtype
			push parent
			push variant
			push entityID
			push playerMan
			mov ebx, pos
			mov eax, velocity
			call SpawnEntityEvent_Original
		}
	}
	else
		return NULL;
}

int API_TeleportPlayer(int roomID)
{
	PlayerManager* playerMan = Hooks_GetPlayerManager();
	void* roomManager = (void*)((int)playerMan+33088);  // I actually don't know this yet

	_asm
	{
		mov eax, 3
		mov edx, roomID
		mov esi, roomManager
		push -1
		call Player_TeleportFunc
	}
}

TearInfo* API_InitTear(int value, TearInfo* tear)
{
	_asm
	{
		mov ecx, value
		mov esi, tear
		call InitTearFunc
	}
}

void API_AddCollectible(Player* player, int itemID)
{
	_asm
	{
		mov ecx, player
		push 0
		push 0
		push itemID
		call AddCollectibleEvent_Original
	}
}

void API_ShootTears(PointF* pos, PointF* velocity, int pattern, TearInfo* tear, Entity* source)
{
	_asm
	{
		push tear
		push pattern
		push source
		mov edx, pos
		mov ecx, velocity
		call ShootTearsEvent_Original
	}
}

bool API_PlayerHasItem(Player* player, int itemID)
{
	return player->_items[itemID-1];
}

char* API_getItemName(int itemID)
{
	std::map<int, char*> items;
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
	return items[itemID];
} 