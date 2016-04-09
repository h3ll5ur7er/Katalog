using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Katalog
{
    public partial class Objekt : ViewModel
    {
        public ObservableCollection<Departement> Departemente => MainViewModel.Instance.Departemnte;
        public ObservableCollection<Dorf> Dörfer => MainViewModel.Instance.Dörfer;
        public ObservableCollection<Sprachgruppe> Sprachgruppen => MainViewModel.Instance.Sprachgruppen;
        public ObservableCollection<Kategorie> Kategorien => MainViewModel.Instance.Kategorien;

        public string GroupTag => ObjektNummer.Split(' ')[0];
        
        public ObservableCollection<Uri> BilderSources => new ObservableCollection<Uri>(Bilder.Split(';').Select(x=>File.Exists(x)?new Uri(x, UriKind.RelativeOrAbsolute): new Uri(@"C:\temp\remoteSnapshots\Camera1\9b9e78a2-f85a-46f3-a46b-0b5d7b7061c9.bmp", UriKind.RelativeOrAbsolute)));
        //public ObservableCollection<Uri> BilderSources => new ObservableCollection<Uri>(Bilder.Split(';').Select(x=>new Uri(x, UriKind.RelativeOrAbsolute)));

        public Objekt()
        {
            Herkunft = "";
        }

        public string Serialize()
        {
            return $"{Id}|{ObjektNummer ?? ""}|{ObjektName ?? ""}|{Bilder ?? ""}|{Herkunft ?? ""}|{BeschreibungMaterial ?? ""}|{BeschreibungHerstellung ?? ""}|{Zustand ?? ""}|{Masse ?? ""}|{ErworbenBei ?? ""}|{Datierung ?? ""}|{Versicherungswert ?? ""}|{Dorf?.Id??0}|{Kategorie?.Id ?? 0}";
        }
    }
}