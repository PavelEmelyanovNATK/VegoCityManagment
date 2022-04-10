using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class AddCategoryWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public AddCategoryWindowViewModel()
        {
            _vegoApi = new VegoAPI.Domain.VegoAPI();
        }

        private string _categoryName;

        public string CategoryName { get => _categoryName; set { _categoryName = value; PropertyWasChanged(); } }

        public Action CloseWindow { get; set; }

        private Command _addCategoryCommand;
        public Command AddCategoryCommand
            => _addCategoryCommand ??= new Command(async o =>
            {
                try
                {
                    var category = new AddCategoryRequest
                    {
                        Name = _categoryName,
                    };

                    await _vegoApi.AddCategoryAsync(category);

                    CloseWindow?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            });

        private Command _closeCommand;
        public Command CloseCommand
            => _closeCommand ??= new Command(o =>
            {
                CloseWindow?.Invoke();
            });
    }
}
