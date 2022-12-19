using FamilyAccounting.Data;
using FamilyAccounting.Models;
using FamilyAccounting.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyAccounting.ViewModel
{
    public class AddMoneyyViewModel : DialogViewModelBase
    {
        DataContextSQLite Context = new DataContextSQLite();
        public IList<Family> Familys { get; set; } 
        public IList<Categori> Categories { get; set; }
        public string Name
        {
            get { return name; }
            set { SetText(ref name, value); }
        }
        private string name;
        public bool Win
        {
            get { return win; }
            set 
            {
                Set(ref win, value);
                LoadCategoriesData();
                RaisePropertyChanged(nameof(Categories));

                SelectCategory = Categories.FirstOrDefault();
            }
        }
        private bool win;
        public bool Spent
        {
            get { return spent; }
            set
            {
               
                Set(ref spent, value);
                LoadCategoriesData();
                RaisePropertyChanged(nameof(Categories));

                SelectCategory = Categories.FirstOrDefault();
            }
        }
        private bool spent;
 
        public Categori SelectCategory
        {
            get { return selectCategory; }
            set { Set(ref selectCategory, value); }
        }
        private Categori selectCategory;
        public Family SelectFamily
        {
            get { return selectFamily; }
            set { Set(ref selectFamily, value); }
        }
        private Family selectFamily;

        public int Suum
        {
            get { return suum; }
            set { Set(ref suum, value); }
        }
        private int suum;
        public string Comment
        {
            get { return comment; }
            set { SetText(ref comment, value); }
        }
        private string comment;

        public DateTime Date
        {
            get { return date; }
            set { SetVal(ref date, value); }
        }
        private DateTime date;
        public AddMoneyyViewModel() : base()
        {
            Win = true;
         //   LoadCategoriesData();
            LoadFamilyData(); 
          //  SelectCategory = Categories.FirstOrDefault();
        SelectFamily = Familys?.FirstOrDefault();
            Date = DateTime.Now;
         
        }
        private void LoadCategoriesData()
        {
            if (Win) //Доход
            {
                Categories = Context.Categories.Where(x => x.Type == "S")?.ToList();

            }
            if (Spent) //Расход
            {
                Categories = Context.Categories.Where(x => x.Type == "U")?.ToList();

            }

     
        }
        private void LoadFamilyData()
        {


            Familys = Context.Family.ToList();
        }

        public override string Title => "Бабло";

        protected override void OnCancel()
        {
            DialogService.CloseDialog();
        }

        protected override void OnOk()
        {
            if (Win) //Доход
            {
                Context.Sources.Add(new  Source { Category = SelectCategory.id, Comment = Comment, Date = Date,Income = Suum, User = SelectFamily.id  });


            }
            if (Spent) //Расход
            {
                Context.Usages.Add(new  Usages { Category = SelectCategory.id, Comment = Comment, Date = Date, Usage = Suum, User = SelectFamily.id });


            }


            Context.SaveChanges();

            MessageService.SendSave<Accounting>();
        }
    }
}
