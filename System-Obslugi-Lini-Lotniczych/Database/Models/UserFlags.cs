using System;

namespace LotSystem.Database.Models;

[Flags]
public enum UserFlags : byte
{
    NONE = 0,
    EMAIL_VERIFIED = 1 << 0,
    SUSPENDED = 1 << 1,
    TWO_FACTOR_PHONE_NUMBER_CALL = 1 << 2,
    TWO_FACTOR_PHONE_NUMBER_SMS = 1 << 3,
    TWO_FACTOR_EXTERNAL = 1 << 4,
}