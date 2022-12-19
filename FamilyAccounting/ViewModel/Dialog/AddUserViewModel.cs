using FamilyAccounting.Data;
using FamilyAccounting.Models;
using FamilyAccounting.Services;
using System;

namespace FamilyAccounting.ViewModel
{
    public class AddUserViewModel : DialogViewModelBase
    {
        DataContextSQLite Context = new DataContextSQLite();
        public string Name
        {
            get { return name; }
            set { SetText(ref name, value); }
        }
        private string name;
        public DateTime Date
        {
            get { return date; }
            set { SetVal(ref date, value); }
        }
        private DateTime date;


        public AddUserViewModel() : base()
        {
            Date = DateTime.Now;
        }
        public override string Title => "Добавление человека";

        protected override void OnCancel()
        {
            DialogService.CloseDialog();
        }

        protected override void OnOk()
        {
            Context.Family.Add(new Family { Name = Name, Birthdate = Date, id = -1 });
            Context.SaveChanges();

            MessageService.SendSave<Family>();
        }
    }
}
