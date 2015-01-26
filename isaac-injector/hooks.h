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
#define GAME_EVENT_CHANGEROOM		0x07

extern char* eventMasks[];

// Hooks
void TakePillEvent_Hook();
void AddCollectibleEvent_Hook();
char SpawnEntityEvent_Hook();
int HpUpEvent_Hook();
void AddSoulHeartsEvent_Hook();
void ShootTearsEvent_Hook();

// functions
using IsaacRandomFuncType = unsigned int __cdecl(void);
extern IsaacRandomFuncType* IsaacRandomFunc;

using GoodPillEffectFuncType = int __stdcall(Player*);
extern GoodPillEffectFuncType* GoodPillEffectFunc;

using InitTearFuncType = TearInfo* __cdecl (int, TearInfo*);
extern InitTearFuncType* InitTearFunc;

using Player_TeleportFuncType = void (void);
extern Player_TeleportFuncType* Player_TeleportFunc;

// initialization + playermanager
bool Hooks_Init();
PlayerManager* Hooks_GetPlayerManager();