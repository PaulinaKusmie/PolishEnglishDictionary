using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PolishEnglishDictionary.ViewModel
{
    class DictionaryPageViewModel : INotifyPropertyChanged
    {
        #region Fields
        private string word = string.Empty;
        private string userTranslate = string.Empty;
        private string effect = string.Empty;
        private Color effectColor;
        public List<Tuple<string, string>> myDic = new List<Tuple<string, string>>();

        INavigation Navigation => Application.Current.MainPage.Navigation;
        #endregion

        public ICommand TranslateCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        #region Properties
        public string Word
        {
            get => word;
            set
            {
  
                word = value;
                
                OnPropertyChanged();
            }
        }
        
        public string UserTranslate
        {
            get => userTranslate;
            set
            {

                userTranslate = value;

                OnPropertyChanged();
            }
        }



        public string Effect
        {
            get => effect;
            set
            {

                effect = value;

                OnPropertyChanged();
            }
        }


        public Color EffectColor
        {
            get => effectColor;
            set
            {

                effectColor = value;

                OnPropertyChanged();
            }
        }

        


        #endregion

        public DictionaryPageViewModel()
        {
            TranslateCommand = new Command(Translate);
            CheckCommand = new Command(Check);

            AddTuple("Dog","Pies");
            AddTuple("Cat", "Kot");
            AddTuple("Monkey", "Małpa");

            Word = myDic[0].Item1;
        }


        private void Translate()
        {



        }


        
        private void Check()
        {
            string answear = string.Empty;

            foreach (var v in myDic)
            {
                if (v.Item1 == Word)
                {
                    answear = v.Item2;
                    break;
                } 
            }


           if(UserTranslate == answear)
           {
                Effect = "Gratulacje! Poprawna odpowiedź!";
                EffectColor = Color.Green;
           }
           else
           {
                Effect = $"Nie udało się, poprawna odpowiedź to {answear}";
                EffectColor = Color.Red;
           }

        }


        public void AddTuple(string englishWord, string polishWorld)
        {
            myDic.Add(new Tuple<string, string>(englishWord, polishWorld));
        }

        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class Dictionary : INotifyPropertyChanged
    {
        private string englishWord;
        private string polishWorld;
        public List<Tuple<string, string>> myDic = new List<Tuple<string, string>>();


        public string EnglishWord
        {
            get => englishWord;
            set
            {

                englishWord = value;

                OnPropertyChanged();
            }
        }

        public string PolishWorld
        {
            get => polishWorld;
            set
            {

                polishWorld = value;

                OnPropertyChanged();
            }
        }

        public Dictionary()
        {
            
        }
        public Dictionary(string englishWord, string polishWorld)
        {
            this.EnglishWord = englishWord;
            this.PolishWorld = polishWorld;

            AddTuple(englishWord, polishWorld);
        }

        public void AddTuple(string englishWord, string polishWorld)
        {
            myDic.Add(new Tuple<string, string>(englishWord, polishWorld));
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
