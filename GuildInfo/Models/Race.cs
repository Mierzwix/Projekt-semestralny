using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuildInfo.Models
{
    [Table("Races")]
    public class Race : BaseModel
    {
        [Key]
        public Guid RaceId { get; set; }
        public Race() { this.RaceId = Guid.NewGuid(); }
        public bool IsNameOK => !string.IsNullOrWhiteSpace(Name);

        public Race(string name, string description) : this()
        {
            this.Name = name;
            this.Description = description;
        }

        public Race(Race race)
        {
            if (race == null)
                return;

            this.RaceId = race.RaceId;
            this.Name = race.Name;
            this.Description = race.Description;
        }

        /// <summary>
        /// Aktualizacja informacji o rasie
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public bool UpdateProperties(Race race)
        {
            if (race == null)
                return false;

            this.RaceId = race.RaceId;
            this.Name = race.Name;
            this.Description = race.Description;

            return true;
        }

        /// <summary>
        /// Sprawdza czy istnieje już taka rasa
        /// </summary>
        /// <returns></returns>
        public bool IsRaceExist()
        {
            using (GIContext cont = new GIContext())
            {
                return cont.Races.Any(x => x.RaceId == this.RaceId && x.Name == Name);
            }
        }

        /// <summary>
        /// Dodanie nowej rasy
        /// </summary>
        public bool Add()
        {
            if (IsRaceExist())
            {
                MessageBox.Show($"Już posiadasz rasę z taką nazwą", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (!IsNameOK)
            {
                MessageBox.Show("Nazwa nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    cont.Races.Add(this);
                    cont.SaveChanges();
                    MessageBox.Show($"Rasa dodana pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas dodawania rasy:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizacja wybranej rasy
        /// </summary>
        public bool Update()
        {
            if (!IsNameOK)
            {
                MessageBox.Show("Nazwa nie może być pusta", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            try
            {
                using (GIContext cont = new GIContext())
                {
                    Race raceFromDB = cont.Races.FirstOrDefault(x => x.RaceId == RaceId);
                    bool updateOK = false;
                    if (raceFromDB == null)
                        cont.Races.Add(this);
                    else
                        updateOK = raceFromDB.UpdateProperties(this);

                    if (updateOK)
                    {
                        cont.SaveChanges();
                        MessageBox.Show($"Rasa zaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Rasa niezaktualizowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas aktualizowania rasy:\n{ex.Message}");
            }
        }

        /// <summary>
        /// Usuwanie wybranej rasy
        /// </summary>
        public bool Remove()
        {
            if (!IsRaceExist())
            {
                MessageBox.Show($"Brak rasy z taką nazwą", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            var res = MessageBox.Show($"Czy na pewno usunąć wybraną rasę?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
                return false;

            try
            {
                using (GIContext cont = new GIContext())
                {
                    var members = cont.Members.Where(x => x.Race.RaceId == this.RaceId).ToList();
                    members.ForEach(x => x.Race = null);
                    Race race = cont.Races.FirstOrDefault(x => x.RaceId == RaceId);
                    if (race == null)
                        return false;

                    cont.Races.Remove(race);
                    cont.SaveChanges();
                    MessageBox.Show($"Rasa usunięta pomyślnie", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem podczas usuwania rasy:\n{ex.Message}");
            }
        }
    }
}