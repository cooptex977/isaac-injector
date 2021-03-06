#pragma once
#include "stdafx.h"
#include "Externs.h"

#define PLAYER_EVENT_TAKEPILL		0x00
#define PLAYER_EVENT_ADDCOLLECTIBLE 0x01
#define GAME_EVENT_SPAWNENTITY		0x02
#define PLAYER_EVENT_HPUP		    0x03
#define PLAYER_EVENT_HPDOWN		    0x04 
#define PLAYER_EVENT_ADDSOULHEARTS  0x05
#define ENEMY_EVENT_SHOOTTEARS      0x06
#define PLAYER_EVENT_CHANGEROOM		0x07
#define GAME_EVENT_UPDATE			0x08
#define PLAYER_EVENT_UPDATE         0x09
#define PLAYER_EVENT_USECARD        0x0A
#define GAME_EVENT_GOTOFLOOR        0x0B
#define PLAYER_EVENT_HITBYENEMY		0x0C

// initialization + playermanager
bool Hooks_Init();
extern ItemStorageArray* itemStorageArray;
extern std::map<int, Item*> custom_items;
extern std::map<int, bool> has_custom_item;

PlayerManager* Hooks_GetPlayerManager();

// Hooks
extern void* TakePillEvent_Original;
extern void* AddCollectibleEvent_Original;
extern void* SpawnEntityEvent_Original;
extern void* GameUpdateEvent_Original;
extern void* HpUpEvent_Original;
extern void* AddSoulHeartsEvent_Original;
extern void* AddBlackHeartsEvent_Original;
extern void* ShootTearsEvent_Original;
extern void* GotoFloorEvent_Original;

// functions
using IsaacRandomFuncType = unsigned int __cdecl(void);
extern IsaacRandomFuncType* IsaacRandomFunc;

extern void* GoodPillEffectFunc;
extern void* BadPillEffectFunc;

using InitTearFuncType = TearInfo* __cdecl (int, TearInfo*);
extern InitTearFuncType* InitTearFunc;

using Player_TeleportFuncType = void (void);
extern Player_TeleportFuncType* Player_TeleportFunc;

extern void* Player_GiveEternalHeartFunc;

extern void* Game_FreezeEntityFunc;
extern void* Game_FearEntityFunc;
extern void* Game_ConfuseEntityFunc;
extern void* Game_CharmEntityFunc;

extern void* Game_ChangePickupEntityFunc;

extern void* Game_UpdateRoomFunc;

extern void* Game_PoisonCloudFunc;
extern void* Game_IsEnemyFunc;

extern void* Game_SpawnBlueFliesFunc;

extern void* Player_AddCostumeFunc;