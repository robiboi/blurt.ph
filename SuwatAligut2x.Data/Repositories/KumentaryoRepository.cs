using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SuwatAligut2x.Data.Repositories
{
    public class KumentaryoRepository : BaseRepository
    {

        public List<Kumentaryo> GetCommentsForMessage(int MessageId)
        {
            try
            {
                var comments = (from k in context.Kumentaryos
                                where k.MessageId == MessageId
                                select k).ToList();

                return comments;
            }
            catch
            {
                throw;
            }
        }

        public void InsertComment(string comment, int msgId, int userId)
        {
            try
            {
                context.SPI_BuhatUgKumentaryo(comment, msgId, userId);
            }
            catch
            {
                throw;
            }
        }
    }
}
