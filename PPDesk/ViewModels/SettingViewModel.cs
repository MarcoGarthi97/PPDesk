using CommunityToolkit.Mvvm.ComponentModel;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.ViewModels
{
    public class SettingViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly ISrvDatabaseService _databaseService;

        public SettingViewModel(ISrvDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task CreateDatabaseAsync()
        {
            await _databaseService.CreateTablesAsync();
        }
    }
}
