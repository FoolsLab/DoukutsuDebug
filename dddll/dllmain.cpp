// dllmain.cpp : DLL アプリケーションのエントリ ポイントを定義します。
#include "stdafx.h"
#pragma comment(lib, "shlwapi.lib")
//#pragma comment(lib, "hwbrk32.lib")

#pragma pack(1)
struct CSSprite
{
	int Existence;
	int CollisionFlag;
	int X;
	int Y;
	int VelX;
	int VelY;
	int State1;
	int State2;
	int State3;
	int State4;
	int SpriteType;
	int FlagID;
	int EventID;
	int Image;
	int DefeatedSE;
	int DamagedSE;
	int HP;
	int Exp;
	int DefeatedEffect;
	int Direction;
	int DetailsFlag;
	RECT SrcRect;
	int AnimTimer;
	int AnimIndex;
	int LifeTimer;
	int State5;
	int ActionState;
	int ActionTimer;
	RECT CollisionRect;
	RECT ViewRect;
	int FrameCounter;
	int DamageCounter;
	int DamageToTouch;
	CSSprite* Parent;
};

#pragma pack(4)
struct CSBulletShot
{
	int Collision;
	int Type;
	int Flags;
	int Exist;
	int PosX;
	int PosY;
	int Data[8];
	int Direction;
	RECT SrcRect;
	int Timer;
	int Unknown;
	int TTL;
	int Damage;
	int hitNum;
	RECT CollisionRect;
	int ViewLenFront;
	int ViewWidth;
	int ViewLenBack;
	int Unknown2;
};

#pragma pack(4)
struct CSPossessBullet
{
	int type;
	int lv;
	int exp;
	int shotNumMax;
	int shotNum;
};

#pragma pack(1)
struct CSData
{
	int MyCharX;
	int MyCharY;
	int MyCharVelX;
	int MyCharVelY;
	int CameraX;
	int CameraY;
	int MyCharState;
	int MyCharAir;
	short MyCharDamageTimer;
	int BoosterFuel;
	unsigned char ShotConsumeTimer;
	int Frame;
	char BoostInfo;
	int MyCharCollisionState;
	int ScriptNowLocation;
	int ScriptStr;
	char ScriptBuf[16];
	int EventTimer;
	int _290Timer;
	unsigned int holdrand;
	int EffectExist[64];
	int numberEffectExist[16];
	CSSprite SpriteDB[512];
	CSSprite MBSpriteDB[16];
	CSBulletShot BulletShotDB[64];
	byte TileMapDB[307200];
	byte TileTypeDB[256];
	short MapWidth;
	short MapHeight;
	CSPossessBullet PossessBulletDB[8];
	int BulletMaxExpTbl[42];
	int UsingBulletIndex;
	short MyCharHP;
	short MyCharMaxHP;
};

void WriteCC(HANDLE hPrc)
{
	unsigned __int8 cc = 0xCC;
	SIZE_T sz;
	WriteProcessMemory(hPrc, reinterpret_cast<void*>(0x0040B340U), &cc, 1, &sz);
	return;
}

void FixCC(HANDLE hPrc)
{
	unsigned __int8 orig = 0x55;
	SIZE_T sz;
	WriteProcessMemory(hPrc, reinterpret_cast<void*>(0x40B340U), &orig, 1, &sz);
}

std::pair<std::unique_ptr<unsigned __int8[]>, size_t>
BuildRemoteCode(char* codeStr, void* pGift)
{
	int sz = 0;
	char* p = codeStr;

	while (true)
	{
		if (*p == '\0')
		{
			break;
		}

		if (isxdigit(*p) && isxdigit(*(p + 1)))
		{
			sz += 1;
			p += 2;
			continue;
		}

		if (*p == '%')
		{
			switch (*(p + 1))
			{
			case 'p':
				sz += sizeof(pGift);
				break;
			default:
				break;
			}
			p += 2;
			continue;
		}

		p++;
	}

	std::unique_ptr<unsigned __int8[]> code(new unsigned __int8[sz]);
	p = codeStr;
	auto pd = code.get();

	while (true)
	{
		if (*p == '\0')
		{
			break;
		}

		if (isxdigit(*p) && isxdigit(*(p + 1)))
		{
			char buf[3] = { *(p + 0), *(p + 1), '\0' };

			*pd = static_cast<unsigned __int8>(strtol(buf, NULL, 16));
			pd++;
			p += 2;
			continue;
		}

		if (*p == '%')
		{
			switch (*(p + 1))
			{
			case 'p':
				*(reinterpret_cast<decltype(pGift)*>(pd)) = pGift;
				pd += sizeof(pGift);
				break;
			default:
				break;
			}
			p += 2;
			continue;
		}

		p++;
	}

	return { std::move(code), sz };
}

DWORD GetMainThread(DWORD Pid)
{
	struct
	{
		const DWORD Pid;
		DWORD Tid;
	} tmpDat = {Pid, 0};
	
	EnumWindows(
		[](HWND hWnd, LPARAM lp) -> BOOL
		{
			DWORD WndPid;
			DWORD tmpTid = GetWindowThreadProcessId(hWnd, &WndPid);
			
			auto pTmpDat = reinterpret_cast<decltype(tmpDat)*>(lp);

			if (
				WndPid == pTmpDat->Pid &&
				GetWindow(hWnd, GW_OWNER) == NULL &&
				IsWindowVisible(hWnd))
			{
				pTmpDat->Tid = tmpTid;
				return FALSE;
			}
			else
			{
				return TRUE;
			}
		},
		reinterpret_cast<LPARAM>(&tmpDat));

	return tmpDat.Tid;
}

DLLEXPORT void Detach(HANDLE hPrc, DWORD Pid)
{
	FixCC(hPrc);
	DebugActiveProcessStop(Pid);
	CloseHandle(hPrc);
}

DLLEXPORT DWORD FindCaveStory()
{
	HANDLE hSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

	if (hSnapShot == INVALID_HANDLE_VALUE)
	{
		return NULL;
	}

	PROCESSENTRY32 process;

	process.dwSize = sizeof(process);

	Process32First(hSnapShot, &process);

	bool flag = false;

	do {
		TCHAR buf[512];
		_tcscpy_s(buf, process.szExeFile);	// TO FIX
		PathStripPath(buf);

		if (_tcscmp(buf, _T("Doukutsu.exe")) == 0)
		{
			flag = true;
			break;
		}
		if (_tcscmp(buf, _T("doukutsu.exe")) == 0)
		{
			flag = true;
			break;
		}
	} while (Process32Next(hSnapShot, &process));

	CloseHandle(hSnapShot);

	if (!flag)
	{
		return NULL;
	}

	return process.th32ProcessID;
}

DLLEXPORT HANDLE OpenCaveStory(DWORD Pid)
{
	auto hPrc = OpenProcess(PROCESS_ALL_ACCESS, FALSE, Pid);
	if (hPrc == NULL)
	{
		return NULL;
	}

	if (DebugActiveProcess(Pid) == 0)
	{
		return NULL;
	}
	DebugSetProcessKillOnExit(FALSE);

	while (1)
	{
		DEBUG_EVENT de;
		WaitForDebugEvent(&de, INFINITE);
		if (de.dwDebugEventCode == EXCEPTION_DEBUG_EVENT)
		{
			WriteCC(hPrc);
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
			break;
		}
		else
		{
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
		}
	}

	return hPrc;
}

DLLEXPORT DWORD GetTls(HANDLE hPrc, DWORD Pid)
{
	char Thief[] =
	{
		"FF 35 88 90 49 00"	// push TlsIndex
		"FF 15 44 C9 4B 00"	// call GetTlsValue
		"A3 %p"	// mov [p], eax
		"CC"	// int3 Interrupt
	};
	auto ThiefHouse = VirtualAllocEx(hPrc, NULL, 512, MEM_RESERVE | MEM_COMMIT, PAGE_EXECUTE_READWRITE);
	auto[ThiefCode, ThiefSz] = BuildRemoteCode(Thief, ThiefHouse);
	LPVOID ThiefGift;
	DWORD ptls;

	auto Tid = GetMainThread(Pid);
	auto hTh = OpenThread(THREAD_ALL_ACCESS, false, Tid);

	while (1)
	{
		DEBUG_EVENT de;
		auto res = WaitForDebugEvent(&de, INFINITE);
		if (de.dwDebugEventCode == EXCEPTION_DEBUG_EVENT &&
			reinterpret_cast<unsigned int>(de.u.Exception.ExceptionRecord.ExceptionAddress) == 0x40B340U)
		{
			SIZE_T sz;
			WriteProcessMemory(hPrc, static_cast<BYTE*>(ThiefHouse) + sizeof(ThiefGift), ThiefCode.get(), ThiefSz, &sz);

			CONTEXT ct;
			ct.ContextFlags = CONTEXT_CONTROL;
			GetThreadContext(hTh, &ct);
			ct.Eip = reinterpret_cast<DWORD>(ThiefHouse) + sizeof(ThiefGift);
			SetThreadContext(hTh, &ct);
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
			break;
		}
		else
		{
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
		}
	}

	while (1)
	{
		DEBUG_EVENT de;
		auto res = WaitForDebugEvent(&de, INFINITE);
		if (de.dwDebugEventCode == EXCEPTION_DEBUG_EVENT &&
			de.u.Exception.ExceptionRecord.ExceptionAddress == static_cast<BYTE*>(ThiefHouse) + sizeof(ThiefGift) + ThiefSz - 1)
		{
			FixCC(hPrc);

			SIZE_T sz;
			ReadProcessMemory(hPrc, ThiefHouse, &ptls, 4, &sz);

			VirtualFreeEx(hPrc, ThiefHouse, 0, MEM_DECOMMIT | MEM_RELEASE);

			CONTEXT ct;
			ct.ContextFlags = CONTEXT_CONTROL;
			GetThreadContext(hTh, &ct);
			ct.EFlags |= 0x00000100; // TrapFlag
			ct.Eip = 0x40B340U;
			SetThreadContext(hTh, &ct);

			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
		}
		else if (de.dwDebugEventCode == EXCEPTION_DEBUG_EVENT &&
			de.u.Exception.ExceptionRecord.ExceptionCode == EXCEPTION_SINGLE_STEP)
		{
			WriteCC(hPrc);
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
			break;
		}
		else
		{
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
		}
	}

	CloseHandle(hTh);
	return ptls;
}

DLLEXPORT int isAlive(HANDLE hPrc)
{
	DWORD ec;
	GetExitCodeProcess(hPrc, &ec);
	return ec == STILL_ACTIVE;
}

DLLEXPORT void GetCSData(HANDLE hPrc, CSData* dat, void* tls)
{
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E654), &dat->MyCharX, sizeof(dat->MyCharX), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E658), &dat->MyCharY, sizeof(dat->MyCharY), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E66C), &dat->MyCharVelX, sizeof(dat->MyCharVelX), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E670), &dat->MyCharVelY, sizeof(dat->MyCharVelY), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E1C8), &dat->CameraX, sizeof(dat->CameraX), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E1CC), &dat->CameraY, sizeof(dat->CameraY), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E638), &dat->MyCharState, sizeof(dat->MyCharState), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6DC), &dat->MyCharAir, sizeof(dat->MyCharAir), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6C8), &dat->MyCharDamageTimer, sizeof(dat->MyCharDamageTimer), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6E8), &dat->BoosterFuel, sizeof(dat->BoosterFuel), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4A5564), &dat->ShotConsumeTimer, sizeof(dat->ShotConsumeTimer), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E1EC), &dat->Frame, sizeof(dat->Frame), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6E6), &dat->BoostInfo, sizeof(dat->BoostInfo), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E63C), &dat->MyCharCollisionState, sizeof(dat->MyCharCollisionState), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4A5AFC), &dat->EventTimer, sizeof(dat->EventTimer), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6F4), &dat->_290Timer, sizeof(dat->_290Timer), NULL);
	ReadProcessMemory(hPrc, static_cast<BYTE*>(tls) + 0x14, &dat->holdrand, sizeof(dat->holdrand), NULL);

	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4A5AE0), &dat->ScriptNowLocation, sizeof(dat->ScriptNowLocation), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4A5AD8), &dat->ScriptStr, sizeof(dat->ScriptStr), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(dat->ScriptStr + dat->ScriptNowLocation), dat->ScriptBuf, sizeof(dat->ScriptBuf), NULL);
	dat->ScriptBuf[15] = '\0';

	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4A6220), dat->SpriteDB, sizeof(dat->SpriteDB), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4BBA58), dat->MBSpriteDB, sizeof(dat->MBSpriteDB), NULL);

	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x499C98), dat->BulletShotDB, sizeof(dat->BulletShotDB), NULL);

	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x499BC8), dat->PossessBulletDB, sizeof(dat->PossessBulletDB), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x499C68), &dat->UsingBulletIndex, sizeof(dat->UsingBulletIndex), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x493660), dat->BulletMaxExpTbl, sizeof(dat->BulletMaxExpTbl), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6CC), &dat->MyCharHP, sizeof(dat->MyCharHP), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E6D0), &dat->MyCharMaxHP, sizeof(dat->MyCharMaxHP), NULL);

	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E586), &dat->MapWidth, sizeof(dat->MapWidth), NULL);
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E588), &dat->MapHeight, sizeof(dat->MapHeight), NULL);
	{
		void* tmp;
		ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E480), &tmp, sizeof(tmp), NULL);
		ReadProcessMemory(hPrc, tmp, dat->TileMapDB, dat->MapWidth * dat->MapHeight, NULL);
	}
	ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49E484), dat->TileTypeDB, sizeof(dat->TileTypeDB), NULL);

	for (size_t i = 0; i < 64; i++)
	{
		ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x49BCA8 + 0x44 * i), &(dat->EffectExist[i]), sizeof(dat->EffectExist[0]), NULL);
	}

	for (size_t i = 0; i < 16; i++)
	{
		ReadProcessMemory(hPrc, reinterpret_cast<void*>(0x4A5F98 + 0x28 * i), &(dat->numberEffectExist[i]), sizeof(dat->numberEffectExist[0]), NULL);
	}
}

DLLEXPORT bool WaitFrame(HANDLE hPrc, DWORD* pid, DWORD* tid, DWORD* optDat)
{
	while (1)
	{
		DEBUG_EVENT de;
		auto res = WaitForDebugEvent(&de, 100);
		if (res == FALSE)	// No debug events occured
		{
			return false;
		}
		if (de.dwDebugEventCode == EXCEPTION_DEBUG_EVENT && 
			reinterpret_cast<unsigned int>(de.u.Exception.ExceptionRecord.ExceptionAddress) == 0x40B340U)
		{
			*pid = de.dwProcessId;
			*tid = de.dwThreadId;
			*optDat = 0;	// reserved

			FixCC(hPrc);

			auto hTh = OpenThread(THREAD_ALL_ACCESS, false, de.dwThreadId);
			CONTEXT ct;
			ct.ContextFlags = CONTEXT_CONTROL;
			GetThreadContext(hTh, &ct);
			ct.EFlags |= 0x00000100; // TrapFlag
			ct.Eip = 0x40B340U;
			SetThreadContext(hTh, &ct);
			CloseHandle(hTh);

			return true;
		}
		else if (de.dwDebugEventCode == EXCEPTION_DEBUG_EVENT &&
			de.u.Exception.ExceptionRecord.ExceptionCode == EXCEPTION_SINGLE_STEP)
		{
			WriteCC(hPrc);
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
		}
		else
		{
			ContinueDebugEvent(de.dwProcessId, de.dwThreadId, DBG_CONTINUE);
			return false;
		}
	}
}

DLLEXPORT void ContinueFrame(DWORD pid, DWORD tid)
{
	ContinueDebugEvent(pid, tid, DBG_CONTINUE);
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

