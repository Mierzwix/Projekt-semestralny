using GuildInfo.Models;
using GuildInfo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GuildInfo.ViewModels
{
    /// <summary>
    /// ViewModel dla wyboru czlonkow gildii
    /// </summary>
    public class ChooseMembersWindowViewModel: BaseViewModel
    {
        public ObservableCollection<CheckedMember> Members { get; private set; }
        public List<CheckedMember> CheckedMembers => Members?.Where(x => x.IsChecked).ToList();
        private bool _isJustClosing;
        public ICommand AddCmd { get; set; }
        public ICommand CloseCmd { get; set; }

        public ChooseMembersWindowViewModel() 
        {
            this.Members = new ObservableCollection<CheckedMember>();
            InitializeCommands();
        }

        /// <summary>
        /// Inicjalizacja komend
        /// </summary>
        private void InitializeCommands()
        {
            AddCmd = new RelayCommand(ExecAdd, x => Members != null && Members.Count(y => y.IsChecked) > 0);
            CloseCmd = new RelayCommand(ExecClose);
        }

        private void ExecAdd(object obj)
        {
            if (obj is ChooseMembersWindow w)
            {
                w.DialogResult = true;
            }
        }

        private void ExecClose(object obj)
        {
            if(obj is ChooseMembersWindow w)
            {
                w.DialogResult = false;
            }
        }
    }
}
