using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildInfo.Models
{
    public class CheckedMember: Member
    {
        public bool IsChecked { get; set; }
        public Member Member { get; }

        public CheckedMember() { }
        public CheckedMember(Member member): this()
        {
            if (member == null)
                return;

            this.Member = member;
            this.MemberId = member.MemberId;
            this.Name = member.Name;
            this.Nickname = member.Nickname;
            this.Level = member.Level;
            this.Profession = member.Profession;
            this.Race = member.Race;
            this.Guild = member.Guild;
        }
    }
}
