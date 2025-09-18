using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class PatternService
    {
        public PatternService(IAppConfiguration configuration, PatternRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly PatternRepository repository;
        #endregion

        internal async Task<PatternModel> CreateAsync(PatternModel value)
        {
            throw new NotImplementedException();
        }

        internal async Task DeleteAsync(PatternModel? selectedPattern)
        {
            throw new NotImplementedException();
        }

        internal async Task<IEnumerable<PatternModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        internal async Task UpdateAsync(PatternModel? selectedPattern)
        {
            throw new NotImplementedException();
        }
    }
}
