using GuildInfo.Models;
using GuildInfo.ViewModels;
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
    public class MembersWindowViewModel: BaseViewModel
    {
        /// <summary>
        /// True jeśli nie dokonano dodawania / aktualizacji / kasowania, czyli po prostu zamknięcie okna bez działania
        /// </summary>
        private bool _isJustClosedWindow = true;
        public ObservableCollection<Member> Members { get; set; }
        public ObservableCollection<Guild> Guilds { get; set; }
        public ObservableCollection<Race> Races { get; set; }
        public ObservableCollection<Profession> Professions { get; set; }

        private Member _selectedMember;
        public Member SelectedMember
        {
            get { return _selectedMember; }
            set
            {
                if (_selectedMember != value)
                {
                    _selectedMember = value;
                    TempMember = new Member(value);
                    if(value != null)
                    {
                        if (value.Profession != null)
                            TempMember.Profession = Professions.FirstOrDefault(x => x.ProfessionId == value.Profession.ProfessionId);
                        if(value.Race != null)
                            TempMember.Race = Races.FirstOrDefault(x => x.RaceId == value.Race.RaceId);
                        if(value.Guild != null)
                            TempMember.Guild = Guilds.FirstOrDefault(x => x.GuildId == value.Guild.GuildId);
                    }
                }
            }
        }

        public Member TempMember { get; set; }

        public ICommand LoadedCmd { get; set; }
        public ICommand NewMemberCmd { get; set; }
        public ICommand AddMemberCmd { get; set; }
        public ICommand UpdateMemberCmd { get; set; }
        public ICommand RemoveMemberCmd { get; set; }
        public ICommand CloseCmd { get; set; }

        public MembersWindowViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LoadedCmd = new RelayCommand(ExecLoaded);
            NewMemberCmd = new RelayCommand(ExecNewMember);
            AddMemberCmd = new RelayCommand(ExecAddMember, x => TempMember != null && TempMember.IsNicknameOK);
            UpdateMemberCmd = new RelayCommand(ExecUpdateMember, x => TempMember != null && TempMember.IsNicknameOK);
            RemoveMemberCmd = new RelayCommand(ExecRemoveMember, x => SelectedMember != null);
            CloseCmd = new RelayCommand(ExecClose);
        }

        private void Initialize()
        {
            try
            {
                using (GIContext cont = new GIContext())
                {
                    Members = new ObservableCollection<Member>(cont.Members.ToList().OrderBy(x => x.Name));
                    Guilds = new ObservableCollection<Guild>(cont.Guilds.ToList().OrderBy(x => x.Name));
                    Races = new ObservableCollection<Race>(cont.Races.ToList().OrderBy(x => x.Name));
                    Professions = new ObservableCollection<Profession>(cont.Professions.ToList().OrderBy(x => x.Name));
                    NewMember();
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
        private void NewMember()
        {
            SelectedMember = null;
            TempMember = new Member();
        }

        /// <summary>
        /// Dodanie nowej gildii do bazy danych
        /// </summary>
        private void AddMember()
        {
            if (TempMember != null)
            {
                if (TempMember.Add())
                {
                    Members.Add(TempMember);
                    NewMember();
                    _isJustClosedWindow = false;
                }
            }
        }

        /// <summary>
        /// Aktualizacja wybranej gildii
        /// </summary>
        private void UpdateMember()
        {
            if (SelectedMember != null)
            {
                TempMember.Update();
                SelectedMember.UpdateProperties(TempMember);
                _isJustClosedWindow = false;
            }
        }

        /// <summary>
        /// Usunięcie wybranej gildii z bazy danych
        /// </summary>
        private void RemoveMember()        
        {
            if (SelectedMember != null)
            {
                if (SelectedMember.Remove())
                {
                    Members.Remove(Members.FirstOrDefault(x => x.MemberId == SelectedMember.MemberId));
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
            if (obj is MembersWindow gw)
            {
                gw.DialogResult = !_isJustClosedWindow;
            }
        }


        private void ExecLoaded(object obj)
        {
            Initialize();
        }

        private void ExecNewMember(object obj)
        {
            NewMember();
        }

        private void ExecAddMember(object obj)
        {
            AddMember();
        }

        private void ExecUpdateMember(object obj)
        {
            UpdateMember();
        }

        private void ExecRemoveMember(object obj)
        {
            RemoveMember();
        }

        private void ExecClose(object obj)
        {
            Close(obj);
        }
    }
}
