using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.ViewModels.Server
{
    public enum Result
    {
        Error,
        Ok,
        NotEnoughRights,
        NotAuthenticated
    }
    public interface IViewModel
    {
        Result Result { get; set; }
        string Message { get; set; }
    }
    public interface IKeyViewModel<TKey>
    {
        TKey Id { get; set; }
    }
    public interface INameViewModel
    {
        string Name { get; set; }
    }
    public interface IShortNameViewModel
    {
        string ShortName { get; set; }
    }
    public class ResultViewModel : IViewModel
    {
        #region Properties
        public Result Result { get; set; }
        public string Message { get; set; }
        #endregion        
    }
}
