using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoserviceApp
{
    internal class FilterService
    {
        public static IQueryable<Client> Filters(IQueryable<Client> data, string searchText, int genderCode)
        {
            string email = TakeEmail(searchText);
            string phone = TakePhone(searchText);
            string fullName = searchText;

            if (!string.IsNullOrEmpty(searchText))
            {
                if (phone != null)
                    fullName = fullName.Replace(phone, "");
                if (email != null)
                    fullName = fullName.Replace(email, "");

                fullName = fullName.Trim();
            }

            data = FilterByGender(data, genderCode);
            data = FilterByEmail(data, email);
            data = FilterByPhone(data, phone);

            foreach (var s in fullName.Split(' '))
            {
                data = FilterByName(data, s);
            }

            return data;
        }

        private static string TakeEmail(string text)
        {

            text = text.Trim();
            foreach (string s in text.Split(' '))
            {
                if (s.IndexOf('@') != -1) return s;
            }

            return null;
        }

        private static string TakePhone(string text)
        {
            text = text.Trim();
            foreach (string s in text.Split(' '))
            {
                if (s.Any(char.IsDigit)) return s;
            }

            return null;
        }

        private static IQueryable<Client> FilterByGender(IQueryable<Client> data, int genderCode)
        {
            if (genderCode == 0)
                return data;

            return data.Where(o => o.GenderCode == genderCode);
        }

        private static IQueryable<Client> FilterByEmail(IQueryable<Client> data, string email)
        {
            if (string.IsNullOrEmpty(email))
                return data;

            return data.Where(o => o.Email.IndexOf(email) != -1);
        }

        private static IQueryable<Client> FilterByPhone(IQueryable<Client> data, string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return data;

            return data.Where(o => o.Phone.IndexOf(phone) != -1);
        }

        private static IQueryable<Client> FilterByName(IQueryable<Client> data, string name)
        {
            if (string.IsNullOrEmpty(name))
                return data;

            return data.Where(o => o.FirstName.IndexOf(name) != -1 || o.LastName.IndexOf(name) != -1 || o.Patronymic.IndexOf(name) != -1);
        }
    }
}
