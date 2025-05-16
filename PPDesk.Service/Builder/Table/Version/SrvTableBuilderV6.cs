using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Builder.Table.Version
{
    public interface ISrvTableBuilderV6 : IForServiceCollectionExtension
    {
        SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass);
    }

    public class SrvTableBuilderV6 : ISrvTableBuilderV6
    {
        public SrvTableBuilderV6() { }

        public SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass)
        {
            SrvTable builder = new SrvTable();

            builder.Type = TypeBuilder(eTicketClass.DisplayName);

            string forDate = builder.Type == EnumTableType.Multitavolo ? eTicketClass.Description : eTicketClass.Name;
            string forMaster = builder.Type == EnumTableType.SessioneGdr ? eTicketClass.Description : eTicketClass.Name;

            builder.GdrName = GdrNameBuilder(forMaster, builder.Type);
            builder.Description = DescriptionBuilder(eTicketClass.Description);
            builder.StartDate = StartDateBuilder(forDate, eTicketClass.SalesEnd);
            builder.EndDate = EndDateBuilder(forDate, eTicketClass.SalesEnd);
            builder.Master = builder.Type == EnumTableType.Multitavolo ? "Peter Pan" : MasterBuilder(eTicketClass.Description);

            return builder;
        }

        private EnumTableType TypeBuilder(string displayName)
        {
            if (displayName.Contains("Multitavolo GDR"))
            {
                return EnumTableType.Multitavolo;
            }
            else
            {
                return EnumTableType.SessioneGdr;
            }
        }

        private string MasterBuilder(string description)
        {
            var split = description.Split('\n');
            split = split[0].Split('-');

            return ExtractText(split[0]);
        }

        private DateTime EndDateBuilder(string description, DateTime date)
        {
            return ExtractDate(description, date, false);
        }

        private DateTime StartDateBuilder(string description, DateTime date)
        {
            return ExtractDate(description, date, true);
        }

        private DateTime ExtractDate(string description, DateTime date, bool start)
        {
            try
            {
                var split = description.Split('\n');
                string s = split[0];

                if (s.Contains("0RE") || s.Contains("ORE"))
                {
                    s = s.Replace("0RE", "ORE");
                    s = s.Substring(s.IndexOf("ORE") + 3);
                }

                split = s.Split('-');
                if (split.Length > 2)
                {
                    split = split.ToList().GetRange(split.Length - 2, 2).ToArray();
                }

                s = start ? split[0] : split[1];
                split = s.Split('.');

                int hours = Convert.ToInt32(split[0]);
                int minutes = Convert.ToInt32(split[1]);

                return date.Date.Date.AddHours(hours).AddMinutes(minutes);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        private string DescriptionBuilder(string description)
        {
            List<string> split = description.Split('\n').ToList();
            split = split.GetRange(1, split.Count - 1);

            return string.Join("\n", split);
        }

        private string GdrNameBuilder(string name, EnumTableType type)
        {
            if (type == EnumTableType.Multitavolo)
            {
                return name;
            }
            else
            {
                var split = name.Split('\n');
                string s = split.FirstOrDefault(x => x.Contains("Master"));
                if (!string.IsNullOrEmpty(s))
                {
                    s = s.Substring(s.IndexOf(":") + 1);
                }

                return ExtractText(s);
            }
        }

        private string ExtractText(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Trim();
                s = s.Replace(".", "");

                int end = 1000;
                if (s.Contains("\"") && s.IndexOf("\"") < end)
                {
                    end = s.IndexOf("\"");
                }
                else if (s.Contains("(") && s.IndexOf("(") < end)
                {
                    end = s.IndexOf("(");
                }

                if (end != 1000)
                {
                    s = s.Substring(0, end - 1);
                }
            }
            else
            {
                s = "Peter Pan";
            }

            return s;
        }
    }
}
