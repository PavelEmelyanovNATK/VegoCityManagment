using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoCityManagment.Shared.Domain;
using VegoCityManagment.Shared.Domain.Models;
using VegoCityManagment.Shared.Domain.VegoAPI;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class AddCategoryWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public AddCategoryWindowViewModel()
        {
            _vegoApi = new VegoAPI();
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
    }
}
