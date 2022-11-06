using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PolishEnglishDictionary.ViewModel
{
    class WordGridViewModel : INotifyPropertyChanged
    {
        #region -- fields --
        public ObservableCollection<Dictionary> Words { get; set; }
        private string englishWord;
        private string polishWord;
        bool iKnow = false;
        string know = string.Empty;
        int id = 0;

        #endregion

        #region -- properties --
        public string EnglishWord
        {
            get => englishWord;
            set
            {

                englishWord = value;
                OnPropertyChanged();
            }
        }


        public string PolishWord
        {
            get => polishWord;
            set
            {

                polishWord = value;
                OnPropertyChanged();
            }
        }


        public bool IKnow
        {
            get => iKnow;

            set
            {

                iKnow = value;

                OnPropertyChanged();
            }
        }


        public string Know
        {
            get => know;
            set
            {

                know = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get => id;
            set
            {

                id = value;
                OnPropertyChanged();
            }
        }

        //public Dictionary WordItem
        //{
        //    get => wordItem;
        //    set
        //    {

        //        wordItem = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #region -- commands --
        public ICommand EditWordCommand { get; set; }
        public ICommand DeleteWordCommand { get; set; }
        #endregion
        public WordGridViewModel()
        {
            Dictionary Dic = new Dictionary();
            List<int> count = Dic.FindCount();
            Words = Dic.ListDictionary(count[0], count[1]);

            EditWordCommand = new DelegateCommand<Dictionary>(EditWord);
            DeleteWordCommand = new DelegateCommand<Dictionary>(DeleteWord);

        }
        private  void DeleteWord(Dictionary userWord)
        {
            Dictionary Dic = new Dictionary();
            Dic.DeleteWord(userWord.Id);
            List<int> count = Dic.FindCount();
            Words = Dic.ListDictionary(count[0], count[1]);
        }

        private async void EditWord(Dictionary userWord)
        {
            OnActionSheetSimpleClicked(null,null);

            //await EditWordAsync(userWord);
        }

        async Task OnActionSheetSimpleClicked(object sender, EventArgs e)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Co chciałbyś zmienić", "Anuluj", null, "Wyraz", "Tłumaczenie", "Już umiem");
            Debug.WriteLine("Action: " + action);

            if(action == "Już umiem")
            {
                await OnAlertYesNoClicked(action);
            }
            else
            {
                await EditWordAsync(action);
            }
            
        }

        private async Task EditWordAsync(string s)
        {
           string result;
           result = await App.Current.MainPage.DisplayPromptAsync($"Podaj nowy {s}", s);
     

        }

        async Task OnAlertYesNoClicked(string s)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Umiem", $"Czy znasz tłumaczenia słowa {s}", "Tak", "Nie");
            Debug.WriteLine("Answer: " + answer);
        }

        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    

    }
}
