﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Miki.Bot.Models.Exceptions;
using Miki.Functional;
using Miki.Services.Scheduling;

namespace Miki.Services.Lottery
{
    public interface ILotteryService
    {
        /// <summary>
        /// Individual entry price to enter this lottery.
        /// </summary>
        int EntryPrice { get; }

        int WinningAmount { get; }

        /// <summary>
        /// Gets the specific user's entries. Throws an exception <see cref="EntityNullException{T}"/>
        /// if not found.
        /// </summary>
        ValueTask<Result<LotteryEntry>> GetEntriesForUserAsync(long userId);

        ValueTask<IEnumerable<LotteryEntry>> GetEntriesAsync();

        /// <summary>
        /// Attempts to purchase entries for the user and updates the user's entries appropriately.
        /// </summary>
        ValueTask<LotteryEntry> PurchaseEntriesAsync(long userId, int amountOfTickets);

        ValueTask<TaskPayload> GetLotteryTaskAsync();

        ValueTask<int> GetTotalPrizeAsync();
    }
}