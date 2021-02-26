using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;

namespace GuildInfo.Models
{
    [Table("Members")]
    public class Member : BaseModel
    {
        [Key]
        public Guid MemberId { get; set; }
        public Guild Guild { get; set; }
        public int Level { get; set; }
        public string Nickname { get; set; }
        public Profession Profession { get; set; }
        public Race Race { get; set; }

        public bool IsNicknameOK => !string.IsNullOrWhiteSpace(Nickname);

        public Member() { this.MemberId = Guid.NewGuid(); this.Level = 1; }

        public Member(string name, string nickname, int level, Profession profession) : this()
        {
            this.Name = name;
            this.Nickname = nickname;
            this.Level = level;
            //this.ProfessionId = profession?.ProfessionId;
            this.Profession = profession;
        }

        public Member(Guild guild, string name, string nickname, int level, Profession profession) : this(name, nickname, level, profession)
        {
            this.Guild = guild;
        }

        public Member(Guild guild, string name, string nickname, int level, Profession profession, Race race) : this(guild, name, nickname, level, profession)
        {
            this.Race = race;
        }

        public Member(Member member)
        {
            if (member == null)
                return;

            this.MemberId = member.MemberId;
            this.Name = member.Name;
            this.Nickname = member.Nickname;
            this.Level = member.Level;
            this.Profession = member.Profession;
            this.Race = member.Race;
            this.Guild = member.Guild;
        }

        /// <summary>
        /// Aktualizacja informacji obecnego członka gildii
        /// </summary>
        /// <param name="member"></param>
        public bool UpdateProperties(Member member)
        {
            if (member == null)
                return false;

            this.MemberId = member.MemberId;
            this.Name = member.Name;
            this.Nickname = member.Nickname;
            this.Level = member.Level;
            this.Profession = member.Profession;
            this.Race = member.Race;
            this.Guild = member.Guild;

            return true;
        }

        /// <summary>
        /// Sprawdza czy istnieje już taki członek
        /// </summary>
        /// <param name="checkID"></param>
        /// <returns></returns>
        public bool IsMemberExist(bool checkID = false)
        {
            using (GIContext cont = new GIContext())
            {
                return cont.Members.Any(x => checkID ? x.MemberId != this.MemberId && x.Nickname == Nickname : x.Nickname == Nickname);
            }
        }

        /// <summary>
        /// Dodanie nowego członka
        /// </summary>
        public bool Add()
        {
            if (IsMemberExist(true))
            {
                MessageBox.Show($"Już posiadasz członka z takim pseudonimem", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (!IsNicknameOK)
            {
                MessageBox.Show("Ksywka nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Race == null)
            {
                MessageBox.Show("Nie wybrano rasy", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Profession == null)
            {
                MessageBox.Show("Nie wybrano profesji", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Level < 0)
            {
                MessageBox.Show("Poziom nie może być <= 0", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    if (Profession != null)
                        Profession = cont.Professions.FirstOrDefault(x => x.ProfessionId == Profession.ProfessionId);
                    if (Race != null)
                        Race = cont.Races.FirstOrDefault(x => x.RaceId == Race.RaceId);
                    if (Guild != null)
                    {
                        Guild = cont.Guilds.FirstOrDefault(x => x.GuildId == Guild.GuildId);
                        Guild.Members.Add(this);
                    }

                    cont.Members.Add(this);
                    cont.SaveChanges();
                    MessageBox.Show($"Członek dodany pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas dodawania członka:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizacja wybranego członka
        /// </summary>
        public bool Update()
        {
            if (!IsNicknameOK)
            {
                MessageBox.Show("Ksywka nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Race == null)
            {
                MessageBox.Show("Nie wybrano rasy", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Profession == null)
            {
                MessageBox.Show("Nie wybrano profesji", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Level < 0)
            {
                MessageBox.Show("Poziom nie może być <= 0", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    Member memberFromDB = cont.Members.Include(x => x.Guild).FirstOrDefault(x => x.MemberId == MemberId);
                    bool updateOK = false;
                    if (memberFromDB == null)
                        cont.Members.Add(this);
                    else
                    {
                        updateOK = memberFromDB.UpdateProperties(this);
                        if (updateOK)
                        {
                            if (Profession != null)
                                memberFromDB.Profession = cont.Professions.FirstOrDefault(x => x.ProfessionId == Profession.ProfessionId);
                            if (Race != null)
                                memberFromDB.Race = cont.Races.FirstOrDefault(x => x.RaceId == Race.RaceId);
                            if (Guild != null)
                                memberFromDB.Guild = cont.Guilds.FirstOrDefault(x => x.GuildId == Guild.GuildId);
                        }
                    }
                    if (updateOK)
                    {
                        cont.Entry(memberFromDB).State = EntityState.Modified;
                        cont.SaveChanges();
                        MessageBox.Show($"Członek zaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Członek niezaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas aktualizowania członka:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Usuwanie wybranego członka
        /// </summary>
        public bool Remove()
        {
            if (!IsMemberExist())
            {
                MessageBox.Show($"Brak członka z takim pseudonimem", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var res = MessageBox.Show($"Czy na pewno usunąć wybranego członka?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return false;

            try
            {
                using (GIContext cont = new GIContext())
                {
                    Member member = cont.Members.FirstOrDefault(x => x.MemberId == MemberId);
                    if (member == null)
                        return false;

                    cont.Members.Remove(member);
                    cont.SaveChanges();
                    MessageBox.Show($"Członek usunięty pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas usuwania członka:\n{ex.Message}");
            }
        }
    }
}