using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WpfAppSQLGlass
{
    /// <summary>
    /// Keegan Carlo Falcao 5F 8/10/2020
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dtaGridStud.Visibility = Visibility.Hidden;
        }
        //Lista di Studenti
        List<Studente> students = new List<Studente>();
        //contatore id che incrementa ogni volta che si aggiunge un nuovo studente
        static int id;

        private void btnCreaStud_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int eta = int.Parse(txtBoxEta.Text);
                ControlloErrore(txtBoxNome.Text, eta);
                //Creazione di un nuovo studente e aggiunta alla relativa lista              
                students.Add(new Studente(++id, txtBoxNome.Text, eta));
            }
            //Intercettazione Errore
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMostraGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtaGridStud.Visibility = Visibility.Visible;
                //Aggiorna la datagrid
                dtaGridStud.Items.Refresh();
                dtaGridStud.ItemsSource = students;
                //Impone alla prima colonna l'impossibilità di modificare i valori
                DataGridColumn IDcol = dtaGridStud.Columns[0];
                IDcol.IsReadOnly = true;
                //Determinazione delle lunghezze dele colonne
                dtaGridStud.Columns[0].Width = 75;
                dtaGridStud.Columns[1].Width = 200;
                dtaGridStud.Columns[2].Width = 40;
            }
            catch(Exception ed)
            {
                MessageBox.Show(ed.Message);
            }


        }

        private void txtBoxEta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Permette l'inserimento esclusivo di caratteri numerici
            int result;
            if (!int.TryParse(e.Text, out result))
            {
                e.Handled = true;
            }
        }

        private void btnFiltra_Click(object sender, RoutedEventArgs e)
        {
            //Aggiunta dei filtri disponibili nella combobox
            cmbRicerche.Items.Add("Ricerca Maggiorenni");
            cmbRicerche.Items.Add("Calcola Media");
            cmbRicerche.Items.Add("Ricerca Per Nome");
            cmbRicerche.Items.Add("Ricerca Per Età");
            AttivaMostraItems();
        }
        /// <summary>
        /// Mostra determinati Controlli
        /// </summary>
        private void AttivaMostraItems()
        {
            btnEseguiAzioni.Visibility = Visibility.Visible; cmbRicerche.Visibility = Visibility.Visible;

        }

        List<List<Studente>> listaDiListe = new List<List<Studente>>();

        private void btnEseguiAzioni_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listaDiListe.Add((List<Studente>)dtaGridStud.ItemsSource);
                if(listaDiListe.Count == 3)                    
                    if(students.Contains(Compatibility(listaDiListe[1], listaDiListe[2])))
                        students.Remove(Compatibility(listaDiListe[1], listaDiListe[2]));
                //Creazione di un oggetto di tipo Filtri
                //Attraverso la lista passata come argomento l'oggetto è in grado di svolgere certe funzioni
                Filtri sorts = new Filtri(students);
                //List<Studente> d = dtaGridStud.
                //In base a copsa si ha selezionato nella combobox
                switch (cmbRicerche.SelectedItem.ToString())
                {
                    //esegui il metodo relativo ai maggiorenni
                    case "Ricerca Maggiorenni":
                        dtaGridStud.ItemsSource = sorts.RicercaMaggiorenni();
                        listaDiListe.Add(sorts.RicercaMaggiorenni());
                        break;
                    //calcola la media
                    case "Calcola Media":
                        lblMedia.Content = "Media: " + (float)sorts.CalcolaMediaEta();
                        lblMedia.Visibility = Visibility.Visible;
                        break;
                    //seleziona e stampa solo le righe che contengono il determinato nome
                    case "Ricerca Per Nome":
                        dtaGridStud.ItemsSource = sorts.RicercaPerNome(txtBoxNomeFiltered.Text);
                        listaDiListe.Add(sorts.RicercaPerNome(txtBoxNomeFiltered.Text));
                        break;
                    case "Ricerca Per Età":
                        dtaGridStud.ItemsSource = sorts.RicercaPerEta(int.Parse(txtBoxEtaFIltered.Text));
                        listaDiListe.Add(sorts.RicercaPerEta(int.Parse(txtBoxEtaFIltered.Text)));
                        break;

                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCaricaStudentiFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Dati da estrapolare dal file
                int etaFile;
                string nomeFile;
                //Creazione di una lista per imagazzinare il file in una lista
                List<string> file = new List<string>();
                StreamReader sr = new StreamReader("Studenti.txt");
                while (!sr.EndOfStream)
                    file.Add(sr.ReadLine());
                //Controllo errore e aggiunta del nuovo studente della corrispondente lista
                for (int i = 0; i < file.Count; i++)
                {
                    //Struttura file:-> <Nome> <Età>
                    //assegna alla stringa separata dello spazio il valore adatto
                    etaFile = int.Parse(file[i].Split(' ')[1]);
                    nomeFile = file[i].Split(' ')[0];
                    ControlloErrore(nomeFile, etaFile);
                    students.Add(new Studente(++id, nomeFile, etaFile));
                }
                //disattiva il bottone di apertura del file e di conseguenza non lo rende utilizzabile
                btnCaricaStudentiFile.IsEnabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbRicerche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbRicerche.SelectedItem.ToString())
            {
                case "Ricerca Per Nome":
                    //Rende i controlli relativi al nome visibili
                    lblNomeRicerca.Visibility = Visibility.Visible;
                    txtBoxNomeFiltered.Visibility = Visibility.Visible;
                    //Rende i controlli relativi all'età invisibili
                    lblEtaRIcerca.Visibility = Visibility.Hidden;
                    txtBoxEtaFIltered.Visibility = Visibility.Hidden;
                    break;
                case "Ricerca Per Età":
                    //Rende i controlli relativi all'età visibili
                    lblEtaRIcerca.Visibility = Visibility.Visible;
                    txtBoxEtaFIltered.Visibility = Visibility.Visible;
                    //Rende i controlli relativi al nome invisibili
                    lblNomeRicerca.Visibility = Visibility.Hidden;
                    txtBoxNomeFiltered.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /// <summary>
        /// Controlla se ci sono errori nell'inserimento dei valori passati dai parametri
        /// </summary>
        /// <param name="nome">controllo se non sia una stringa vuota</param>
        /// <param name="anni">controlla il limite di età</param>
        private void ControlloErrore(string nome, int anni)
        {
            if (anni < 3 || anni > 70)
                throw new ArgumentOutOfRangeException("Anni", anni, "Età inserita non accettabile");
            if (nome == "")
                throw new FormatException("Inserire un nome perfavore.");
            return;
        }

        private Studente Compatibility(List<Studente> listToCompare, List<Studente> comparer)
        {
            for (int i = 0; i < listToCompare.Count; i++)
            {
                if (!comparer.Contains(listToCompare[i]))
                    return listToCompare[i];                    
            }
            return null;
        }
    }

    public class StudentValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            Studente s = (value as BindingGroup).Items[0] as Studente;
            if (s.Anni > 70 || s.Anni < 3)
            {
                return new ValidationResult(false,
                    "L'età deve essere compresa tra 3 ai 70 anni.");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

   

}
