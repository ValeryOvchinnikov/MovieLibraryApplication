using ClosedXML.Excel;
using IdentityModel.Client;
using LinqKit;
using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Models;
using MovieLibrary.WPF.Comands;
using MovieLibrary.WPF.Comands.Pagination;
using MovieLibrary.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;

namespace MovieLibrary.WPF.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        HttpClient authClient = new HttpClient();
        HttpClient webApiClient = new HttpClient();
        private readonly IDialogService _dialogService;
        private ICommand _openCSVCommand;
        private ICommand _writeXMLCommand;
        private ICommand _writeXLSCommand;
        private ICommand _applyFilterCommand;
        private ICommand _resetFilterCommand;
        private string _filterMovieName = "";
        private string _filterMovieYear = "";
        private string _filterDirectorFirstName = "";
        private string _filterDirectorLastName = "";
        private string _filterMovieRating;
        private readonly int _itemPerPage = 50;
        private int _itemcount;
        private int _totalPages;
        private int _currentPageIndex;
        private bool _isEnabled = true;
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<MovieDTO> _movieList = new();
        public ExpressionStarter<MovieDTO> filter;
        public CollectionViewSource ViewList { get; set; }
        public ReadOnlyObservableCollection<MovieDTO> MovieList
        {
            get { return new ReadOnlyObservableCollection<MovieDTO>(_movieList); }
        }

        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand FirstCommand { get; private set; }
        public ICommand LastCommand { get; private set; }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("isEnabled");
            }
        }

        public string FilterMovieName
        {
            get
            {
                return _filterMovieName;
            }
            set
            {
                _filterMovieName = value;
                RaisePropertyChanged("FilterMovieName");
            }
        }


        public string FilterMovieYear
        {
            get
            {
                return _filterMovieYear;
            }
            set
            {
                _filterMovieYear = value;
                RaisePropertyChanged("FilterMovieYear");
            }
        }

        public string FilterDirectorFirstName
        {
            get
            {
                return _filterDirectorFirstName;
            }
            set
            {
                _filterDirectorFirstName = value;
                RaisePropertyChanged("FilterDirectorFirstName");
            }
        }

        public string FilterDirectorLastName
        {
            get
            {
                return _filterDirectorLastName;
            }
            set
            {
                _filterDirectorLastName = value;
                RaisePropertyChanged("FilterDirectorLastName");
            }
        }

        public string FilterMovieRating
        {
            get
            {
                return _filterMovieRating;
            }
            set
            {
                _filterMovieRating = value;
                RaisePropertyChanged("FilterMovieRating");
            }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                _currentPageIndex = value;
                RaisePropertyChanged("CurrentPage");
            }
        }

        public int CurrentPage
        {
            get { return _currentPageIndex + 1; }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            private set
            {
                _totalPages = value;
                RaisePropertyChanged("TotalPages");
            }
        }

        public void ShowNextPage()
        {
            CurrentPageIndex++;
            ViewList.View.Refresh();
        }

        public void ShowPreviousPage()
        {
            CurrentPageIndex--;
            ViewList.View.Refresh();
        }

        public void ShowFirstPage()
        {
            CurrentPageIndex = 0;
            ViewList.View.Refresh();
        }

        public void ShowLastPage()
        {
            CurrentPageIndex = TotalPages - 1;
            ViewList.View.Refresh();
        }


        public ICommand OpenCSVCommand
        {
            get
            {
                if (_openCSVCommand == null) _openCSVCommand = new RelayCommand(p => OpenCSV());
                return _openCSVCommand;
            }
        }

        public ICommand WriteXMLCommand
        {
            get
            {
                if (_writeXMLCommand == null) _writeXMLCommand = new RelayCommand(p => WriteXML());
                return _writeXMLCommand;
            }
        }

        public ICommand WriteXLSCommand
        {
            get
            {
                if (_writeXLSCommand == null) _writeXLSCommand = new RelayCommand(p => WriteXLS());
                return _writeXLSCommand;
            }
        }

        public ICommand ApplyFilterCommand
        {
            get
            {
                if (_applyFilterCommand == null) _applyFilterCommand = new RelayCommand(p => ApplyFilters());
                return _applyFilterCommand;
            }
        }

        public ICommand ResetFilterCommand
        {
            get
            {
                if (_resetFilterCommand == null) _resetFilterCommand = new RelayCommand(p => ResetFiltersAsync());
                return _resetFilterCommand;
            }
        }

        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            webApiClient.BaseAddress = new Uri("https://localhost:6001");
            webApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            RequestAndSetAccessTokenAsync();
            ViewList = new CollectionViewSource();
            NextCommand = new NextPageCommand(this);
            PreviousCommand = new PreviousPageCommand(this);
            FirstCommand = new FirstPageCommand(this);
            LastCommand = new LastPageCommand(this);
            ViewList.Filter += new FilterEventHandler(PageFilter);

            CurrentPageIndex = 0;
        }

        private void PageFilter(object sender, FilterEventArgs e)
        {
            int index = _movieList.IndexOf((MovieDTO)e.Item);
            if (index >= _itemPerPage * CurrentPageIndex && index < _itemPerPage * (CurrentPageIndex + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        public async Task RequestAndSetAccessTokenAsync()
        {
            EnableButtons(false);

            var discovery = await authClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovery.IsError)
            {
                MessageBox.Show(discovery.Error);
                return;
            }

            var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = "WPF App",
                ClientSecret = "secret",
                Scope = "movieLibraryApi"
            });

            if (tokenResponse.IsError)
            {
                MessageBox.Show(tokenResponse.Error);
                return;
            }

            webApiClient.SetBearerToken(tokenResponse.AccessToken);
            EnableButtons(true);
        }
        private void CalculateTotalPages()
        {
            _itemcount = ViewList.View.SourceCollection.Cast<MovieDTO>().Count();
            if (_itemcount % _itemPerPage == 0)
            {
                TotalPages = (_itemcount / _itemPerPage);
            }
            else
            {
                TotalPages = (_itemcount / _itemPerPage) + 1;
            }
        }

        private async Task OpenCSV()
        {
            var file = _dialogService.OpenFileDialog(".csv", "Doc (.csv)|*.csv*");
            await ReadCSV(file);
            var movies = await GetMovies();
            _movieList = new ObservableCollection<MovieDTO>(movies);
            ViewList.Source = MovieList;
            CalculateTotalPages();
        }

        public async Task ReadCSV(string fileName)
        {
            try
            {
                EnableButtons(false);
                using var file = new System.IO.StreamReader(fileName);
                string line;
                while ((line = await file.ReadLineAsync()) != null)
                {
                    string[] data = line.Split(';');
                    var newMovie = new MovieDTO { Name = data[2], Year = Convert.ToInt32(data[3]), Rating = Convert.ToInt32(data[4]), DirectorFirstName = data[0], DirectorLastName = data[1] };
                    await AddMovie(newMovie);
                }
                EnableButtons(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Caught",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task WriteXML()
        {
            try
            {
                EnableButtons(false);
                var file = _dialogService.SaveFileDiaolog(".xml", "Doc (.xml)|*.xml*");

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter textWriter = XmlWriter.Create(file, settings);
                textWriter.WriteStartDocument();
                textWriter.WriteStartElement("Movies");
                var movies = await GetMovies();
                foreach (MovieDTO movie in movies)
                {

                    textWriter.WriteStartElement("Record");
                    textWriter.WriteAttributeString("id", movie.Id.ToString());

                    textWriter.WriteStartElement("FirstName");
                    textWriter.WriteString(movie.DirectorFirstName);
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("LastName");
                    textWriter.WriteString(movie.DirectorLastName);
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("MovieName");
                    textWriter.WriteString(movie.Name);
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("MovieYear");
                    textWriter.WriteString(movie.Year.ToString());
                    textWriter.WriteEndElement();

                    textWriter.WriteStartElement("MovieRating");
                    textWriter.WriteString(movie.Rating.ToString());
                    textWriter.WriteEndElement();

                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();
                textWriter.WriteEndDocument();
                textWriter.Flush();
                EnableButtons(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Caught",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task WriteXLS()
        {
            try
            {
                EnableButtons(false);
                var file = _dialogService.SaveFileDiaolog(".xlsx", "Doc (.xlsx)|*.xlsx*");

                var movies = await GetMovies();
                var listOfMovies = movies.ToList();
                _movieList = new ObservableCollection<MovieDTO>(listOfMovies);
                ViewList.Source = MovieList;
                ViewList.View.Refresh();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Movies");
                    worksheet.Cell("A1").Value = "FirstName";
                    worksheet.Cell("B1").Value = "LastName";
                    worksheet.Cell("C1").Value = "MovieName";
                    worksheet.Cell("D1").Value = "Year";
                    worksheet.Cell("E1").Value = "Rating";


                    for (int i = 0; i < listOfMovies.Count; i++)
                    {
                        var movie = listOfMovies[i];
                        worksheet.Cell(i + 2, 1).Value = movie.DirectorFirstName;
                        worksheet.Cell(i + 2, 2).Value = movie.DirectorLastName;
                        worksheet.Cell(i + 2, 3).Value = movie.Name;
                        worksheet.Cell(i + 2, 4).Value = movie.Year;
                        worksheet.Cell(i + 2, 5).Value = movie.Rating;
                    }
                    workbook.SaveAs(file);
                }
                EnableButtons(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception Caught",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task ApplyFilters()
        {
            Filter newFilter = new()
            {
                FilterMovieName = FilterMovieName,
                FilterMovieYear = FilterMovieYear,
                FilterDirectorFirstName = FilterDirectorFirstName,
                FilterDirectorLastName = FilterDirectorLastName,
                FilterMovieRating = FilterMovieRating,
            };

            var properties = from p in newFilter.GetType().GetProperties()
                             where !string.IsNullOrEmpty((string)p.GetValue(newFilter, null))
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(newFilter, null).ToString());
            string queryString = String.Join("&", properties.ToArray());
            EnableButtons(false);
            var response = await webApiClient.GetAsync($"api/Movies/GetMoviesByFilter?{queryString}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var movies = await response.Content.ReadFromJsonAsync<List<MovieDTO>>();
                _movieList = new ObservableCollection<MovieDTO>(movies);
                ViewList.Source = MovieList;
                CurrentPageIndex = 0;
                CalculateTotalPages();
                ViewList.View.Refresh();
                EnableButtons(true);
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        public async Task ResetFiltersAsync()
        {
            EnableButtons(false);
            FilterMovieName = "";
            FilterMovieYear = "";
            FilterDirectorFirstName = "";
            FilterDirectorLastName = "";
            FilterMovieRating = "";
            var movies = await GetMovies();
            _movieList = new ObservableCollection<MovieDTO>(movies);
            ViewList.Source = MovieList;
            ViewList.View.Refresh();
            CurrentPageIndex = 0;
            CalculateTotalPages();
            ViewList.View.Refresh();
            EnableButtons(true);

        }

        public async Task<IEnumerable<MovieDTO>> GetMovies()
        {
            var response = await webApiClient.GetAsync("api/Movies/GetMovies");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var movies = await response.Content.ReadFromJsonAsync<IEnumerable<MovieDTO>>();
                return movies;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return null;
            }
        }

        public void EnableButtons(bool status)
        {
            IsEnabled = status;
        }

        public async Task AddMovie(MovieDTO movie)
        {
            var response = await webApiClient.PostAsJsonAsync("api/Movies/CreateMovie", movie);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
