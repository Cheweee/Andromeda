using Andromeda.Models.Context;
using Andromeda.Models.Logs;
using Andromeda.ViewModels.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Andromeda.Core.Logs
{
    public class LogErrorManager
    {
        public static string Path => AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Errors.json";
        public static ResultViewModel Add(Exception exception, [CallerMemberName] string method = "")
        {
            string message = exception.Message;
            LogError log = new LogError()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now.ToLocalTime(),
                Message = message,
                Method = method
            };
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.LogErrors.Add(log);
                    context.SaveChanges();
                }
            }
            catch(Exception logException)
            {
                if (!File.Exists(Path))
                {
                    File.Create(Path);
                }

                File.AppendAllText(Path, JsonConvert.SerializeObject(log));
            }

            return new ResultViewModel { Message = message, Result = Result.Error };
        }
    }
}