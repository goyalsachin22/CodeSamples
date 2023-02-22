using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LoadPersonalData
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _people;
        private string _selectedCountry;
        private string _sortBy;
        private bool _sortAscending;

        public MainViewModel()
        {
            Reload();


            Countries = new ObservableCollection<string>(People.Select(p => p.Country).Distinct());
            Countries.Add("All");
            // Default sorting
            _sortBy = "Name";
            _sortAscending = true;
            SortPeople();
        }

        private void Reload()
        {
            if(People!=null)
                People.Clear();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Persons.csv");
            LoadPeople(filePath);
        }

        public ObservableCollection<Person> People
        {
            get { return _people; }
            set
            {
                _people = value;
                OnPropertyChanged(nameof(People));
            }
        }


        public ObservableCollection<string> Countries { get; set; }

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                Reload();
                FilterPeople();
            }
        }

        private void FilterPeople()
        {
            if (string.IsNullOrEmpty(SelectedCountry) || SelectedCountry == "All")
            {
                People = new ObservableCollection<Person>(_people);
            }
            else
            {
                People = new ObservableCollection<Person>(_people.Where(p => p.Country == SelectedCountry));
            }

            SortPeople();
        }

        //public ObservableCollection<Person> FilteredPeople => string.IsNullOrEmpty(SelectedCountry)
        //    ? People
        //    : new ObservableCollection<Person>(People.Where(p => p.Country == SelectedCountry));

        // Property for the current sort direction
        public bool SortAscending
        {
            get => _sortAscending;
            set
            {
                _sortAscending = value;
                SortPeople();
            }
        }

        // Property for the current sort field
        public string SortBy
        {
            get => _sortBy;
            set
            {
                _sortBy = value;
                SortPeople();
                OnPropertyChanged(nameof(SortBy));
                OnPropertyChanged(nameof(People));
            }
        }

        private void SortPeople()
        {
            if (_sortBy == "Name")
            {
                if (_sortAscending)
                {
                   People  = new ObservableCollection<Person>(People.OrderBy(p => p.Name));
                }
                else
                {
                    People = new ObservableCollection<Person>(People.OrderByDescending(p => p.Name));
                }
            }
            else if (_sortBy == "Country")
            {
                if (_sortAscending)
                {
                    People = new ObservableCollection<Person>(People.OrderBy(p => p.Country));
                }
                else
                {
                    People = new ObservableCollection<Person>(People.OrderByDescending(p => p.Country));
                }
            }
        }

        public void LoadPeople(string filePath)
        {
            List<Person> people = new List<Person>();

            IEnumerable<string> dataLines = File.ReadLines(filePath).Skip(1);

            foreach (string dataLine in dataLines)
            {
                string[] dataFields = dataLine.Split(',');
                Person person = new Person
                {
                    Name = dataFields[0],
                    Country = dataFields[1],
                    Address = dataFields[2],
                    PostalZip = dataFields[3],
                    Email = dataFields[4],
                    PhoneNumber = dataFields[5]
                };
                people.Add(person);
            }

            People = new ObservableCollection<Person>(people);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

     
    }
}
