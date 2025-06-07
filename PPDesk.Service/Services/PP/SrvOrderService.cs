using AutoMapper;
using PPDesk.Abstraction.DTO.Repository.Order;
using PPDesk.Abstraction.DTO.Repository.User;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.DTO.Service.PP.Order;
using PPDesk.Abstraction.DTO.Service.PP.Order;
using PPDesk.Abstraction.DTO.Service.PP.User;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvOrderService : IForServiceCollectionExtension
    {
        Task<int> CountAllInformationOrdersAsync();
        Task<int> CountInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status, EnumTableType? type);
        Task CreateTableOrdersAsync();
        Task DeleteAllOrders();
        Task<IEnumerable<SrvInformationOrder>> GetAllInformationOrdersAsync();
        Task<IEnumerable<SrvInformationOrder>> GetInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status, EnumTableType? type, int page, int limit = 50);
        Task<IEnumerable<SrvOrder>> GetOrdersAsync(int page, int limit = 50);
        IEnumerable<SrvOrder> GetOrdersByEOrders(IEnumerable<SrvEOrder> eOrders);
        Task InsertOrdersAsync(IEnumerable<SrvOrder> srvOrders);
        Task UpsertOrdersAsync(IEnumerable<SrvOrder> srvOrders);
    }

    public class SrvOrderService : ISrvOrderService
    {
        private readonly IMdlOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public SrvOrderService(IMdlOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task CreateTableOrdersAsync()
        {
            await _orderRepository.CreateTableOrdersAsync();
        }

        public IEnumerable<SrvOrder> GetOrdersByEOrders(IEnumerable<SrvEOrder> eOrders)
        {
            IEnumerable<SrvEAttendee> eAttendees = eOrders.SelectMany(x => x.Attendees);
            var ordersTemp = _mapper.Map<List<SrvOrder>>(eAttendees);

            var orders = new List<SrvOrder>();
            foreach(var order in ordersTemp)
            {
                var temp = orders.FirstOrDefault(x => x != null && x.Name == order.Name && x.TableIdEventbride == order.TableIdEventbride);
                if (temp != null)
                {
                    temp.Quantity++;
                }
                else
                {
                    orders.Add(order);
                }
            }

            return orders;
        }

        public async Task<IEnumerable<SrvOrder>> GetOrdersAsync(int page, int limit = 50)
        {
            var mdlOrders = await _orderRepository.GetOrdersAsync(page, limit);
            return _mapper.Map<IEnumerable<SrvOrder>>(mdlOrders);
        }

        public async Task<IEnumerable<SrvInformationOrder>> GetInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status, EnumTableType? type, int page, int limit = 50)
        {
            var mdlOrders = await _orderRepository.GetInformationOrdersAsync(name, nameOrder, gdrName, master, status, type, page, limit);
            return _mapper.Map<IEnumerable<SrvInformationOrder>>(mdlOrders);
        }

        public async Task<IEnumerable<SrvInformationOrder>> GetAllInformationOrdersAsync()
        {
            var mdlOrders = await _orderRepository.GetAllInformationOrdersAsync();
            return _mapper.Map<IEnumerable<SrvInformationOrder>>(mdlOrders);
        }

        public async Task<int> CountInformationOrdersAsync(string name, string nameOrder, string gdrName, string master, EnumEventStatus? status, EnumTableType? type)
        {
            return await _orderRepository.CountInformationOrdersAsync(name, nameOrder, gdrName, master, status, type);
        }

        public async Task<int> CountAllInformationOrdersAsync()
        {
            return await _orderRepository.CountAllInformationOrdersAsync();
        }

        public async Task InsertOrdersAsync(IEnumerable<SrvOrder> srvOrders)
        {
            var mdlOrders = _mapper.Map<IEnumerable<MdlOrder>>(srvOrders);
            await _orderRepository.InsertOrdersAsync(mdlOrders);
        }

        public async Task UpsertOrdersAsync(IEnumerable<SrvOrder> srvOrders)
        {
            var mdlOrders = _mapper.Map<IEnumerable<MdlOrder>>(srvOrders);
            await _orderRepository.UpsertOrdersAsync(mdlOrders);
        }

        public async Task DeleteAllOrders()
        {
            await _orderRepository.DeleteAllOrdersAsync();
        }
    }
}
