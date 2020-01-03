﻿using Miki.Localization;
using Miki.Localization.Exceptions;
using Miki.Localization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miki.Services.Blackjack.Exceptions
{
	public class BlackjackSessionNullException : LocalizedException
	{
		public override IResource LocaleResource 
			=> new LanguageResource("error_blackjack_null");
	}
}
