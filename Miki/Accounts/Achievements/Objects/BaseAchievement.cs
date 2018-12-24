﻿using Miki.Accounts.Achievements.Objects;
using Miki.Discord.Common;
using Miki.Framework;
using Miki.Helpers;
using Miki.Models;
using System;
using System.Threading.Tasks;

namespace Miki.Accounts.Achievements
{
	public class BaseAchievement
	{
		public string Name { get; set; } = Constants.NotDefined;
		public string ParentName { get; set; } = Constants.NotDefined;

		public string Icon { get; set; } = Constants.NotDefined;
		public int Points { get; set; } = 5;

		public BaseAchievement()
		{
		}

		public BaseAchievement(Action<BaseAchievement> act)
		{
			act.Invoke(this);
		}

		public virtual async Task<bool> CheckAsync(BasePacket packet)
		{
			await Task.Yield();
			return false;
		}

		/// <summary>
		/// Unlocks the achievement and if not yet added to the database, It'll add it to the database.
		/// </summary>
		/// <param name="context">sql context</param>
		/// <param name="id">user id</param>
		/// <param name="r">rank set to (optional)</param>
		/// <returns></returns>
		internal async Task UnlockAsync(IDiscordChannel channel, IDiscordUser user, int r = 0)
		{
			long userid = user.Id.ToDbLong();

			if (await UnlockIsValid(userid, r))
			{
				await AchievementManager.Instance.CallAchievementUnlockEventAsync(this, user, channel);
				Notification.SendAchievement(this, channel, user);
			}
		}

		internal async Task UnlockAsync(IDiscordUser user, int r = 0)
		{
			long userid = user.Id.ToDbLong();

			if (await UnlockIsValid(userid, r))
			{
				await Notification.SendAchievementAsync(this, user);
			}
		}

		internal async Task<bool> UnlockIsValid(long userId, int newRank)
		{
			using (var context = new MikiContext())
			{
				var achievement = await DatabaseHelpers.GetAchievementAsync(context, userId, ParentName);

				// If no achievement has been found and want to unlock first
				if (achievement == null && newRank == 0)
				{
					achievement = context.Achievements.Add(new Achievement()
					{
						UserId = userId,
						Name = ParentName,
						Rank = 0
					}).Entity;

					await DatabaseHelpers.UpdateCacheAchievementAsync(userId, Name, achievement);
					await context.SaveChangesAsync();
					return true;
				}
				// If achievement we want to unlock is the next achievement
				if (achievement != null)
				{
					if (achievement.Rank == newRank - 1)
					{
						achievement.Rank++;
					}
					else
					{
						return false;
					}

					await DatabaseHelpers.UpdateCacheAchievementAsync(userId, ParentName, achievement);
					await context.SaveChangesAsync();
					return true;
				}
			}
			return false;
		}
	}
}