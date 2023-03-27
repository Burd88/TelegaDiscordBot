using System;
using static DiscordBot.Functions;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CharInfo
    {

        private CharInfoAll _charInfo;

        public CharInfoAll GetCharInfo(string name)
        {
            _charInfo = new();
            GetLinkForChar(name);
            if (!_charInfo.Error)
            {
                return _charInfo;
            }
            else
            {
                return null;
            }
        }
        private void GetLinkForChar(string text)
        {
            if (text.Contains("-"))
            {
                string[] str = text.Split("-");
                foreach (RealmList rlm in Functions.Realms)
                {
                    if (rlm.Name.ToLower() == str[1].ToLower())
                    {
                        GetCharacterMainInfo(str[0], rlm.Slug);
                    }
                }
            }
            else
            {
                GetCharacterMainInfo(text, settings.RealmSlug);
            }
        }
        private void GetCharacterMainInfo(string name, string realm)
        {
            string mainLink = $"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}?namespace=profile-eu&locale={settings.Locale}&access_token={tokenWow}";
            CharFullInfo character = GetWebJson<CharFullInfo>(mainLink);
            if (character != null)
            {
                if (character.equipped_item_level.ToString() == "error")
                {
                    _charInfo.ILvl = "0";
                }
                else
                {
                    _charInfo.ILvl = character.equipped_item_level.ToString();
                }
                _charInfo.Name = character.name;
                if (character.active_spec != null)
                {
                    _charInfo.Spec = character.active_spec.name;

                }
                if (character.character_class != null)
                {
                    _charInfo.Class = character.character_class.name;
                }
                if (character.guild != null)
                {
                    _charInfo.Guild = character.guild.name;
                }
                _charInfo.Race = character.race.name;
                _charInfo.Lvl = character.level.ToString();

                _charInfo.LastLogin = Relative_time(FromUnixTimeStampToDateTime(character.last_login_timestamp.ToString()));
                if (character.covenant_progress != null)
                {
                    _charInfo.Coven = character.covenant_progress.chosen_covenant.name + " (" + character.covenant_progress.renown_level.ToString() + ")";
                }

                //string soulbindsLink = $"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/soulbinds?namespace=profile-eu&locale={settings.Locale}&access_token={tokenWow}";
                string raidLink = $"https://raider.io/api/v1/characters/profile?region=eu&realm={realm}&name={Char.ToUpper(name[0]) + name.Substring(1).ToLower()}&fields=mythic_plus_scores%2Craid_progression";
                string mediaLink = $"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/character-media?namespace=profile-eu&locale={settings.Locale}&access_token={tokenWow}";
                string setLink = $"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/equipment?namespace=profile-eu&locale={settings.Locale}&access_token={tokenWow}";
                string statsLink = $"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/statistics?namespace=profile-eu&locale={settings.Locale}&access_token={tokenWow}";
                //GetSoulbindsCharacter(soulbindsLink);
                Character_raid_progress(raidLink);
                GetCharMedia(mediaLink, realm, name);
                GetCharSet(setLink);
                GetCharStats(statsLink);
            }
            else
            {
                _charInfo.Error = true;
            }
        }

        private void GetSoulbindsCharacter(string link)
        {
            CharacterSoulbinds allSoulbinds = GetWebJson<CharacterSoulbinds>(link);
            if (allSoulbinds != null)
            {
                if (allSoulbinds.soulbinds != null)
                {
                    foreach (Soulbinds soulbinds in allSoulbinds.soulbinds)
                    {
                        if (soulbinds.is_active == true)
                        {
                            _charInfo.CovenSoul = soulbinds.soulbind.name;
                        }
                    }
                }
            }
        }
        private void Character_raid_progress(string link)
        {

            RaiderIOCharInfo character = GetWebJson<RaiderIOCharInfo>(link);
            if (character != null)
            {
                _charInfo.RaidProgress = character.raid_progression.VaultOfTheIncarnates.summary;
                _charInfo.MythicPlus = character.mythic_plus_scores.all;
            }

        }
        private void GetCharMedia(string link, string realm, string name)
        {
            CharMedia charMedia = GetWebJson<CharMedia>(link);
            if (charMedia != null)
            {
                foreach (Asset media in charMedia.assets)
                {
                    // Console.WriteLine(media.key);
                    if (media.key == "main-raw")
                        _charInfo.ImageCharMainRaw = media.value;
                    else if (media.key == "main")
                        _charInfo.ImageCharMain = media.value;
                    else if (media.key == "avatar")
                        _charInfo.ImageCharAvatar = media.value;
                    else if (media.key == "inset")
                        _charInfo.ImageCharInset = media.value;
                }
                string lnk = $"https://worldofwarcraft.com/ru-ru/character/eu/{realm}/{name.ToLower()}";
                _charInfo.LinkBnet = lnk;
            }
        }
        private void GetCharSet(string link)
        {
            CharEquip charEquip = GetWebJson<CharEquip>(link);
            if (charEquip != null)
            {
                if (charEquip.equipped_item_sets != null)
                {
                    foreach (EquippedItemSet setequip in charEquip.equipped_item_sets)
                    {
                        if (setequip.display_string.Contains("/5)"))
                        {
                            _charInfo.SetcountItem = "Set:**" + setequip.display_string.Replace(setequip.item_set.name, "") + "**";
                            _charInfo.SetNameItem += $"**{setequip.display_string}**\n";
                        }
                        
                    }
                   
                }
            }
        }
        private void GetCharStats(string link)
        {
            CharStats charStats = GetWebJson<CharStats>(link);
            if (charStats != null)
            {
                double crit = charStats.melee_crit.value;
                crit = Math.Round(crit, 1);
                double haste = charStats.melee_haste.value;
                haste = Math.Round(haste, 1);
                double mastery = charStats.mastery.value;
                mastery = Math.Round(mastery, 1);
                double versality_damage = charStats.versatility_damage_done_bonus;
                versality_damage = Math.Round(versality_damage, 1);
                double versality_healing = charStats.versatility_damage_taken_bonus;
                versality_healing = Math.Round(versality_healing, 1);
                _charInfo.Stats = _charInfo.Stats + "\n" + "  **Критический удар**:  " + crit.ToString() + "%\n  **Скорость**:  " + haste.ToString() + "%\n"
                    + "  **Искусность**:  " + mastery.ToString() + "%\n  **Универсальность**:  " + versality_damage.ToString() + "% / " + versality_healing.ToString() + "%";

            }
        }
    }
    //  public class Asset
    //  {
    //      public string key { get; set; }
    //     public string value { get; set; }
    //   }


}
