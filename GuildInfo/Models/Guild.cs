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
    [Table("Guilds")]
    public class Guild : BaseModel
    {
        [Key]
        public Guid GuildId { get; set; }
        public ObservableCollection<Member> Members { get; set; }
        [NotMapped]
        public Member SelectedMember { get; set; }

        public Guild()
        {
            this.GuildId = Guid.NewGuid();
            this.Members = new ObservableCollection<Member>();
        }

        public Guild(string name, ObservableCollection<Member> members) : this()
        {
            this.Name = name;
            this.Members = members;
        }

        public Guild(Guild guild)
        {
            if (guild == null)
                return;

            this.GuildId = guild.GuildId;
            this.Name = guild.Name;
            this.Description = guild.Description;
            this.Members = guild.Members;
            this.SelectedMember = guild.SelectedMember;
        }

        /// <summary>
        /// Sprawdza czy w gildii z taką nazwą już istnieje
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsGuildExist()
        {
            using (GIContext cont = new GIContext())
            {
                return cont.Guilds.Any(x => x.Name == this.Name);
            }
        }

        /// <summary>
        /// Sprawdza czy nazwa nie jest pusta (True - jest ok)
        /// </summary>
        /// <returns></returns>
        public bool IsNameOK => !string.IsNullOrWhiteSpace(Name);

        /// <summary>
        /// Dodanie nowej gildii
        /// </summary>
        public bool Add()
        {
            if(IsGuildExist())
            {
                MessageBox.Show($"Już posiadasz gildię z taką nazwą", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if(!IsNameOK)
            {
                MessageBox.Show($"Nazwa nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    cont.Guilds.Add(this);
                    cont.SaveChanges();
                    MessageBox.Show($"Członek gildii dodany pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas dodawania członka gildii:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizacja gildii
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        public bool Update(Guild guild)
        {
            if (guild == null)
                return false;

            if (!IsNameOK)
            {
                MessageBox.Show($"Nazwa nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    var guildFromDB = cont.Guilds.ToList().FirstOrDefault(x => x.GuildId == guild.GuildId);
                    bool updateOK = false;
                    if (guildFromDB == null)
                        cont.Guilds.Add(guild);
                    else
                        updateOK = guildFromDB.UpdateProperties(guild);

                    if (updateOK)
                    {
                        cont.SaveChanges();
                        MessageBox.Show($"Członek gildii zaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Członek gildii niezaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas aktualizowania członka gildii:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizowanie informacji o gildii
        /// </summary>
        /// <param name="guild"></param>
        public bool UpdateProperties(Guild guild)
        {
            if (guild == null)
                return false;

            this.GuildId = guild.GuildId;
            this.Name = guild.Name;
            this.Description = guild.Description;
            this.Members = guild.Members;
            this.SelectedMember = guild.SelectedMember;

            return true;
        }

        /// <summary>
        /// Usuwanie gildii
        /// </summary>
        /// <returns></returns>
        public bool RemoveGuild()
        {
            if (!IsGuildExist())
            {
                MessageBox.Show($"Brak gildii z taką nazwą", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var res = MessageBox.Show($"Czy na pewno usunąć wybraną gildię?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return false;

            try
            {
                using (GIContext cont = new GIContext())
                {
                    var guild = cont.Guilds.Include(x => x.Members).FirstOrDefault(x => x.Name == this.Name);
                    if (guild != null)
                    {
                        cont.Guilds.Remove(guild);
                        cont.SaveChanges();
                        MessageBox.Show($"Gildia usunięta pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas usuwania gildii:\n{ex.Message}");
            }
        }
    }
}