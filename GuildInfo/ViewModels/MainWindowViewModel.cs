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
using System.Data.Entity;

namespace GuildInfo.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        public ObservableCollection<Guild> Guilds { get; set; }
        public ObservableCollection<Profession> Professions { get; set; }
        public ObservableCollection<Race> Races { get; set; }

        public Guild SelectedGuild { get; set; }
        
        public ICommand LoadedCmd { get; set; }
        public ICommand ShowGuildsCmd { get; set; }
        public ICommand ShowMembersCmd { get; set; }
        public ICommand ShowProfessionsCmd { get; set; }
        public ICommand ShowRacesCmd { get; set; }
        public ICommand AddMemberIntoGuildCmd { get; set; }
        public ICommand DeleteMemberFromGuildCmd { get; set; }

        public MainWindowViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LoadedCmd = new RelayCommand(ExecLoaded);
            ShowGuildsCmd = new RelayCommand(ExecShowGuilds);
            ShowMembersCmd = new RelayCommand(ExecShowMembers);
            ShowProfessionsCmd = new RelayCommand(ExecShowProfessions);
            ShowRacesCmd = new RelayCommand(ExecShowRaces);
            AddMemberIntoGuildCmd = new RelayCommand(ExecAddMemberIntoGuild, x => SelectedGuild != null);
            DeleteMemberFromGuildCmd = new RelayCommand(ExecDeleteMemberFromGuild, x => SelectedGuild != null && SelectedGuild.SelectedMember != null);
        }

        private void ExecAddMemberIntoGuild(object obj)
        {
            ChooseMembersWindow w = new ChooseMembersWindow();
            var vm = w.DataContext as ChooseMembersWindowViewModel;
            using (GIContext cont = new GIContext())
            {
                foreach (var member in cont.Members.Where(x => x.Guild == null).ToList())
                    vm.Members.Add(new CheckedMember(member));

                w.DataContext = vm;
                if (w.ShowDialog().Value)
                {
                    var guild = cont.Guilds.FirstOrDefault(x => x.GuildId == SelectedGuild.GuildId);
                    foreach (CheckedMember cm in vm.CheckedMembers)
                    {
                        var memberFromDB = cont.Members.FirstOrDefault(x => x.MemberId == cm.MemberId);
                        if (memberFromDB != null)
                        {
                            memberFromDB.Guild = guild;
                            cont.Entry(memberFromDB).State = EntityState.Modified;
                        }
                    }

                    cont.SaveChanges();
                    Initialize();
                }
            }
        }

        private void ExecDeleteMemberFromGuild(object obj)
        {
            if(obj is Guild g && g.SelectedMember != null)
            {
                var res = MessageBox.Show("Czy na pewno usunąć wybranego członka?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.No)
                    return;

                using (GIContext cont = new GIContext())
                {
                    var memberFromDB = cont.Members.Include(x => x.Guild).FirstOrDefault(x => x.MemberId == g.SelectedMember.MemberId);
                    if(memberFromDB!= null)
                    {
                        memberFromDB.Guild = null;
                        cont.Entry(memberFromDB).State = EntityState.Modified;
                        g.Members.Remove(g.SelectedMember);
                    }
                    cont.SaveChanges();
                }
            }
        }

        private void Initialize()
        {
            try
            {
                using (GIContext cont = new GIContext())
                {
                    Guilds = new ObservableCollection<Guild>(cont.Guilds.Include(x => x.Members).ToList().OrderBy(x => x.Name));
                    Professions = new ObservableCollection<Profession>(cont.Professions.ToList().OrderBy(x => x.Name));
                    Races = new ObservableCollection<Race>(cont.Races.ToList().OrderBy(x => x.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem podczas inicjalizacji:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecLoaded(object obj)
        {
            Initialize();
        }

        private void ExecShowGuilds(object obj)
        {
            GuildsWindow w = new GuildsWindow();
            if (w.ShowDialog().Value)
                Initialize();
        }

        private void ExecShowMembers(object obj)
        {
            MembersWindow w = new MembersWindow();
            if (w.ShowDialog().Value)
                Initialize();
        }

        private void ExecShowProfessions(object obj)
        {
            ProfessionsWindow w = new ProfessionsWindow();
            if (w.ShowDialog().Value)
                Initialize();
        }

        private void ExecShowRaces(object obj)
        {
            RacesWindow w = new RacesWindow();
            if (w.ShowDialog().Value)
                Initialize();
        }
    }
}
