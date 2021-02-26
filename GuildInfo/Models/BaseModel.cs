using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildInfo.Models
{
    /// <summary>
    /// Klasa bazowa
    /// </summary>
    public class BaseModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
