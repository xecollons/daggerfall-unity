﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2017 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    
// 
// Notes:
//

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

namespace DaggerfallWorkshop.Game.Questing.Actions
{
    /// <summary>
    /// Triggers when a Foe has been injured.
    /// Will not fire if Foe dies immediately (e.g. player one-shots enemy).
    /// </summary>
    public class InjuredFoe : ActionTemplate
    {
        Symbol foeSymbol;

        public override string Pattern
        {
            get { return @"injured (?<aFoe>[a-zA-Z0-9_.-]+)"; }
        }

        public InjuredFoe(Quest parentQuest)
            : base(parentQuest)
        {
            IsTriggerCondition = true;
        }

        public override IQuestAction CreateNew(string source, Quest parentQuest)
        {
            base.CreateNew(source, parentQuest);

            // Source must match pattern
            Match match = Test(source);
            if (!match.Success)
                return null;

            // Factory new action
            InjuredFoe action = new InjuredFoe(parentQuest);
            action.foeSymbol = new Symbol(match.Groups["aFoe"].Value);

            return action;
        }

        public override bool CheckTrigger(Task caller)
        {
            // Get related Foe resource
            Foe foe = ParentQuest.GetFoe(foeSymbol);
            if (foe == null)
                return false;

            // Check injured flag
            if (foe.InjuredTrigger)
            {
                return true;
            }

            return false;
        }
    }
}