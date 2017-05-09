using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Piller.ViewModels;
using MvvmCross.Core.ViewModels;
using System.Windows.Input;
using Android.Util;

namespace Piller.Data
{
    [Table("MEDICATION_DOSAGE")]
    public class MedicationDosage
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Dosage { get; set; }

        public DaysOfWeek Days { get; set; }

        //lista godzin w postaci hh:mm;hh:mm...
        public string HoursEncoded { get; set; }

        //kodowanie i dekodowanie godzin. Tej wlasciwosci nie zapisujemy do bazy
        [Ignore]
        public IEnumerable<TimeSpan> DosageHours
        {
            get
            {
                if (string.IsNullOrEmpty(HoursEncoded))
                    return new TimeSpan[0];
                return HoursEncoded.Split(';').Select(enc => TimeSpan.Parse(enc));
            }

            set
            {
                if (value == null)
                    HoursEncoded = null;
                else
                    HoursEncoded = string.Join(";", value.Select(i => i.ToString(@"hh\:mm")));
            }

        }
        [Ignore]
        public MvxCommand ShowGalleryCommand
        {
            get
            {
                return new MvxCommand(ShowGallery);
            }
        }
        private void ShowGallery()
        {
            //this.ShowViewModel<PhotoGalleryViewModel>(new MedicationDosageNavigation { MedicationDosageId = this.Id.Value });
        }
    }
}
