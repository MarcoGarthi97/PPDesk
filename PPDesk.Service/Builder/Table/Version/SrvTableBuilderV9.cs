using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP.Table;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Builder.Table.Version
{
    public interface ISrvTableBuilderV9 : IForServiceCollectionExtension
    {
        SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass);
    }

    public class SrvTableBuilderV9 : ISrvTableBuilderV9
    {
        public SrvTableBuilderV9() { }

        public SrvTable TableByTickeClassBuilder(SrvETicketClass eTicketClass)
        {
            SrvTable builder = new SrvTable();

            builder.Type = TypeBuilder(eTicketClass.Description);

            // Multitavolo: GdrName is the part of Name before " - ORE" (e.g. "Aegis-01 "Bastion"")
            // SessioneGdr: GdrName is the part of Name after "-"   (e.g. "ORE 10.00 - Morkthulhu" -> "Morkthulhu")
            string forName = builder.Type == EnumTableType.Multitavolo
                ? eTicketClass.Name.Split(new string[] { " - " }, StringSplitOptions.None)[0]
                : eTicketClass.Name;

            builder.GdrName = GdrNameBuilder(forName, builder.Type);
            builder.Description = DescriptionBuilder(eTicketClass.Description, builder.Type);
            builder.StartDate = StartDateBuilder(eTicketClass.Description, eTicketClass.SalesEnd, builder.Type);
            builder.EndDate = EndDateBuilder(eTicketClass.Description, eTicketClass.SalesEnd, builder.Type);
            builder.Master = MasterBuilder(eTicketClass.Description);

            return builder;
        }

        // V9: type is detected from Description's first line (DisplayName no longer contains "Multitavolo GDR")
        private EnumTableType TypeBuilder(string description)
        {
            var firstLine = description.Split('\n')[0];
            if (firstLine.Contains("Multitavolo GDR") || firstLine.Contains("TORNEO GDR"))
            {
                return EnumTableType.Multitavolo;
            }
            else if (firstLine.Contains("Laboratorio"))
            {
                return EnumTableType.LaboratorioCreativo;
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
            return ExtractDate(description, date, type, false);
        }

        private DateTime StartDateBuilder(string description, DateTime date, EnumTableType type)
        {
            return ExtractDate(description, date, type, true);
        }

        private DateTime ExtractDate(string description, DateTime date, EnumTableType type, bool start)
        {
            try
            {
                if (!string.IsNullOrEmpty(description))
                {
                    if (type == EnumTableType.Multitavolo)
                    {
                        // Multitavolo GDR: time range in parentheses of Description's first line
                        // e.g. "Multitavolo GDR di NOVA ... (SABATO 18 Aprile 15.00-18.30)"
                        string firstLine = description.Split('\n')[0];
                        int openParen = firstLine.IndexOf('(');
                        int closeParen = firstLine.IndexOf(')');
                        if (openParen >= 0 && closeParen >= 0)
                        {
                            string parenContent = firstLine.Substring(openParen + 1, closeParen - openParen - 1);
                            // e.g. "SABATO 18 Aprile 15.00-18.30"
                            var timeSplit = parenContent.Split('-');

                            string timeStr;
                            if (start)
                            {
                                // Last token of the first part: "SABATO 18 Aprile 15.00" -> "15.00"
                                timeStr = timeSplit[0].Trim().Split(' ').Last();
                            }
                            else
                            {
                                // Second part: "18.30"
                                timeStr = timeSplit[1].Trim();
                            }

                            var hourMin = timeStr.Split('.');
                            return date.Date.AddHours(Convert.ToInt32(hourMin[0])).AddMinutes(Convert.ToInt32(hourMin[1]));
                        }
                        else
                        {
                            // Torneo GDR: time in Name (e.g. "VALERIUS - ORE 14.30")
                            var parts = description.Split(new string[] { " - " }, StringSplitOptions.None);
                            int oreIndex = Array.FindIndex(parts, p => p.Contains("ORE") || p.Contains("0RE"));
                            if (oreIndex < 0) return DateTime.MinValue;

                            string timeStr;
                            if (start)
                            {
                                string orePart = parts[oreIndex].Replace("0RE", "ORE");
                                timeStr = orePart.Substring(orePart.IndexOf("ORE") + 3).Trim();
                            }
                            else
                            {
                                if (oreIndex + 1 >= parts.Length) return DateTime.MinValue;
                                timeStr = parts[oreIndex + 1].Trim();
                            }

                            var hourMin = timeStr.Split('.');
                            return date.Date.AddHours(Convert.ToInt32(hourMin[0])).AddMinutes(Convert.ToInt32(hourMin[1]));
                        }
                    }
                    else if (type == EnumTableType.LaboratorioCreativo)
                    {
                        // Dates are in Name: e.g. "SABATO 18 - ORE 10.30 - 12.00 - NARRA E DISEGNA!"
                        var parts = description.Split(new string[] { " - " }, StringSplitOptions.None);
                        int oreIndex = Array.FindIndex(parts, p => p.Contains("ORE") || p.Contains("0RE"));
                        if (oreIndex < 0) return DateTime.MinValue;

                        string timeStr;
                        if (start)
                        {
                            // "ORE 10.30" -> "10.30"
                            string orePart = parts[oreIndex].Replace("0RE", "ORE");
                            timeStr = orePart.Substring(orePart.IndexOf("ORE") + 3).Trim();
                        }
                        else
                        {
                            // element right after "ORE ..." -> "12.00"
                            timeStr = parts[oreIndex + 1].Trim();
                        }

                        var hourMin = timeStr.Split('.');
                        return date.Date.AddHours(Convert.ToInt32(hourMin[0])).AddMinutes(Convert.ToInt32(hourMin[1]));
                    }
                    else
                    {
                        // SessioneGdr: find "Orario:" line
                        // e.g. "Orario: 10.00 - 13.00"
                        var lines = description.Split('\n');
                        string orarioLine = lines.FirstOrDefault(x => x.Contains("Orario"));
                        if (string.IsNullOrEmpty(orarioLine)) return DateTime.MinValue;

                        string timeRange = orarioLine.Substring(orarioLine.IndexOf(':') + 1).Trim();
                        // e.g. "10.00 - 13.00"
                        var timeSplit = timeRange.Split('-');
                        string timeStr = start ? timeSplit[0].Trim() : timeSplit[1].Trim();

                        var hourMin = timeStr.Split('.');
                        return date.Date.AddHours(Convert.ToInt32(hourMin[0])).AddMinutes(Convert.ToInt32(hourMin[1]));
                    }
                }
            }
            catch
            {
            }

            return DateTime.MinValue;
        }

        private string DescriptionBuilder(string description, EnumTableType type)
        {
            List<string> split = description.Split('\n').ToList();

            if (type == EnumTableType.Multitavolo)
            {
                // Skip the first header line (contains type + date range)
                split = split.GetRange(1, split.Count - 1);
            }
            else if (type == EnumTableType.LaboratorioCreativo)
            {
                // Full description is already the narrative text, no metadata header to skip
            }
            else
            {
                // SessioneGdr: skip metadata lines up to and including "Orario:" line
                int index = split.LastIndexOf(split.FirstOrDefault(x => x.Contains("Orario")));
                if (index >= 0)
                {
                    split = split.GetRange(index + 1, split.Count - (index + 1));
                }
            }

            return string.Join("\n", split);
        }

        private string GdrNameBuilder(string name, EnumTableType type)
        {
            try
            {
                if (type == EnumTableType.Multitavolo)
                {
                    // name is already the part before " - ORE", e.g. "Aegis-01 "Bastion""
                    return ToPascalCase(ExtractText(name));
                }
                else if (type == EnumTableType.LaboratorioCreativo)
                {
                    // name is DisplayName split[0], e.g. "THE FRAMEWORK LAB"
                    return "Laboratorio Creativo";
                }
                else
                {
                    // name is e.g. "ORE 10.00 - Morkthulhu" -> take part after "-"
                    var split = name.Split('-');
                    return ToPascalCase(ExtractText(split[1]));
                }
            }
            catch (Exception ex)
            {
                return "???";
            }
        }

        private string ToPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
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
