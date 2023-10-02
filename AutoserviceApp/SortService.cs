using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceApp
{
    static internal class SortService
    {
        public static IQueryable<Client> Sort(IQueryable<Client> data, int sortFactor)
        {
            switch (sortFactor)
            {
                case 0:
                    return SortByLastName(data);

                case 1:
                    return SortByLastActive(data);

                case 2:
                    return SortByCountOfActive(data);

                case 3:
                    return SortById(data);

                default:
                    return SortById(data);
            }
        }

        private static IQueryable<Client> SortByLastName(IQueryable<Client> data)
        {
            return data.OrderBy(o => o.LastName);
        }

        private static IQueryable<Client> SortByLastActive(IQueryable<Client> data)
        {
            return data.OrderByDescending(o => o.ClientService.OrderByDescending(e => e.StartTime).FirstOrDefault().StartTime);
        }

        private static IQueryable<Client> SortByCountOfActive(IQueryable<Client> data)
        {
            return data.OrderByDescending(o => o.ClientService.Count);
        }

        private static IQueryable<Client> SortById(IQueryable<Client> data)
        {
            return data.OrderBy(o => o.ID);
        }
    }
}
