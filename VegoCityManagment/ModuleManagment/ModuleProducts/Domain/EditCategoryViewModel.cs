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
    public class EditCategoryViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public EditCategoryViewModel()
        {
            _vegoApi = new VegoAPI();
        }

        private string _categoryName;

        private CategoryResponse _oldCategory;

        public string CategoryName { get => _categoryName; set { _categoryName = value; PropertyWasChanged(); } }

        public Action CloseWindow { get; set; }

        public void SetCategory(int categoryId)
            => _oldCategory = new CategoryResponse { Id = categoryId };

        private Command _saveCategoryCommand;
        public Command SaveCategoryCommand
            => _saveCategoryCommand ??= new Command(async o =>
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

        private Command _loadCategoryCommand;
        public Command loadCategoryCommand
            => _loadCategoryCommand ??= new Command(async o =>
            {
                try
                {
                    //_oldCategory = 

                    CloseWindow?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
    }
}
