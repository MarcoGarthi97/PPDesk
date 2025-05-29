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
    public interface ISrvTableBuilderV1 : IForServiceCollectionExtension
    {
        SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass);
    }

    public class SrvTableBuilderV1 : ISrvTableBuilderV1
    {
        public SrvTableBuilderV1() { }

        public SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass)
        {
            SrvTable builder = new();

            builder.GdrName = GdrNameBuilder(eTicketClass.Name);
            builder.Description = DescriptionBuilder(eTicketClass.Description);
            builder.StartDate = StartDateBuilder(eTicketClass.Description, eTicketClass.SalesEnd);
            builder.EndDate = EndDateBuilder(eTicketClass.Description, eTicketClass.SalesEnd);
            builder.Master = MasterBuilder(eTicketClass.Name);
            builder.Type = TypeBuilder(eTicketClass.DisplayName);

            return builder;
        }

        private EnumTableType TypeBuilder(string displayName)
        {
            if(displayName.Contains("Multitavolo GDR"))
            {
                return EnumTableType.Multitavolo;
            }
            else
            {
                return EnumTableType.SessioneGdr;
            }
        }

        private string MasterBuilder(string name)
        {
            return ExtractText(name, false);
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

                if(s.Contains("0RE") || s.Contains("ORE"))
                {
                    s = s.Replace("0RE", "ORE");
                    s = s.Substring(s.IndexOf("ORE") + 3);
                }

                split = s.Split('-');
                if(split.Length > 2)
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
            int index = split.LastIndexOf(string.Empty);

            return split[index + 1];
        }

        private string GdrNameBuilder(string name)
        {
            return ExtractText(name, true);
        }

        private string ExtractText(string name, bool gdr)
        {
            string[] split = name.Split('-');
            string s = string.Empty;

            if(split.Length > 1)
            {
                s = gdr ? split[0].Trim() : split[1].Trim();

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
                s = gdr ? s : split[0].Trim();
            }
            
            return s;
        }
    }
}
