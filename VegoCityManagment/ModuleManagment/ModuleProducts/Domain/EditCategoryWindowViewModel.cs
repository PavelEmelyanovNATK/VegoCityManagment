using MVVMBaseByNH.Domain;
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
    public class EditCategoryWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public EditCategoryWindowViewModel()
        {
            _vegoApi = new VegoAPI.Domain.VegoAPI();
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
                    var changedFields = new Dictionary<string, string>();

                    if (_oldCategory.Name != CategoryName)
                        changedFields["Name"] = CategoryName;

                    await _vegoApi.EditCategoryInfoAsync(new EditEntityWithIntIdRequest
                    {
                        EntityId = _oldCategory.Id,
                        ChangedFields = changedFields
                    });

                    CloseWindow?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

        private Command _loadCategoryCommand;
        public Command LoadCategoryCommand
            => _loadCategoryCommand ??= new Command(async o =>
            {
                try
                {
                    _oldCategory = await _vegoApi.FetchCategoryAsync(_oldCategory.Id);

                    CategoryName = _oldCategory.Name;
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
