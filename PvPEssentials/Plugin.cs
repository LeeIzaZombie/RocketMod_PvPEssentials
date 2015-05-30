using System;
using Rocket.Unturned;
using Rocket.API;
using UnityEngine;
using System.IO;
using System.Timers;
using System.Threading;
using System.Collections.Generic;
using Rocket.Unturned.Plugins;
using SDG;

namespace PvPEssentials
{
    public class PluginEvents : RocketPlugin<PvpEConfig>
    {

        #region Declarations.
        public string Name
        {
            get { return "pvpessentials"; }
        }
        #endregion

        void RocketPlayerEvents_OnPlayerUpdateWater(Rocket.Unturned.Player.RocketPlayer player, byte water) //Untested
        {
            if (this.Configuration.ShowHungerThirstAlerts)
            {
                if (water == 50)
                {
                    RocketChat.Say(player.CSteamID, this.Configuration.thirst50, Color.red);
                }
                else if (water == 25)
                {
                    RocketChat.Say(player.CSteamID, this.Configuration.thirst25, Color.red);
                }
            }
        }
        void RocketPlayerEvents_OnPlayerUpdateFood(Rocket.Unturned.Player.RocketPlayer player, byte food) //Untested
        {
            if (this.Configuration.ShowHungerThirstAlerts)
            {
                if (food == 50)
                {
                    RocketChat.Say(player.CSteamID, this.Configuration.hunger50, Color.red);
                }
                else if (food == 25)
                {
                    RocketChat.Say(player.CSteamID, this.Configuration.hunger25, Color.red);
                }
            }
        }
        void RocketPlayerEvents_OnPlayerDeath(Rocket.Unturned.Player.RocketPlayer player, SDG.EDeathCause cause, SDG.ELimb limb, Steamworks.CSteamID murderer)
        {
            if (this.Configuration.ShowDeathCause)
            {
                if (cause == SDG.EDeathCause.ZOMBIE)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration.DeathByZombie, Color.red);
                }
                else if (cause == SDG.EDeathCause.BONES)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration.DeathByFallDmg, Color.red);
                }
                else if (cause == SDG.EDeathCause.BREATH)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration.DeathByDrown, Color.red);
                }
                else if (cause == SDG.EDeathCause.WATER)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration.DeathBy0Water, Color.red);
                }
                else if (cause == SDG.EDeathCause.FOOD)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration.DeathBy0Food, Color.red);
                }
                else if (cause == SDG.EDeathCause.SHRED)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration, Color.red);
                }
                else if (cause == SDG.EDeathCause.BLEEDING)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration, Color.red);
                }
                else if (cause == SDG.EDeathCause.FREEZING)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration, Color.red);
                }
                else if (cause == SDG.EDeathCause.INFECTION)
                {
                    RocketChat.Say(player.CharacterName + " " + this.Configuration, Color.red);
                }
            }
            if (this.Configuration.PvPExpforkills)
            {
                string c = "";
                if (limb == ELimb.SKULL)
                {
                    c = " in the head to death!";
                    player.Experience += (uint)this.Configuration.ExpForHeadshot;
                }
                else if (limb == ELimb.SPINE)
                {
                    c = " in the body to death!";
                    player.Experience += (uint)this.Configuration.ExpForBodyShot;
                }
                else if (limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_ARM || limb == ELimb.LEFT_HAND || limb == ELimb.RIGHT_HAND)
                {
                    c = " in the arm to death!";
                    player.Experience += (uint)this.Configuration.ExpForArmShot;
                }
                else if (limb == ELimb.LEFT_FOOT || limb == ELimb.RIGHT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_LEG)
                {
                    c = " in the leg to death!";
                    player.Experience += (uint)this.Configuration.ExpForLegshot;
                }

                if (cause == EDeathCause.GUN)
                {
                    if (this.Configuration.ShowDeathCause)
                        RocketChat.Say(Rocket.Unturned.Player.RocketPlayer.FromCSteamID(murderer).CharacterName + " just shot " + player.CharacterName + c, Color.red);
                }
                else if (cause == EDeathCause.MELEE)
                {
                    if (this.Configuration.ShowDeathCause)
                        RocketChat.Say(Rocket.Unturned.Player.RocketPlayer.FromCSteamID(murderer).CharacterName + " just melee'd " + player.CharacterName + c, Color.red);
                }
                else if (cause == EDeathCause.PUNCH)
                {
                    if (this.Configuration.ShowDeathCause)
                        RocketChat.Say(Rocket.Unturned.Player.RocketPlayer.FromCSteamID(murderer).CharacterName + " just punched " + player.CharacterName + c, Color.red);
                }
                else if (cause == EDeathCause.ROADKILL)
                {
                    if (this.Configuration.ShowDeathCause)
                        RocketChat.Say(Rocket.Unturned.Player.RocketPlayer.FromCSteamID(murderer).CharacterName + " just roadkilled " + player.CharacterName, Color.red);
                }
            }
        }

        protected override void Load()
        {
            Rocket.Unturned.Events.RocketPlayerEvents.OnPlayerDeath += RocketPlayerEvents_OnPlayerDeath;
            Rocket.Unturned.Events.RocketPlayerEvents.OnPlayerUpdateFood += RocketPlayerEvents_OnPlayerUpdateFood;
            Rocket.Unturned.Events.RocketPlayerEvents.OnPlayerUpdateWater += RocketPlayerEvents_OnPlayerUpdateWater;
        }
    }

    public class PvpEConfig : IRocketPluginConfiguration
    {
        public bool ShowHungerThirstAlerts, ShowDeathCause, PvPExpforkills;
        public string hunger50, hunger25, thirst50, thirst25;

        public int ExpForHeadshot, ExpForBodyShot, ExpForArmShot, ExpForLegshot;

        public string DeathByZombie, DeathBy0Food, DeathBy0Water, DeathByDrown, DeathByFallDmg, DeathByInfect, DeathBy0Freeze, DeathBy0Bleed, DeathByTrap;

        public IRocketPluginConfiguration DefaultConfiguration
        {
            get
            {
                return new PvpEConfig()
                {
                    ShowHungerThirstAlerts = true,
                    hunger50 = "You're starting to feel a bit hungry.",
                    hunger25 = "You are going to starve soon!",
                    thirst50 = "You're starting to feel a bit thirsty.",
                    thirst25 = "You are going to dehydrate soon!",
                    ShowDeathCause = true,
                    DeathByZombie = "just got rekt by a zombie!",
                    DeathBy0Food = "just starved to death!",
                    DeathBy0Water = "dehydrated to death!",
                    DeathByDrown = "decided to swim with the fish!",
                    DeathByFallDmg = "took a dive off a building and died!",
                    DeathByInfect = "died from the zombie infection!",
                    DeathBy0Freeze = "died from frostbite!",
                    DeathBy0Bleed = "bled to death!",
                    DeathByTrap = "got rekt by a trap!",
                    PvPExpforkills = true,
                    ExpForHeadshot = 20,
                    ExpForBodyShot = 15,
                    ExpForArmShot = 10,
                    ExpForLegshot = 10
                };
            }
        }
    }
}
