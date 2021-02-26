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
    public class GuildsWindowViewModel: BaseViewModel
    {
        /// <summary>
        /// True jeśli nie dokonano dodawania / aktualizacji / kasowania, czyli po prostu zamknięcie okna bez działania
        /// </summary>
        private bool _isJustClosedWindow = true;
        public ObservableCollection<Guild> Guilds { get; set; }

        private Guild _selectedGuild;
        public Guild SelectedGuild
        {
            get { return _selectedGuild; }
            set
            {
                if (_selectedGuild != value)
                {
                    _selectedGuild = value;
                    TempGuild = new Guild(value);
                }
            }
        }

        public Guild TempGuild { get; set; }

        public ICommand LoadedCmd { get; set; }
        public ICommand NewGuildCmd { get; set; }
        public ICommand AddGuildCmd { get; set; }
        public ICommand UpdateGuildCmd { get; set; }
        public ICommand RemoveGuildCmd { get; set; }
        public ICommand CloseCmd { get; set; }

        public GuildsWindowViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LoadedCmd = new RelayCommand(ExecLoaded);
            NewGuildCmd = new RelayCommand(ExecNewGuild);
            AddGuildCmd = new RelayCommand(ExecAddGuild, x => TempGuild != null && TempGuild.IsNameOK);
            UpdateGuildCmd = new RelayCommand(ExecUpdateGuild, x => TempGuild != null && TempGuild.IsNameOK);
            RemoveGuildCmd = new RelayCommand(ExecRemoveGuild, x => SelectedGuild != null);
            CloseCmd = new RelayCommand(ExecClose);
        }

        private void Initialize()
        {
            try
            {
                using (GIContext cont = new GIContext())
                {
                    Guilds = new ObservableCollection<Guild>(cont.Guilds.ToList().OrderBy(x => x.Name));
                    NewGuild();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem podczas inicjalizacji:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Tworzenie nowej gildii
        /// </summary>
        private void NewGuild()
        {
            TempGuild = new Guild();
        }

        /// <summary>
        /// Dodanie nowej gildii do bazy danych
        /// </summary>
        private void AddGuild()
        {
            if (TempGuild != null)
            {
                if (TempGuild.Add())                
                {
                    Guilds.Add(TempGuild);
                    NewGuild();
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Aktualizacja wybranej gildii
        /// </summary>
        private void UpdateGuild()
        {
            if (SelectedGuild != null)
            {
                SelectedGuild.Update(TempGuild);
                _isJustClosedWindow = false;
            }
        }

        /// <summary>
        /// Usunięcie wybranej gildii z bazy danych
        /// </summary>
        private void RemoveGuild()
        {
            if (SelectedGuild != null)
            {
                if (SelectedGuild.RemoveGuild())
                {
                    Guilds.Remove(Guilds.FirstOrDefault(x => x.GuildId == SelectedGuild.GuildId));
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Zamknięcie okna z gildiami
        /// </summary>
        /// <param name="obj"></param>
        private void Close(object obj)
        {
            if(obj is GuildsWindow gw)
            {
                gw.DialogResult = !_isJustClosedWindow;
            }
        }


        private void ExecLoaded(object obj)
        {
            Initialize();
        }

        private void ExecNewGuild(object obj)
        {
            NewGuild();
        }

        private void ExecAddGuild(object obj)
        {
            AddGuild();
        }

        private void ExecUpdateGuild(object obj)
        {
            UpdateGuild();
        }

        private void ExecRemoveGuild(object obj)
        {
            RemoveGuild();
        }

        private void ExecClose(object obj)
        {
            Close(obj);
        }
    }
}
