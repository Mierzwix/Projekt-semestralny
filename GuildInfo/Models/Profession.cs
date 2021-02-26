using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuildInfo.Models
{
    [Table("Professions")]
    public class Profession : BaseModel
    {
        [Key]
        public Guid ProfessionId { get; set; }
        public bool IsNameOK => !string.IsNullOrWhiteSpace(Name);

        public Profession() { ProfessionId = Guid.NewGuid(); }
        public Profession(string name, string description) : this()
        {
            this.Name = name;
            this.Description = description;
        }

        public Profession(Profession profession)
        {
            if (profession == null)
                return;

            this.ProfessionId = profession.ProfessionId;
            this.Name = profession.Name;
            this.Description = profession.Description;
        }

        /// <summary>
        /// Sprawdza czy taka profesja już istnieje
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsProfessionExist(string name)
        {
            using (GIContext cont = new GIContext())
            {
                return cont.Professions.Any(x => x.ProfessionId == this.ProfessionId && x.Name == name);
            }
        }

        /// <summary>
        /// Dodanie nowej profesji
        /// </summary>
        public bool Add()
        {
            if (!IsNameOK)
            {
                MessageBox.Show("Nazwa nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (IsProfessionExist(Name))
            {
                MessageBox.Show($"Już posiadasz profesję z taką nazwą", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    cont.Professions.Add(this);
                    cont.SaveChanges();
                    MessageBox.Show($"Profesja dodana pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas dodawania profesji:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizacja wybranej profesji
        /// </summary>
        public bool Update()
        {
            try
            {
                if (!IsNameOK)
                {
                    MessageBox.Show("Nazwa nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }

                using (GIContext cont = new GIContext())
                {
                    Profession professionFromDB = cont.Professions.FirstOrDefault(x => x.ProfessionId == ProfessionId);
                    bool updateOK = false;
                    if (professionFromDB == null)
                        cont.Professions.Add(this);
                    else
                        updateOK = professionFromDB.UpdateProperties(this);

                    if (updateOK)
                    {
                        cont.SaveChanges();
                        MessageBox.Show($"Profesja zaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show($"Profesja niezaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas aktualizowania profesji:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizacja właściwości profesji
        /// </summary>
        /// <param name="profession"></param>
        /// <returns></returns>
        public bool UpdateProperties(Profession profession)
        {
            if (profession == null)
                return false;

            this.ProfessionId = profession.ProfessionId;
            this.Name = profession.Name;
            this.Description = profession.Description;
            return true;
        }

        /// <summary>
        /// Usuwanie wybranej profesji
        /// </summary>
        public bool Remove()
        {
            if (!IsProfessionExist(Name))
            {
                MessageBox.Show($"Brak profesji z taką nazwą", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var res = MessageBox.Show($"Czy na pewno usunąć wybraną profesję?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return false;

            try
            {
                using (GIContext cont = new GIContext())
                {
                    var members = cont.Members.Where(x => x.Profession.ProfessionId == this.ProfessionId).ToList();
                    members.ForEach(x => x.Profession = null);

                    Profession prof = cont.Professions.FirstOrDefault(x => x.ProfessionId == ProfessionId);
                    if (prof == null)
                        return false;

                    cont.Professions.Remove(prof);
                    cont.SaveChanges();
                    MessageBox.Show($"Profesja usunięta pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas usuwania profesji:\n{ex.Message}");
            }
        }

    }
}