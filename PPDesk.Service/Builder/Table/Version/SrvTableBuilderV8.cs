using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP.Table;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Builder.Table.Version
{
    public interface ISrvTableBuilderV8 : IForServiceCollectionExtension
    {
        SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass);
    }

    public class SrvTableBuilderV8 : ISrvTableBuilderV8
    {
        public SrvTableBuilderV8() { }

        public SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass)
        {
            SrvTable builder = new SrvTable();

            builder.Type = TypeBuilder(eTicketClass.DisplayName);

            string forDate = builder.Type == EnumTableType.Multitavolo ? eTicketClass.Name : eTicketClass.Description;
            forDate = eTicketClass.Id == "2402123933" ? "Orario: ORE 15.00" : forDate;

            string forName = builder.Type == EnumTableType.Multitavolo ? eTicketClass.Description : eTicketClass.Name;

            builder.GdrName = GdrNameBuilder(forName, builder.Type);
            builder.Description = DescriptionBuilder(eTicketClass.Description);
            builder.StartDate = StartDateBuilder(forDate, eTicketClass.SalesEnd, builder.Type);
            builder.EndDate = EndDateBuilder(forDate, eTicketClass.SalesEnd, builder.Type);
            builder.Master = MasterBuilder(eTicketClass.Description);

            return builder;
        }

        private EnumTableType TypeBuilder(string displayName)
        {
            if (displayName.Contains("Multitavolo GDR") || displayName.Contains("TORNEO GDR"))
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
            string s = split.FirstOrDefault(x => x.Contains("Master"));
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(s.IndexOf(":") + 1);
            }

            return ExtractText(s);
        }

        private DateTime EndDateBuilder(string description, DateTime date, EnumTableType type)
        {
            var split = description.Split('\n');

            if (type == EnumTableType.SessioneGdr)
            {
                description = split.FirstOrDefault(x => x.Contains("Orario"));
            }

            return ExtractDate(description, date, type, false);
        }

        private DateTime StartDateBuilder(string description, DateTime date, EnumTableType type)
        {
            var split = description.Split('\n');

            if (type == EnumTableType.SessioneGdr)
            {
                description = split.FirstOrDefault(x => x.Contains("Orario"));
            }

            return ExtractDate(description, date, type, true);
        }

        private DateTime ExtractDate(string description, DateTime date, EnumTableType type, bool start)
        {
            try
            {

                if(!string.IsNullOrEmpty(description))
                {
                    var split = description.Split('\n');
                    string s = split[0];

                    if (s.Contains("0RE") || s.Contains("ORE"))
                    {
                        s = s.Replace("0RE", "ORE");
                        s = s.Substring(s.IndexOf("ORE") + 3);
                    }

                    s = s.Replace("Orario:", "");

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
            }
            catch
            {
            }

            return DateTime.MinValue;
        }

        private string DescriptionBuilder(string description)
        {
            List<string> split = description.Split('\n').ToList();
            int index = split.LastIndexOf(split.FirstOrDefault(x => x.Contains("Durata")));

            split = split.GetRange(index + 1, split.Count - (index + 1));

            return string.Join("\n", split);
        }

        private string GdrNameBuilder(string name, EnumTableType type)
        {
            try
            {
                var split = name.Split('-');
                return ExtractText(split[1]);
            }
            catch (Exception ex)
            {
                return "???";
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
