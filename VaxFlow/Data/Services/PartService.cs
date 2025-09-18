using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class PartService
    {
        public PartService(IAppConfiguration configuration, PartRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly PartRepository repository;
        #endregion
        internal async Task<PartModel> CreateAsync(PartModel value)
        {
            throw new NotImplementedException();
        }

        internal async Task DeleteAsync(PartModel? selectedPart)
        {
            throw new NotImplementedException();
        }

        internal async Task<IEnumerable<PartModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        internal async Task UpdateAsync(PartModel? selectedPart)
        {
            throw new NotImplementedException();
        }
    }
}
