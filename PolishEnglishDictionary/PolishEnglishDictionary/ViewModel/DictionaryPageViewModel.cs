using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
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
        private Dictionary dic = new Dictionary();

        INavigation Navigation => Application.Current.MainPage.Navigation;
        #endregion

        public ICommand TranslateCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand RememberCommand { get; set; }
        public ICommand NoRememberCommand { get; set; }


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
        public bool IsBackwardNavAllowed { get; set; } = false;

        public DictionaryPageViewModel()
        {
            TranslateCommand = new Command(Translate);
            CheckCommand = new Command(Check);
            RememberCommand = new Command(Remember);
            NoRememberCommand = new Command(NoRemember);
            Word = dic.PolishWord;

    
            OnAppearingCommand = new Command(() => OnAppearing());
            OnDisappearingCommand = new Command(() => OnDisappearing());

        }

        
        public Command OnAppearingCommand { get; set; }

        public Command OnDisappearingCommand { get; set; }

       

        private void OnAppearing()
        {
            Shell.Current.Navigating += Current_Navigating;
        }

        private void OnDisappearing()
        {
            Shell.Current.Navigating -= Current_Navigating;
        }

        private async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            if (e.CanCancel)
            {
                e.Cancel();
                await GoBack();
            }
        }

        private async Task GoBack()
        {
            var result = await Shell.Current.DisplayAlert(
                "Going Back?",
                "Are you sure you want to go back?",
                "Yes, Please!", "Nope!");

            if (result)
            {
                Shell.Current.Navigating -= Current_Navigating;
                await Shell.Current.GoToAsync("..", true);
            }
        }
        private void Remember()
        {
            dic.IKnowing(dic.Id, true);
            ClearState();
            dic.Connect();
            Word = dic.PolishWord;
        }


        private void NoRemember()
        {
            ClearState();
            dic.Connect();
            Word = dic.PolishWord;
        }


        private void Translate()
        {

            Effect = "Tłumaczenie słowa to " + dic.EnglishWord;
            EffectColor = Color.WhiteSmoke;
        }


        
        private void Check()
        {

           if(UserTranslate.ToLower() == dic.EnglishWord.ToLower())
           {
                Effect = "Gratulacje! Poprawna odpowiedź!";
                EffectColor = Color.Green;
           }
           else
           {
                Effect = $"Nie udało się, poprawna odpowiedź to {dic.EnglishWord}";
                EffectColor = Color.Red;
           }

        }

        private void ClearState()
        {
            UserTranslate = string.Empty;
            Word = string.Empty;
            Effect = string.Empty;
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
        private string polishWord;
        SqlConnection sqlConnection;
        SqlConnection sqlConnection2;
        string answear = string.Empty;
        string world = string.Empty;
        bool iKnow = false;
        int id = 0;
        string know = string.Empty;


        #region Properties
        public int Id
        {
            get => id;
            set
            {

                id = value;
                ///Connect();
                OnPropertyChanged();
            }
        }

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
                Change(iKnow);
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

        #endregion

        public Dictionary()
        {
            Connection();
            Connect();
        }



        public void Change(bool bit)
        {
            if(bit == true)
            {
                Know = "Umiem";
            }
            else
            {
                Know = "Nie umiem"; 
            }

        }


        public void DeleteWord(int Id)
        {

            Connection();
            sqlConnection.Open();
            using (SqlCommand command2 = new SqlCommand("DELETE FROM Words (nolock) WHERE Id = @PW", sqlConnection))
            {
                command2.Parameters.Add(new SqlParameter("@PW", Id));
                command2.ExecuteNonQuery();
            }
            sqlConnection.Close();
        }




        public List<int> FindCount()
        {
            List<int> Listcount = new List<int>();
            string cnd = ConnectionString();
            int count = 0;
            int start = 0;
            sqlConnection.Open();
            using (SqlCommand command2 = new SqlCommand("SELECT TOP 1 * FROM Words (nolock) order BY Id DESC", sqlConnection))
            {
                SqlDataReader rader = command2.ExecuteReader();
                while (rader.Read())
                {
                    count = Convert.ToInt32(rader["Id"]);
                }
            }
            sqlConnection.Close();


            sqlConnection.Open();
            using (SqlCommand command2 = new SqlCommand("SELECT TOP 1 * FROM Words (nolock) order BY Id ASC", sqlConnection))
            {
                SqlDataReader rader = command2.ExecuteReader();
                while (rader.Read())
                {
                    start = Convert.ToInt32(rader["Id"]);
                }
            }
            sqlConnection.Close();

            Listcount.Add(count);
            Listcount.Add(start);



            return Listcount;
        }



        public ObservableCollection<Dictionary> ListDictionary(int count, int start)
        {
            ObservableCollection<Dictionary> ListWord = new ObservableCollection<Dictionary>();


            Connection2();
            for (int i = start; i < count; i++)
            {
                sqlConnection2.Open();
                using (SqlCommand command = new SqlCommand("SELECT  * FROM Words (nolock) where Id = @PW", sqlConnection2))
                {
                    command.Parameters.Add(new SqlParameter("@PW", i));
                    SqlDataReader radera = command.ExecuteReader();
                    while (radera.Read())
                    {
                        //Id = (int)(radera["Id"]);
                        PolishWord = (string)radera["PolishWorld"];
                        EnglishWord = (string)radera["EnglishWorld"];
                        IKnow = (bool)radera["IKnow"];
                    }

                    if (radera.HasRows == true)
                    {
                        Dictionary dic = new Dictionary();
                        dic.Id = i;
                        dic.PolishWord = PolishWord;
                        dic.EnglishWord = EnglishWord;
                        dic.IKnow = IKnow;

                        ListWord.Add(dic);
                    }
               

                    
                }
                sqlConnection2.Close();
            }

            

            return ListWord;

        }
    

               
 
        public void IKnowing(int id, bool know)
        {
            Connection();
            sqlConnection.Open();
            using (SqlCommand command2 = new SqlCommand("Update Words (nolock) set IKnow = @IKnow where Id = @PW", sqlConnection))
            {
                command2.Parameters.Add(new SqlParameter("@PW", id));
                command2.Parameters.Add(new SqlParameter("@IKnow", know));
                command2.ExecuteNonQuery();
            }         
            sqlConnection.Close();
        }

        public void Connect()
        {
            id++;
            sqlConnection.Open();
            string query = "SELECT  * FROM Words (nolock) where Id = @PW";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.Add(new SqlParameter("@PW", id));

            SqlDataReader rader = command.ExecuteReader();
            while (rader.Read())
            {
                PolishWord = (string)rader["PolishWorld"];
                EnglishWord = (string)rader["EnglishWorld"];
                IKnow = (bool)rader["IKnow"];
            }
            sqlConnection.Close();
        }

        private void Connection()
        {
            string srvrbdname = "DictionaryDatabase";
            string srvrname = "172.20.10.5";
            string srvarusername = "Paulina";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrbdname};User ID={srvarusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private void Connection2()
        {
            string srvrbdname = "DictionaryDatabase";
            string srvrname = "172.20.10.5";
            string srvarusername = "Paulina";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrbdname};User ID={srvarusername};Password={srvrpassword}";
            sqlConnection2 = new SqlConnection(sqlconn);
        }


        private string ConnectionString()
        {
            string srvrbdname = "DictionaryDatabase";
            string srvrname = "172.20.10.5";
            string srvarusername = "Paulina";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrbdname};User ID={srvarusername};Password={srvrpassword}";
            return sqlconn;
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
