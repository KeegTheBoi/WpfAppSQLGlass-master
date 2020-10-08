using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAppSQLGlass
{
    class Studente
    {
        //Entità
        public Studente(int key, string nominativo, int eta)
        {
            //Assegnazione dei parametri alle relative proprietà
            IDStudente = key;
            Anni = eta;
            Nominativo = nominativo;
        }

        #region Attributi

        //Si riferisce all'ID Studente Univoco
        public int IDStudente { get; set; }

        //Ottiene il nome dello Studente
        public string Nominativo { get; set; }

        //Ottiene l'età dello studente
        public int Anni { get; set; }
        #endregion


    }
}
