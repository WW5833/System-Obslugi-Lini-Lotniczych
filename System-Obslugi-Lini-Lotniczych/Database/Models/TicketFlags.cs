using System;

namespace LotSystem.Database.Models;

[Flags]
public enum TicketFlags
{
    NONE = 0,
    ADDITIONAL_LUGGAGE = 1 << 0,
    ECONOMY_TICKET = 1 << 1,
    BUSINESS_TICKET = 1 << 2,
}