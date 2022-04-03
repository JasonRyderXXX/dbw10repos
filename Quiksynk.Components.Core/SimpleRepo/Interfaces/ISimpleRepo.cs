using System;

namespace Quiksynk.Components.Core.SimpleRepo.Interfaces
{
    public interface ISimpleRepo<TProvider,TModel,TIdentity>
    {
        TModel Get(TIdentity identity);
        TModel[] List();

        TModel Create();
        void Delete(TIdentity identity);
        void Update(TModel model);

    }
}
