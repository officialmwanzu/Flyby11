// CpuCheck.cpp
#include "pch.h"
#include <intrin.h>

extern "C" __declspec(dllexport) bool __cdecl HasPopcnt()
{
    int cpuInfo[4] = { 0 };
    __cpuid(cpuInfo, 1);
    // Check if the processor supports SSE4.2 (bit 20 of the ECX register)
    bool hasSSE42 = (cpuInfo[2] & (1 << 20)) != 0;
    // Check if the processor supports POPCNT (bit 23 of the ECX register)
    bool hasPOPCNT = (cpuInfo[2] & (1 << 23)) != 0;

    return hasSSE42 && hasPOPCNT;
}
