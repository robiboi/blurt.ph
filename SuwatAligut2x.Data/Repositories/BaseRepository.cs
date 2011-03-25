using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SuwatAligut2x.Data.Repositories
{
    public class BaseRepository
    {
        public SuwatAligut2x_Data_EntitiesDataContext context = new
            SuwatAligut2x_Data_EntitiesDataContext(ConfigurationManager
            .ConnectionStrings["SuwatAligut2x.Data.Properties.Settings.SuwatAligut2xConnectionString"].ConnectionString);

        public void SaveChanges()
        {
            try
            {
                context.SubmitChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
