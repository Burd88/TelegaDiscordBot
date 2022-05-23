using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using static DiscordBot.Functions;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CharInfo
    {

        private CharInfoAll _charInfo;
        // private static string name = "";
        // private static string realm ="";
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

                        Character_info(str[0], rlm.Slug);


                    }
                }

            }
            else
            {

                Character_info(text, settings.RealmSlug);








            }
        }
        private void Character_info(string name, string realm)
        {

            try
            {

                WebRequest requestchar = WebRequest.Create($"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}?namespace=profile-eu&locale=ru_RU&access_token={tokenWow}");

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {
                            Root_charackter_full_info character = JsonConvert.DeserializeObject<Root_charackter_full_info>(line);


                            if (character.equipped_item_level.ToString() == "error")
                            {
                                _charInfo.ILvl = "0";
                            }
                            else
                            {
                                _charInfo.ILvl = character.equipped_item_level.ToString();
                            }
                            _charInfo.Name =character.name;
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

                            _charInfo.LastLogin = relative_time(FromUnixTimeStampToDateTime(character.last_login_timestamp.ToString()));
                            if (line.Contains("\"covenant_progress\":"))
                            {
                                _charInfo.Coven = GetCoven(character.covenant_progress.chosen_covenant.id.ToString()) + " (" + character.covenant_progress.renown_level.ToString() + ")";
                            }

                            GetSoulbindsCharacter(character.name, realm);
                            Character_raid_progress(character.name, realm);
                            GetCharMedia(character.name, realm);
                            GetCharSet(character.name, realm);
                            GetCharStats(character.name, realm);
                        }
                    }
                }
                responcechar.Close();
                _charInfo.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _charInfo.Error = true;
                    string message = $"\nGetCharInvLeave Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _charInfo.Error = true;
                string message = $"GetCharInvLeave Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
        private string GetCoven(string id)
        {
            if (id == "1")
            {
                return "Кирии";
            }
            else if (id == "2")
            {
                return "Вентиры";
            }
            else if (id == "3")
            {
                return "Ночной народец";
            }
            else if (id == "4")
            {
                return "Некролорды";
            }
            return "";
        }

        private void GetSoulbindsCharacter(string name, string realm)
        {

            try
            {
                WebRequest request = WebRequest.Create($"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/soulbinds?namespace=profile-eu&locale=ru_RU&access_token={tokenWow}");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {

                            CharacterSoulbindsAll allSoulbinds = JsonConvert.DeserializeObject<CharacterSoulbindsAll>(line);
                            if (allSoulbinds.soulbinds != null)
                            {
                                foreach (SoulbindSoulbinds soulbinds in allSoulbinds.soulbinds)
                                {

                                    if (soulbinds.is_active == true)
                                    {
                                        _charInfo.CovenSoul = soulbinds.soulbind.name;





                                    }

                                }
                            }

                        }
                    }
                }
                responce.Close();
                _charInfo.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _charInfo.Error = true;
                    string message = $"\nGetSoulbindsCharacter Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _charInfo.Error = true;
                string message = $"GetSoulbindsCharacter Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }


        }
        private void Character_raid_progress(string name, string realm)
        {

            try
            {
                WebRequest requestchar = WebRequest.Create($"https://raider.io/api/v1/characters/profile?region=eu&realm={realm}&name={name}&fields=mythic_plus_scores%2Craid_progression");

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {
                        string line = "";
                        while ((line = reader1.ReadLine()) != null)
                        {
                            RaiderIO_info character = JsonConvert.DeserializeObject<RaiderIO_info>(line);

                            _charInfo.RaidProgress = character.raid_progression.SepulcherOfTheFirstOnes.summary;
                            _charInfo.MythicPlus = character.mythic_plus_scores.all;
                        }
                    }
                }
                responcechar.Close();
                _charInfo.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _charInfo.Error = true;
                    string message = $"\nCharacter_raid_progress Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _charInfo.Error = true;
                string message = $"Character_raid_progress Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
        private void GetCharMedia(string name, string realm)
        {


            try
            {
                WebRequest request = WebRequest.Create($"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/character-media?namespace=profile-eu&locale=ru_RU&access_token={tokenWow}");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {


                            CharMedia charMedia = JsonConvert.DeserializeObject<CharMedia>(line);


                            foreach (AssetCM media in charMedia.assets)
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
                }
                responce.Close();
                _charInfo.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _charInfo.Error = true;
                    string message = $"\nGetCharMedia Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _charInfo.Error = true;
                string message = $"GetCharMedia Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }


        }

        private void GetCharSet(string name, string realm)
        {


            try
            {
                WebRequest request = WebRequest.Create($"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/equipment?namespace=profile-eu&locale=ru_RU&access_token={tokenWow}");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {


                            CharEquipAll charEquip = JsonConvert.DeserializeObject<CharEquipAll>(line);

                            if (charEquip.equipped_item_sets != null)
                            {
                                foreach (EquippedItemSet setequip in charEquip.equipped_item_sets)
                                {
                                    if (setequip.display_string.Contains("/5)"))
                                    {
                                        _charInfo.SetcountItem = "Set:**" + setequip.display_string.Replace(setequip.item_set.name, "") + "**" ;
                                    }
                                }
                            }
                            
                           


                           

                        }
                    }
                }
                responce.Close();
                _charInfo.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _charInfo.Error = true;
                    string message = $"\nGetCharMedia Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _charInfo.Error = true;
                string message = $"GetCharMedia Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }


        }
        private void GetCharStats(string name, string realm)
        {


            try
            {
                WebRequest request = WebRequest.Create($"https://eu.api.blizzard.com/profile/wow/character/{realm}/{name.ToLower()}/statistics?namespace=profile-eu&locale=ru_RU&access_token={tokenWow}");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {


                            CharStats charStats = JsonConvert.DeserializeObject<CharStats>(line);
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
                responce.Close();
                _charInfo.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _charInfo.Error = true;
                    string message = $"\nGetCharStats Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _charInfo.Error = true;
                string message = $"GetCharStats Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }


        }
    }
    public class CharInfoAll
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public string Lvl { get; set; }
        public string Guild { get; set; }
        public string ILvl { get; set; }
        public string Class { get; set; }
        public string Spec { get; set; }
        public string Coven { get; set; }
        public string CovenSoul { get; set; }
        public string RaidProgress { get; set; }
        public string MythicPlus { get; set; }
        public string LastLogin { get; set; }
        public bool Error { get; set; }
        public string ImageCharMain { get; set; }
        public string ImageCharMainRaw { get; set; }
        public string ImageCharAvatar { get; set; }
        public string ImageCharInset { get; set; }
        public string Stats { get; set; }
        public string LinkBnet { get; set; }
        public string SetcountItem { get; set; }

        public CharInfoAll()
        {
            Name = "";
            ILvl = "";
            Class = "нет";
            Spec = "нет";
            Coven = "нет";
            Stats = "";
            CovenSoul = "нет";
            LastLogin = "хз";
            Guild = "нет";
            RaidProgress = "0/0";
            MythicPlus = "0";
            ImageCharMain = "";
            ImageCharAvatar = "";
            ImageCharMainRaw = "";
            ImageCharInset = "";
            Error = false;
            LinkBnet = "";
            SetcountItem = "";
        }

    }
    #region Char Stats

    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Key
    {
        public string href { get; set; }
    }

    public class PowerType
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Speed
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
    }

    public class Strength
    {
        public int @base { get; set; }
        public int effective { get; set; }
    }

    public class Agility
    {
        public int @base { get; set; }
        public int effective { get; set; }
    }

    public class Intellect
    {
        public int @base { get; set; }
        public int effective { get; set; }
    }

    public class Stamina
    {
        public int @base { get; set; }
        public int effective { get; set; }
    }

    public class MeleeCrit
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class MeleeHaste
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Mastery
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Lifesteal
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Avoidance
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
    }

    public class SpellCrit
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Armor
    {
        public int @base { get; set; }
        public int effective { get; set; }
    }

    public class Dodge
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Parry
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Block
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class RangedCrit
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class RangedHaste
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class SpellHaste
    {
        public double rating { get; set; }
        public double rating_bonus { get; set; }
        public double value { get; set; }
    }

    public class Realm
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Character
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
    }

    public class CharStats
    {
        public Links _links { get; set; }
        public double health { get; set; }
        public double power { get; set; }
        public PowerType power_type { get; set; }
        public Speed speed { get; set; }
        public Strength strength { get; set; }
        public Agility agility { get; set; }
        public Intellect intellect { get; set; }
        public Stamina stamina { get; set; }
        public MeleeCrit melee_crit { get; set; }
        public MeleeHaste melee_haste { get; set; }
        public Mastery mastery { get; set; }
        public double bonus_armor { get; set; }
        public Lifesteal lifesteal { get; set; }
        public double versatility { get; set; }
        public double versatility_damage_done_bonus { get; set; }
        public double versatility_healing_done_bonus { get; set; }
        public double versatility_damage_taken_bonus { get; set; }
        public Avoidance avoidance { get; set; }
        public double attack_power { get; set; }
        public double main_hand_damage_min { get; set; }
        public double main_hand_damage_max { get; set; }
        public double main_hand_speed { get; set; }
        public double main_hand_dps { get; set; }
        public double off_hand_damage_min { get; set; }
        public double off_hand_damage_max { get; set; }
        public double off_hand_speed { get; set; }
        public double off_hand_dps { get; set; }
        public double spell_power { get; set; }
        public double spell_penetration { get; set; }
        public SpellCrit spell_crit { get; set; }
        public double mana_regen { get; set; }
        public double mana_regen_combat { get; set; }
        public Armor armor { get; set; }
        public Dodge dodge { get; set; }
        public Parry parry { get; set; }
        public Block block { get; set; }
        public RangedCrit ranged_crit { get; set; }
        public RangedHaste ranged_haste { get; set; }
        public SpellHaste spell_haste { get; set; }
        public Character character { get; set; }
    }


    #endregion
    #region Char Media
    public class SelfCM
    {
        public string href { get; set; }
    }

    public class LinksCM
    {
        public SelfCM self { get; set; }
    }

    public class KeyCM
    {
        public string href { get; set; }
    }

    public class RealmCM
    {
        public KeyCM key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class CharacterCM
    {
        public KeyCM key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public RealmCM realm { get; set; }
    }

    public class AssetCM
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class CharMedia
    {
        public LinksCM _links { get; set; }
        public CharacterCM character { get; set; }
        public List<AssetCM> assets { get; set; }
    }
    #endregion
    #region SoulBindClass
    public class SelfSoulbinds
    {
        public string href { get; set; }
    }

    public class LinksSoulbinds
    {
        public SelfSoulbinds self { get; set; }
    }

    public class KeySoulbinds
    {
        public string href { get; set; }
    }

    public class RealmSoulbinds
    {
        public KeySoulbinds key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class CharacterSoulbinds
    {
        public KeySoulbinds key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public RealmSoulbinds realm { get; set; }
    }

    public class ChosenCovenantSoulbinds
    {
        public KeySoulbinds key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbind2Soulbinds
    {
        public KeySoulbinds key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Trait2Soulbinds
    {
        public KeySoulbinds key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class TypeSoulbinds
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class ConduitSoulbinds
    {
        public KeySoulbinds key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class SocketSoulbinds
    {
        public ConduitSoulbinds conduit { get; set; }
        public int rank { get; set; }
    }

    public class ConduitSocketSoulbinds
    {
        public TypeSoulbinds type { get; set; }
        public SocketSoulbinds socket { get; set; }
    }

    public class TraitSoulbinds
    {
        public Trait2Soulbinds trait { get; set; }
        public int tier { get; set; }
        public int display_order { get; set; }
        public ConduitSocketSoulbinds conduit_socket { get; set; }
    }

    public class SoulbindSoulbinds
    {
        public Soulbind2Soulbinds soulbind { get; set; }
        public List<TraitSoulbinds> traits { get; set; }
        public bool? is_active { get; set; }
    }

    public class CharacterSoulbindsAll
    {
        public LinksSoulbinds _links { get; set; }
        public CharacterSoulbinds character { get; set; }
        public ChosenCovenantSoulbinds chosen_covenant { get; set; }
        public int renown_level { get; set; }
        public List<SoulbindSoulbinds> soulbinds { get; set; }
    }
    #endregion
    #region CharInfoClass
    public class Self_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Links_charackter_full_info
    {
        public Self_charackter_full_info self { get; set; }
    }

    public class Gender_charackter_full_info
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Faction_charackter_full_info
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Key_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Race_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class CharacterClass_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ActiveSpec_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Realm_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Guild_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_charackter_full_info realm { get; set; }
        public Faction_charackter_full_info faction { get; set; }
    }

    public class Achievements_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Titles_charackter_full_info
    {
        public string href { get; set; }
    }

    public class PvpSummary_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Encounters_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Media_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Specializations_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Statistics_charackter_full_info
    {
        public string href { get; set; }
    }

    public class MythicKeystoneProfile_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Equipment_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Appearance_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Collections_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Reputations_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Quests_charackter_full_info
    {
        public string href { get; set; }
    }

    public class AchievementsStatistics_charackter_full_info
    {
        public string href { get; set; }
    }

    public class Professions_charackter_full_info
    {
        public string href { get; set; }
    }

    public class ChosenCovenant_charackter_full_info
    {
        public Key_charackter_full_info key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Soulbinds_charackter_full_info
    {
        public string href { get; set; }
    }

    public class CovenantProgress_charackter_full_info
    {
        public ChosenCovenant_charackter_full_info chosen_covenant { get; set; }
        public int renown_level { get; set; }
        public Soulbinds_charackter_full_info soulbinds { get; set; }
    }

    public class Root_charackter_full_info
    {
        public Links_charackter_full_info _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Gender_charackter_full_info gender { get; set; }
        public Faction_charackter_full_info faction { get; set; }
        public Race_charackter_full_info race { get; set; }
        public CharacterClass_charackter_full_info character_class { get; set; }
        public ActiveSpec_charackter_full_info active_spec { get; set; }
        public Realm_charackter_full_info realm { get; set; }
        public Guild_charackter_full_info guild { get; set; }
        public int level { get; set; }
        public int experience { get; set; }
        public int achievement_points { get; set; }
        public Achievements_charackter_full_info achievements { get; set; }
        public Titles_charackter_full_info titles { get; set; }
        public PvpSummary_charackter_full_info pvp_summary { get; set; }
        public Encounters_charackter_full_info encounters { get; set; }
        public Media_charackter_full_info media { get; set; }
        public long last_login_timestamp { get; set; }
        public int average_item_level { get; set; }
        public int equipped_item_level { get; set; }
        public Specializations_charackter_full_info specializations { get; set; }
        public Statistics_charackter_full_info statistics { get; set; }
        public MythicKeystoneProfile_charackter_full_info mythic_keystone_profile { get; set; }
        public Equipment_charackter_full_info equipment { get; set; }
        public Appearance_charackter_full_info appearance { get; set; }
        public Collections_charackter_full_info collections { get; set; }
        public Reputations_charackter_full_info reputations { get; set; }
        public Quests_charackter_full_info quests { get; set; }
        public AchievementsStatistics_charackter_full_info achievements_statistics { get; set; }
        public Professions_charackter_full_info professions { get; set; }
        public CovenantProgress_charackter_full_info covenant_progress { get; set; }
    }
    #endregion
    #region RaiderIOClass
    public class MythicPlusScores
    {
        public string all { get; set; }
        public string dps { get; set; }
        public string healer { get; set; }
        public string tank { get; set; }
        public string spec_0 { get; set; }
        public string spec_1 { get; set; }
        public string spec_2 { get; set; }
        public string spec_3 { get; set; }
    }
    public class CastleNathria
    {
        public string summary { get; set; }
        public string total_bosses { get; set; }
        public string normal_bosses_killed { get; set; }
        public string heroic_bosses_killed { get; set; }
        public string mythic_bosses_killed { get; set; }
    }
    public class SanctumOfDomination
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class SepulcherOfTheFirstOnes
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }
    public class RaidProgression
    {
        [JsonProperty("castle-nathria")]
        public CastleNathria CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public SanctumOfDomination SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public SepulcherOfTheFirstOnes SepulcherOfTheFirstOnes { get; set; }
    }

    public class RaiderIO_info
    {
        public string name { get; set; }
        public string race { get; set; }
        public string @class { get; set; }
        public string active_spec_name { get; set; }
        public string active_spec_role { get; set; }
        public string gender { get; set; }
        public string faction { get; set; }
        public string achievement_points { get; set; }
        public string honorable_kills { get; set; }
        public string thumbnail_url { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public string profile_banner { get; set; }
        public MythicPlusScores mythic_plus_scores { get; set; }
        public RaidProgression raid_progression { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
    

    public class Item1
    {
        public Key key { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Slot
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Quality
    {
        public string type { get; set; }
        public string name { get; set; }
    }



    public class ItemClass
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ItemSubclass
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class InventoryType
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Binding
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Color
    {
        public string r { get; set; }
        public string g { get; set; }
        public string b { get; set; }
        public string a { get; set; }
    }

    public class Display
    {
        public string display_string { get; set; }
        public Color color { get; set; }
    }

    public class Armor1
    {
        public int value { get; set; }
        public Display display { get; set; }
    }

    public class Type
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Stat
    {
        public Type type { get; set; }
        public int value { get; set; }
        public Display display { get; set; }
        public bool? is_negated { get; set; }
        public bool? is_equip_bonus { get; set; }
    }

    public class DisplayStrings
    {
        public string header { get; set; }
        public string gold { get; set; }
        public string silver { get; set; }
        public string copper { get; set; }
    }

    public class SellPrice
    {
        public int value { get; set; }
        public DisplayStrings display_strings { get; set; }
    }

    public class Level
    {
        public int value { get; set; }
        public string display_string { get; set; }
    }

    public class Link
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class PlayableClasses
    {
        public List<Link> links { get; set; }
        public string display_string { get; set; }
    }

    public class Requirements
    {
        public Level level { get; set; }
        public PlayableClasses playable_classes { get; set; }
    }

    public class ItemSet
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Item21
    {
        public Item1 item { get; set; }
        public bool is_equipped { get; set; }
    }

    public class Effect
    {
        public string display_string { get; set; }
        public int required_count { get; set; }
        public bool is_active { get; set; }
    }

    public class Set
    {
        public ItemSet item_set { get; set; }
        public List<Item21> items { get; set; }
        public List<Effect> effects { get; set; }
        public string display_string { get; set; }
    }

    public class SecondItem
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Transmog
    {
        public Item item { get; set; }
        public string display_string { get; set; }
        public int item_modified_appearance_id { get; set; }
        public SecondItem second_item { get; set; }
        public int? second_item_modified_appearance_id { get; set; }
    }

    public class Durability
    {
        public int value { get; set; }
        public string display_string { get; set; }
    }

    public class NameDescription
    {
        public string display_string { get; set; }
        public Color color { get; set; }
    }

    public class SocketType
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Socket
    {
        public SocketType socket_type { get; set; }
        public Item item { get; set; }
        public string display_string { get; set; }
        public Media media { get; set; }
    }

    public class Spell2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class DisplayColor
    {
        public string r { get; set; }
        public string g { get; set; }
        public string b { get; set; }
        public string a { get; set; }
    }

    public class Spell1
    {
        public Spell2 spell { get; set; }
        public string description { get; set; }
        public DisplayColor display_color { get; set; }
    }

    public class SourceItem
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class EnchantmentSlot
    {
        public int id { get; set; }
        public string type { get; set; }
    }

    public class Enchantment
    {
        public string display_string { get; set; }
        public SourceItem source_item { get; set; }
        public int enchantment_id { get; set; }
        public EnchantmentSlot enchantment_slot { get; set; }
    }

    public class DamageClass
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Damage
    {
        public int min_value { get; set; }
        public int max_value { get; set; }
        public string display_string { get; set; }
        public DamageClass damage_class { get; set; }
    }

    public class AttackSpeed
    {
        public int value { get; set; }
        public string display_string { get; set; }
    }

    public class Dps
    {
        public double value { get; set; }
        public string display_string { get; set; }
    }

    public class Weapon
    {
        public Damage damage { get; set; }
        public AttackSpeed attack_speed { get; set; }
        public Dps dps { get; set; }
    }

    public class EquippedItem
    {
        public Item item { get; set; }
        public Slot slot { get; set; }
        public int quantity { get; set; }
        public int context { get; set; }
        public List<int> bonus_list { get; set; }
        public Quality quality { get; set; }
        public string name { get; set; }
        public int modified_appearance_id { get; set; }
        public Media media { get; set; }
        public ItemClass item_class { get; set; }
        public ItemSubclass item_subclass { get; set; }
        public InventoryType inventory_type { get; set; }
        public Binding binding { get; set; }
        public Armor1 armor { get; set; }
        public List<Stat> stats { get; set; }
        public SellPrice sell_price { get; set; }
        public Requirements requirements { get; set; }
        public Set set { get; set; }
        public Level level { get; set; }
        public Transmog transmog { get; set; }
        public Durability durability { get; set; }
        public NameDescription name_description { get; set; }
        public List<Socket> sockets { get; set; }
        public bool? is_subclass_hidden { get; set; }
        public List<Spell1> spells { get; set; }
        public List<Enchantment> enchantments { get; set; }
        public string limit_category { get; set; }
        public string description { get; set; }
        public string unique_equipped { get; set; }
        public Weapon weapon { get; set; }
    }

    public class EquippedItemSet
    {
        public ItemSet item_set { get; set; }
        public List<Item> items { get; set; }
        public List<Effect> effects { get; set; }
        public string display_string { get; set; }
    }

    public class CharEquipAll
    {
        public Links _links { get; set; }
        public Character character { get; set; }
        public List<EquippedItem> equipped_items { get; set; }
        public List<EquippedItemSet> equipped_item_sets { get; set; }
    }


    #endregion
    #region Char RAid

    public class Expansion2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Instance
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Difficulty
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Encounter
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Encounters
    {
        public Encounter encounter { get; set; }
        public int completed_count { get; set; }
        public object last_kill_timestamp { get; set; }
    }

    public class Progress
    {
        public int completed_count { get; set; }
        public int total_count { get; set; }
        public List<Encounters> encounters { get; set; }
    }

    public class Mode
    {
        public Difficulty difficulty { get; set; }
        public Status status { get; set; }
        public Progress progress { get; set; }
    }

    public class Instances
    {
        public Instance instance { get; set; }
        public List<Mode> modes { get; set; }
    }

    public class Expansion
    {
        public Expansion expansion { get; set; }
        public List<Instances> instances { get; set; }
    }

    public class CharachterRaid
    {
        public Links _links { get; set; }
        public Character character { get; set; }
        public List<Expansion> expansions { get; set; }
    }
    #endregion

}
