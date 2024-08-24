
namespace PetFamily.Domain.Models.Base
{
    public abstract class BaseModel
    {
        public Guid Id { get; private set; }

        public BaseModel(Guid id)
        {
            Id = id;
        }
    }
}
