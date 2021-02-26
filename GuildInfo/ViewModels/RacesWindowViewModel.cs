using GuildInfo.Models;
using GuildInfo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuildInfo.ViewModels
{
    public class RacesWindowViewModel: BaseViewModel
    {
        /// <summary>
        /// True jeśli nie dokonano dodawania / aktualizacji / kasowania, czyli po prostu zamknięcie okna bez działania
        /// </summary>
        private bool _isJustClosedWindow = true;
        public ObservableCollection<Race> Races { get; set; }

        private Race _selectedRace;
        public Race SelectedRace
        {
            get { return _selectedRace; }
            set
            {
                if (_selectedRace != value)
                {
                    _selectedRace = value;
                    TempRace = new Race(value);
                }
            }
        }

        public Race TempRace { get; set; }

        public ICommand LoadedCmd { get; set; }
        public ICommand NewRaceCmd { get; set; }
        public ICommand AddRaceCmd { get; set; }
        public ICommand UpdateRaceCmd { get; set; }
        public ICommand RemoveRaceCmd { get; set; }
        public ICommand CloseCmd { get; set; }

        public RacesWindowViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LoadedCmd = new RelayCommand(ExecLoaded);
            NewRaceCmd = new RelayCommand(ExecNewRace);
            AddRaceCmd = new RelayCommand(ExecAddRace, x => TempRace != null && TempRace.IsNameOK);
            UpdateRaceCmd = new RelayCommand(ExecUpdateRace, x => TempRace != null && TempRace.IsNameOK);
            RemoveRaceCmd = new RelayCommand(ExecRemoveRace, x => SelectedRace != null);
            CloseCmd = new RelayCommand(ExecClose);
        }

        private void Initialize()
        {
            try
            {
                using (GIContext cont = new GIContext())
                {
                    Races = new ObservableCollection<Race>(cont.Races.ToList().OrderBy(x => x.Name));
                    NewRace();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem podczas inicjalizacji:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Tworzenie nowej rasy
        /// </summary>
        private void NewRace()
        {
            TempRace = new Race();
        }

        /// <summary>
        /// Dodanie nowej rasy do bazy danych
        /// </summary>
        private void AddRace()
        {
            if (TempRace != null)
            {
                if (TempRace.Add())
                {
                    Races.Add(TempRace);
                    NewRace();
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Aktualizacja wybranej rasy
        /// </summary>
        private void UpdateRace()
        {
            if (SelectedRace != null)
            {
                if (TempRace.Update())
                {
                    SelectedRace.UpdateProperties(TempRace);
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Usunięcie wybranej rasy z bazy danych
        /// </summary>
        private void RemoveRace()
        {
            if (SelectedRace != null)
            {
                if (SelectedRace.Remove())
                {
                    Races.Remove(Races.FirstOrDefault(x => x.RaceId == SelectedRace.RaceId));
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Zamknięcie okna z rasami
        /// </summary>
        /// <param name="obj"></param>
        private void Close(object obj)
        {
            if (obj is RacesWindow gw)
            {
                gw.DialogResult = !_isJustClosedWindow;
            }
        }


        private void ExecLoaded(object obj)
        {
            Initialize();
        }

        private void ExecNewRace(object obj)
        {
            NewRace();
        }

        private void ExecAddRace(object obj)
        {
            AddRace();
        }

        private void ExecUpdateRace(object obj)
        {
            UpdateRace();
        }

        private void ExecRemoveRace(object obj)
        {
            RemoveRace();
        }

        private void ExecClose(object obj)
        {
            Close(obj);
        }
    }
}
