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
    public class ProfessionsWindowViewModel: BaseViewModel    
    {
        /// <summary>
        /// True jeśli nie dokonano dodawania / aktualizacji / kasowania, czyli po prostu zamknięcie okna bez działania
        /// </summary>
        private bool _isJustClosedWindow = true;
        public ObservableCollection<Profession> Professions { get; set; }

        private Profession _selectedProfession;
        public Profession SelectedProfession
        {
            get { return _selectedProfession; }
            set
            {
                if (_selectedProfession != value)
                {
                    _selectedProfession = value;
                    TempProfession = new Profession(value);
                }
            }
        }

        public Profession TempProfession { get; set; }

        public ICommand LoadedCmd { get; set; }
        public ICommand NewProfessionCmd { get; set; }
        public ICommand AddProfessionCmd { get; set; }
        public ICommand UpdateProfessionCmd { get; set; }
        public ICommand RemoveProfessionCmd { get; set; }
        public ICommand CloseCmd { get; set; }

        public ProfessionsWindowViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LoadedCmd = new RelayCommand(ExecLoaded);
            NewProfessionCmd = new RelayCommand(ExecNewProfession);
            AddProfessionCmd = new RelayCommand(ExecAddProfession, x => TempProfession != null && TempProfession.IsNameOK);
            UpdateProfessionCmd = new RelayCommand(ExecUpdateProfession, x => TempProfession != null && TempProfession.IsNameOK);
            RemoveProfessionCmd = new RelayCommand(ExecRemoveProfession, x => SelectedProfession != null);
            CloseCmd = new RelayCommand(ExecClose);
        }

        private void Initialize()
        {
            try
            {
                using (GIContext cont = new GIContext())
                {
                    Professions = new ObservableCollection<Profession>(cont.Professions.ToList().OrderBy(x => x.Name));
                    NewProfession();
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
        private void NewProfession()
        {
            TempProfession = new Profession();
        }

        /// <summary>
        /// Dodanie nowej rasy do bazy danych
        /// </summary>
        private void AddProfession()
        {
            if (TempProfession != null)
            {
                if (TempProfession.Add())
                {
                    Professions.Add(TempProfession);
                    NewProfession();
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Aktualizacja wybranej rasy
        /// </summary>
        private void UpdateProfession()
        {
            if (SelectedProfession != null)
            {
                if (TempProfession.Update())
                {
                    SelectedProfession.UpdateProperties(TempProfession);
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Usunięcie wybranej rasy z bazy danych
        /// </summary>
        private void RemoveProfession()
        {
            if (SelectedProfession != null)
            {
                if (SelectedProfession.Remove())
                {
                    Professions.Remove(Professions.FirstOrDefault(x => x.ProfessionId == SelectedProfession.ProfessionId));
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
            if (obj is ProfessionsWindow gw)
            {
                gw.DialogResult = !_isJustClosedWindow;
            }
        }


        private void ExecLoaded(object obj)
        {
            Initialize();
        }

        private void ExecNewProfession(object obj)
        {
            NewProfession();
        }

        private void ExecAddProfession(object obj)
        {
            AddProfession();
        }

        private void ExecUpdateProfession(object obj)
        {
            UpdateProfession();
        }

        private void ExecRemoveProfession(object obj)
        {
            RemoveProfession();
        }

        private void ExecClose(object obj)
        {
            Close(obj);
        }
    }
}
