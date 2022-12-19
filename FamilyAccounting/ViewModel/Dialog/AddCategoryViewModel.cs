using FamilyAccounting.Data;
using FamilyAccounting.Models;
using FamilyAccounting.Services;

namespace FamilyAccounting.ViewModel
{
    public class AddCategoryViewModel : DialogViewModelBase
    {
        DataContextSQLite Context = new DataContextSQLite();

        public string Name
        {
            get { return name; }
            set { SetText(ref name, value); }
        }
        private string name;
        public bool Win
        {
            get { return win; }
            set { Set(ref win, value); }
        }
        private bool win;
        public bool Spent
        {
            get { return spent; }
            set { Set(ref spent, value); }
        }
        private bool spent;

        public AddCategoryViewModel() : base()
        {

        }
        public override string Title => "Добавление категории";

        protected override void OnCancel()
        {
            DialogService.CloseDialog();
        }

        protected override void OnOk()
        {
            if (Win) //Доход
            {
                Context.Categories.Add(new Categori { Name = Name, Type = "S" });
            }
            if (Spent) //Расход
            {
                Context.Categories.Add(new Categori { Name = Name, Type = "U" });
            }


            Context.SaveChanges();

            MessageService.SendSave<Categori>();
        }
    }
}
