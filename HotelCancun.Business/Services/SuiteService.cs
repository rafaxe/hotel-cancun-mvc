using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace HotelCancun.Business.Services
{
    public class SuiteService : BaseService, ISuiteService
    {
        private readonly ISuiteRepository _suiteRepository;

        public SuiteService(ISuiteRepository suiteRepository,
                              INotifier notifier) : base(notifier)
        {
            _suiteRepository = suiteRepository;
        }

        public async Task Add(Suite suite)
        {
            if (!ExecuteValidation(new SuiteValidation(), suite)) return;

            await _suiteRepository.Add(suite);
        }

        public async Task Update(Suite suite)
        {
            if (!ExecuteValidation(new SuiteValidation(), suite)) return;

            await _suiteRepository.Update(suite);
        }

        public async Task Remove(Guid id)
        {
            await _suiteRepository.Remove(id);
        }

        public void Dispose()
        {
            _suiteRepository?.Dispose();
        }
    }
}