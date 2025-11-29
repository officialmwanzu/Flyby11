#include "pch.h"
#include <intrin.h>

extern "C" __declspec(dllexport) bool __cdecl HasPopcnt()
{
    int cpuInfo[4] = { 0 };

    // Get CPU info by calling __cpuid, which fills the cpuInfo array with CPU feature flags
    __cpuid(cpuInfo, 1);

    // Check if the processor supports SSE4.2 (bit 20 of the ECX register)
    bool hasSSE42 = (cpuInfo[2] & (1 << 20)) != 0;

    // Check if the processor supports POPCNT (bit 23 of the ECX register)
    bool hasPOPCNT = (cpuInfo[2] & (1 << 23)) != 0;

    // POPCNT is the critical instruction for Windows 11 upgrade compatibility!!
    // If both SSE4.2 and POPCNT are supported, yeah we can return true; otherwise, false 
    return hasSSE42 && hasPOPCNT;
}
