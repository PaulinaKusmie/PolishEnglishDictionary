using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.ComponentModel;
using System.Windows.Input;
using System.Data.SqlClient;

namespace PolishEnglishDictionary.ViewModel
{
    class AddWordViewModel : INotifyPropertyChanged
    {
        #region -- Fields --

        private string polishWord = string.Empty;
        private string englishWord = string.Empty;
        private string countWorlds = string.Empty;

        SqlConnection sqlConnection;

        #endregion

        #region --Properties--
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

        public string CountWorlds
        {
            get => countWorlds;
            set
            {

                countWorlds = value;
                OnPropertyChanged();
            }
        }

 
        
        #endregion

        public ICommand AddCommand { get; set; }

        public AddWordViewModel()
        {
            AddCommand = new Command(Add);

            Connection();
            Count();
        }


        public void Connection()
        {
            string srvrbdname = "DictionaryDatabase";
            string srvrname = "172.20.10.5";
            string srvarusername = "Paulina";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrbdname};User ID={srvarusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private void Count()
        {
            int number = 0;
            sqlConnection.Open();
            string query = "SELECT count(*) as number From Words (nolock) ; ";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader rader = command.ExecuteReader();
            while (rader.Read())
            {
              number = Convert.ToInt32(rader["number"]);
            }
            sqlConnection.Close();

            CountWorlds = "Ilość słów w słowniku " + number.ToString();

        }


        private async void Add()
        {
            try
            {
                int id = 0;

                sqlConnection.Open();
                string query = "SELECT TOP 1 * FROM Words (nolock) order BY Id DESC";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader rader = command.ExecuteReader();
                while (rader.Read())
                {
                    id = Convert.ToInt32(rader["Id"]);
                }
                sqlConnection.Close();

                id++;


                if (EnglishWord == string.Empty && PolishWord == string.Empty)
                {
                    await App.Current.MainPage.DisplayAlert("Uwaga", "Wypełnij wszyskie pola!", "Ok");
                    return;
                }
                sqlConnection.Open();
                using (SqlCommand command2 = new SqlCommand("Insert into Words (nolock) VALUES(@Id, @EnglishWorld, @PolishWorld, @IKnow)", sqlConnection))
                {
                    command2.Parameters.Add(new SqlParameter("Id", id));
                    command2.Parameters.Add(new SqlParameter("EnglishWorld", EnglishWord));
                    command2.Parameters.Add(new SqlParameter("PolishWorld", PolishWord));
                    command2.Parameters.Add(new SqlParameter("IKnow", false));
                    command2.ExecuteNonQuery();
                }
                sqlConnection.Close();
                Count();
                ClearState();


            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Uwaga", ex.Message , "Ok");
                throw;
            }
           
        }

        private void ClearState()
        {
            EnglishWord = string.Empty;
            PolishWord = string.Empty;
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
