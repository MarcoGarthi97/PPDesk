using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP.Table;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Builder.Table.Version;
using System.Collections.Generic;

namespace PPDesk.Service.Builder.Table
{
    public interface ISrvTableOrchestratorBuilder : IForServiceCollectionExtension
    {
        SrvTable TableByTickeClassBuilder(SrvETicketClass ticketClasse);
    }

    public class SrvTableOrchestratorBuilder : ISrvTableOrchestratorBuilder
    {
        private readonly ISrvTableBuilderV1 _tableBuilderV1;
        private readonly ISrvTableBuilderV2 _tableBuilderV2;
        private readonly ISrvTableBuilderV3 _tableBuilderV3;
        private readonly ISrvTableBuilderV4 _tableBuilderV4;
        private readonly ISrvTableBuilderV5 _tableBuilderV5;
        private readonly ISrvTableBuilderV6 _tableBuilderV6;
        private readonly ISrvTableBuilderV7 _tableBuilderV7;
        private readonly ISrvTableBuilderV8 _tableBuilderV8;

        public SrvTableOrchestratorBuilder(ISrvTableBuilderV1 tableBuilderV1, ISrvTableBuilderV2 tableBuilderV2, ISrvTableBuilderV3 tableBuilderV3, ISrvTableBuilderV4 tableBuilderV4, ISrvTableBuilderV5 tableBuilderV5, ISrvTableBuilderV6 tableBuilderV6, ISrvTableBuilderV7 tableBuilderV7, ISrvTableBuilderV8 tableBuilderV8)
        {
            _tableBuilderV1 = tableBuilderV1;
            _tableBuilderV2 = tableBuilderV2;
            _tableBuilderV3 = tableBuilderV3;
            _tableBuilderV4 = tableBuilderV4;
            _tableBuilderV5 = tableBuilderV5;
            _tableBuilderV6 = tableBuilderV6;
            _tableBuilderV7 = tableBuilderV7;
            _tableBuilderV8 = tableBuilderV8;
        }

        public SrvTable TableByTickeClassBuilder(SrvETicketClass ticketClasse)
        {
            switch (ticketClasse.EventId)
            {
                case "369202503627":
                    return _tableBuilderV1.TableByTickeClassBuilder(ticketClasse);
                case "432016923247":
                    return _tableBuilderV2.TableByTickeClassBuilder(ticketClasse);
                case "616489525767":
                    return _tableBuilderV3.TableByTickeClassBuilder(ticketClasse);
                case "734800928357":
                    return _tableBuilderV4.TableByTickeClassBuilder(ticketClasse);
                case "795199451967":
                    return _tableBuilderV5.TableByTickeClassBuilder(ticketClasse);
                case "875547073867":
                    return _tableBuilderV6.TableByTickeClassBuilder(ticketClasse);
                case "910087665707":
                    return _tableBuilderV3.TableByTickeClassBuilder(ticketClasse);
                case "1040189273117":
                    return _tableBuilderV4.TableByTickeClassBuilder(ticketClasse);
                case "1200646709629":
                    return _tableBuilderV7.TableByTickeClassBuilder(ticketClasse);
                case "1312805258869":
                    return _tableBuilderV8.TableByTickeClassBuilder(ticketClasse);
                default:
                    return _tableBuilderV1.TableByTickeClassBuilder(ticketClasse);
            }
        }
    }
}
