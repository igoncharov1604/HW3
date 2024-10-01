using System;
using System.IO;

namespace Person1
{
    class Person
    {
        string name;
        int birth_year;
        double pay;

        public Person()  // конструктор без параметрів
        {
            name = "Anonimous";
            birth_year = 0;
            pay = 0;
        }

        public Person(string surname, int year, double salary)  // конструктор з параметрами
        {
            name = surname;
            birth_year = year;
            pay = salary;
            if (birth_year < 0) throw new FormatException("Неприпустимий рік народження.");
            if (pay < 0) throw new FormatException("Неприпустимий оклад.");
        }

        public override string ToString()  // перевизначений метод
        {
            return string.Format("Name: {0,30} birth: {1} pay: {2:F2}", name, birth_year, pay);
        }

        public int Compare(string name)  // порівняння прізвища
        {
            return string.Compare(this.name, 0, name + " ", 0, name.Length + 1, StringComparison.OrdinalIgnoreCase);
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int BirthYear
        {
            get { return birth_year; }
            set
            {
                if (value > 0) birth_year = value;
                else throw new FormatException();
            }
        }

        public double Pay
        {
            get { return pay; }
            set
            {
                if (value > 0) pay = value;
                else throw new FormatException();
            }
        }

        public static double operator +(Person pers, double a)
        {
            pers.pay += a;
            return pers.pay;
        }

        public static double operator -(Person pers, double a)
        {
            pers.pay -= a;
            if (pers.pay < 0) throw new FormatException("Оклад не може бути від'ємним.");
            return pers.pay;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person[] dbase = new Person[100];
            int n = 0;
            try
            {
                StreamReader f = new StreamReader("names.txt");  // Відкриваємо файл для читання
                string s;
                int i = 0;

                while ((s = f.ReadLine()) != null)  // Читання кожного рядка файлу
                {
                    // Розбиваємо рядок на частини
                    string[] parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 3)
                    {
                        string surname = parts[0];  // Прізвище та ініціали
                        int birthYear = int.Parse(parts[1]);  // Рік народження
                        double salary = double.Parse(parts[2]);  // Оклад

                        dbase[i] = new Person(surname, birthYear, salary);
                        Console.WriteLine(dbase[i]);
                        ++i;
                    }
                    else
                    {
                        throw new FormatException("Невірний формат даних у файлі.");
                    }
                }
                n = i;
                f.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Перевірте правильність імені і шляху до файлу!");
                return;
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Дуже великий файл!");
                return;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Помилка: " + e.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
                return;
            }

            // Пошук співробітника за прізвищем
            int n_pers = 0;
            double mean_pay = 0;
            Console.WriteLine("Введіть прізвище співробітника");
            string name;
            while ((name = Console.ReadLine()) != "")  // Цикл пошуку за прізвищем
            {
                bool not_found = true;
                for (int k = 0; k < n; ++k)
                {
                    Person pers = dbase[k];
                    if (pers.Compare(name) == 0)
                    {
                        Console.WriteLine(pers);
                        ++n_pers;
                        mean_pay += pers.Pay;
                        not_found = false;
                    }
                }
                if (not_found) Console.WriteLine("Такого співробітника немає");
                Console.WriteLine("Введіть прізвище співробітника або Enter для завершення");
            }
            if (n_pers > 0)
                Console.WriteLine("Середній оклад: {0:F2}", mean_pay / n_pers);

            Console.ReadKey();
        }
    }
}
