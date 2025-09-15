using VaxFlow.Data.DTO;
using VaxFlow.Models;

namespace VaxFlow.Data.Mappings
{
    public class Map
    {
        public static DoctorDTO To(DoctorModel model)
        {
            return new()
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                NameSuffix = model.NameSuffix,
            };
        }
        public static DoctorModel To(DoctorDTO dto)
        {
            return new()
            {
                Id = dto.Id,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Patronymic = dto.Patronymic,
                NameSuffix = dto.NameSuffix,
            };
        }
    }
}
