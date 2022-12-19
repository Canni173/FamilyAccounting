using FamilyAccounting.Data;
using FamilyAccounting.Models;
using FamilyAccounting.Models.Interfaces;
using FamilyAccounting.Services;
using Proconsult.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FamilyAccounting.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        DataContextSQLite Context = new DataContextSQLite();

        public IList<Family> Familys { get; set; }
        public IList<Accounting> Accountings { get; set; }
        public IList<Categori> Categories { get; set; }
        public IList<AccountingInUser> AccountingsInUsers { get; set; }
        public IList<AccountingInCategory> AccountingsInCategories { get; set; }

        public double Balance
        {
            get { return balance; }
            set { Set(ref balance, value); }
        }
        private double balance;

        #region Commands


        public EnabledCommand RefreshFamilyDataCommand { get; }
        public EnabledCommand RefreshCategoriesDataCommand { get; }
        public EnabledCommand RefreshAccountingDataCommand { get; }
        public EnabledCommand RefreshAccountingsInUsersDataCommand { get; }
        public EnabledCommand RefreshAccountingsInCategoriesDataCommand { get; }


        public EnabledCommand AddCategoryCommand { get; }
        public EnabledCommand AddUserCommand { get; }
        public EnabledCommand AddMoneyCommand { get; }
 
        public EnabledCommand<IList> DeleteCommand { get; }


        #endregion

        #region Constructor

         
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            RefreshCategoriesDataCommand = new EnabledCommand(OnRefresCategoriesData);
            RefreshFamilyDataCommand = new EnabledCommand(OnRefreshFamilyData);
            RefreshAccountingDataCommand = new EnabledCommand(OnRefreshAccountingData);
            RefreshAccountingsInUsersDataCommand = new EnabledCommand(OnRefreshAccountingsInUsersData);
            RefreshAccountingsInCategoriesDataCommand = new EnabledCommand(OnRefreshAccountingsInCategoriesData);
    
            DeleteCommand = new EnabledCommand<IList>(OnDelete);

            AddCategoryCommand = new EnabledCommand(OnAddCategoryData);
            AddUserCommand = new EnabledCommand(OnAddUserData);
            AddMoneyCommand = new EnabledCommand(OnAddMoneyData);
            //   var Categories =  Context.Categories.ToList();


            LoadCategoriesData();
            LoadFamilyData();
            LoadAccountingData();
            LoadAccountingsInUsersData();
            LoadAccountingsInCategoriesData();
            RegisterEvents();
            GetBalance();

        }

        public void RegisterEvents()
        {
            MessageService.Register<SaveMessage<Family>>(this, OnSave);
            MessageService.Register<SaveMessage<Categori>>(this, OnSave);
            MessageService.Register<SaveMessage<Accounting>>(this, OnSave);
        }
        private string GroupConventCategoriesType(IEnumerable<Categori> categoris, ICategory usages)
        {

            var categori = categoris.FirstOrDefault(xf => xf.id == usages.Category);

            return categori?.Type;

        }

        #region Load data Methods 
        private void GetBalance()
        {


           var SuumSources = AccountingsInUsers.Sum(x => x.Win);
            var SuumUsages = AccountingsInUsers.Sum(x => x.Spent);
            Balance = SuumSources - SuumUsages;
           var BalanceDataItem = Context.Data.FirstOrDefault(x => x.Param == "Balance");
            if(BalanceDataItem != null)
            {
                BalanceDataItem.Value = Balance;
                Context.Entry<Models.Data>(BalanceDataItem).State = System.Data.Entity.EntityState.Modified;
                Context.SaveChanges();
            }
             
        }
        private void LoadCategoriesData()
        {
            Categories = Context.Categories.ToList();
        }
        private void LoadFamilyData()
        {


            Familys = Context.Family.ToList();
        }



        private void LoadAccountingsInCategoriesData()
        {
            AccountingsInCategories = new List<AccountingInCategory>();



            var _Usages = Context.Usages.ToList().GroupBy(x => x.User).ToList();
            var _Sources = Context.Sources.ToList().GroupBy(x => x.User).ToList();/*.GroupBy(x => GroupConventCategoriesName(Categories, x)).ToList();*/


            foreach (var item in _Usages)
            {

                var _UsagesCategories = item.GroupBy(x => x.Category).ToList();
                foreach (var UsagesCategori in _UsagesCategories)
                {
                    var categori = Categories.FirstOrDefault(xf => xf.id == UsagesCategori.Key);
                    if (categori is null) continue;
                    var accountingInUser = AccountingsInCategories.FirstOrDefault(x => x.id == item.Key && x.Category == categori.Name && x.CategoryType == categori.Type);
                    if (accountingInUser is null)
                    {
                        var user = Familys.FirstOrDefault(xf => xf.id == item.Key);

                        if (user is null) continue;
                     
                            accountingInUser = new AccountingInCategory
                            {
                                id = item.Key,
                                User = user.Name
                            };
                            AccountingsInCategories.Add(accountingInUser);
                     

                    }
                    accountingInUser.Events = UsagesCategori.Count();


                    accountingInUser.Category = categori.Name;
                    accountingInUser.CategoryType = categori.TypeText;

                    accountingInUser.Suum = UsagesCategori.Sum(x => x.Usage);

                }



            }

            foreach (var item in _Sources)
            {

                var _UsagesCategories = item.GroupBy(x => x.Category).ToList();
                foreach (var UsagesCategori in _UsagesCategories)
                {
                    var categori = Categories.FirstOrDefault(xf => xf.id == UsagesCategori.Key);
                    if (categori is null) continue;
                    var accountingInUser = AccountingsInCategories.FirstOrDefault(x => x.id == item.Key && x.Category == categori.Name && x.CategoryType == categori.Type);
                    if (accountingInUser is null)
                    {
                        var user = Familys.FirstOrDefault(xf => xf.id == item.Key); 
                        if (user is null) continue;
                        accountingInUser = new AccountingInCategory
                        {
                            id = item.Key,
                            User = user.Name
                        };
                        AccountingsInCategories.Add(accountingInUser);

                    }
                    accountingInUser.Events = UsagesCategori.Count();


                    accountingInUser.Category = categori.Name;
                    accountingInUser.CategoryType = categori.TypeText;
                    accountingInUser.Suum = UsagesCategori.Sum(x => x.Income);

                }



            }










        }
        private void LoadAccountingsInUsersData()
        {
            AccountingsInUsers = new List<AccountingInUser>();
            LoadCategoriesData();
            foreach (var item in Familys)
            {
                AccountingsInUsers.Add(new AccountingInUser
                {
                    User = item.Name,
                    id = item.id
                });
            }

            var Usages = Context.Usages.ToList().GroupBy(x => GroupConventCategoriesType(Categories, x)).ToList();



            var Sources = Context.Sources.ToList().GroupBy(x => GroupConventCategoriesType(Categories, x)).ToList();







            foreach (var group in Usages)
            {
                if (group.Key == "S") // Доход
                {
                    foreach (var item in group)
                    {
                        var accountingInUser = AccountingsInUsers.FindById(item.User);

                        if (accountingInUser != null)
                            accountingInUser.Win += item.Usage;


                    }

                }
                if (group.Key == "U") // Расход
                {
                    foreach (var item in group)
                    {
                        var accountingInUser = AccountingsInUsers.FindById(item.User);
                        if(accountingInUser != null)
                        accountingInUser.Spent += item.Usage;


                    }

                }


            }

            foreach (var group in Sources)
            {
                if (group.Key == "S") // Доход
                {
                    foreach (var item in group)
                    {
                        var accountingInUser = AccountingsInUsers.FindById(item.User);

                        if (accountingInUser != null)
                            accountingInUser.Win += item.Income;


                    }

                }
                if (group.Key == "U") // Расход
                {
                    foreach (var item in group)
                    {
                        var accountingInUser = AccountingsInUsers.FindById(item.User);

                        if (accountingInUser != null)
                            accountingInUser.Spent += item.Income;


                    }

                }


            }








        }


        private void LoadAccountingData()
        {
            Accountings = new List<Accounting>();

            var Usages = Context.Usages.ToList();
            var Sources = Context.Sources.ToList();
            LoadCategoriesData();


            foreach (var item in Usages)
            {
                Categori categori;
                string categoryType;
                CategoryFormater(Categories, item, out categori, out categoryType);
                if (categori != null)
                {
                    var user = Familys.FirstOrDefault(x => x.id == item.User);
                    if (user != null)
                        Accountings.Add(new Accounting
                        {
                            CategoryType = categoryType,
                            Category = categori.Name,
                            User = user.Name,
                            Date = item.Date,
                            Comment = item.Comment,
                            Suum = item.Usage

                        }); 
                }
            }
            foreach (var item in Sources)
            {
                Categori categori;
                string categoryType;
                CategoryFormater(Categories, item, out categori, out categoryType);
                if (categori != null)
                {
                    var user = Familys.FirstOrDefault(x => x.id == item.User);

                    if (user != null)
                        Accountings.Add(new Accounting
                        {
                            CategoryType = categoryType,
                            Category = categori.Name,
                            User = user.Name,
                            Date = item.Date,
                            Comment = item.Comment,
                            Suum = item.Income

                        });
                }
            }



        }
        #endregion


        private void CategoryFormater(IList<Categori> Categories, ICategory item, out Categori categori, out string categoryType)
        {
            categori = Categories.FirstOrDefault(x => x.id == item.Category);
            categoryType = categori?.TypeText;
           
        }

        private void OnSave<T>(SaveMessage<T> save)
        where T : class
        {
            DialogService.CloseDialog();
            if (typeof(Family) == typeof(T))
            {
                DialogService.ShowDialog(new WaitingViewModel());
                LoadFamilyData();
                RaisePropertyChanged(nameof(Familys));
                LoadAccountingsInUsersData();
                RaisePropertyChanged(nameof(AccountingsInUsers));

                RaisePropertyChanged(nameof(Categories));
                DialogService.CloseDialog();
            }
            if (typeof(Categori) == typeof(T))
            {
                DialogService.ShowDialog(new WaitingViewModel());
                LoadCategoriesData();
                RaisePropertyChanged(nameof(Categories));
                DialogService.CloseDialog();
            }
            if (typeof(Accounting) == typeof(T))
            {
                DialogService.ShowDialog(new WaitingViewModel());
                LoadAccountingData();
                LoadAccountingsInUsersData();
                LoadAccountingsInCategoriesData();
                RaisePropertyChanged(nameof(Accountings));
                RaisePropertyChanged(nameof(AccountingsInCategories));
                RaisePropertyChanged(nameof(AccountingsInUsers));

                GetBalance();
                DialogService.CloseDialog();
            }

        }
        private void AllRefreshData()
        {

            //DialogService.ShowDialog(new WaitingViewModel());

            //LoadAccountingData();
            //RaisePropertyChanged(nameof(Accountings));
            //LoadFamilyData();
            //RaisePropertyChanged(nameof(Familys));
            //LoadAccountingsInUsersData();
            //RaisePropertyChanged(nameof(AccountingsInUsers));
            //LoadAccountingsInUsersData();
            //RaisePropertyChanged(nameof(AccountingsInUsers));
            //LoadAccountingsInCategoriesData();
            //RaisePropertyChanged(nameof(AccountingsInCategories));
            //DialogService.CloseDialog();
        }

        private void OnRefreshAccountingData()
        {

            DialogService.ShowDialog(new WaitingViewModel());

            LoadAccountingData();
            RaisePropertyChanged(nameof(Accountings));
            DialogService.CloseDialog();
        }



        private void OnRefreshFamilyData()
        {
            DialogService.ShowDialog(new WaitingViewModel());
            LoadFamilyData();
            RaisePropertyChanged(nameof(Familys));
            DialogService.CloseDialog();
        }
        private void OnRefresCategoriesData()
        {
            DialogService.ShowDialog(new WaitingViewModel());
            LoadCategoriesData();
            RaisePropertyChanged(nameof(Categories));
            DialogService.CloseDialog();
        }

        private void OnRefreshAccountingsInUsersData()
        {
            DialogService.ShowDialog(new WaitingViewModel());
            LoadAccountingsInUsersData();
            RaisePropertyChanged(nameof(AccountingsInUsers));
            RaisePropertyChanged(nameof(Categories));
            DialogService.CloseDialog();
        }

        private void OnRefreshAccountingsInCategoriesData()
        {
            DialogService.ShowDialog(new WaitingViewModel());
            LoadAccountingsInCategoriesData();
            RaisePropertyChanged(nameof(AccountingsInCategories));
            RaisePropertyChanged(nameof(Categories));
            DialogService.CloseDialog();
        }


        private void OnAddMoneyData()
        {
            DialogService.ShowDialog(new AddMoneyyViewModel());
        }

        private void OnAddUserData()
        {
            DialogService.ShowDialog(new AddUserViewModel());
        }

        private void OnAddCategoryData()
        {
            DialogService.ShowDialog(new AddCategoryViewModel());
        }



        private void OnDelete(IList obj)
        {
            if (obj == null || obj.Count == 0)
                return;

            DialogService.ShowDialog(new WaitingViewModel());
            var categori = obj.OfType<Categori>().ToArray();
            if (categori.Length > 0)
            {
                foreach (var item in categori)
                { 
                        var msg = MessageBox.Show($"Вы дествительно хотите удалить эту - {item.Name} категорию", "Внимание!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (msg == MessageBoxResult.Yes)
                    {
                        var _categori = Context.Categories.FindById(item.id);
                        Context.Categories.Remove(_categori);
                    }
                             
                }

                Context.SaveChanges();
                LoadCategoriesData(); 
                LoadAccountingData();
                LoadAccountingsInUsersData();
                LoadAccountingsInCategoriesData();
                RaisePropertyChanged(nameof(Familys));
                RaisePropertyChanged(nameof(AccountingsInCategories));
                RaisePropertyChanged(nameof(Accountings));
                RaisePropertyChanged(nameof(Categories));

                RaisePropertyChanged(nameof(AccountingsInUsers));
            }

            var family = obj.OfType<Family>().ToArray();
            if (family.Length > 0)
            {
                foreach (var item in family)
                {
                    var msg = MessageBox.Show($"Вы дествительно хотите удалить этого - {item.Name} человека", "Внимание!!!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (msg == MessageBoxResult.Yes)
                    {
                     var _family =   Context.Family.FindById(item.id);
                        Context.Family.Remove(_family);
                    }

                }
                Context.SaveChanges();
                LoadFamilyData();
                LoadAccountingData();
                LoadAccountingsInUsersData();
                LoadAccountingsInCategoriesData();
                RaisePropertyChanged(nameof(Familys));
                RaisePropertyChanged(nameof(AccountingsInCategories));
                RaisePropertyChanged(nameof(AccountingsInUsers));
                RaisePropertyChanged(nameof(Accountings));
            }

            GetBalance();
            DialogService.CloseDialog();
        }
    

        #endregion
    }
}