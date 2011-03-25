using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SuwatAligut2x.Data.Repositories
{
    public class MensaheRepository : BaseRepository
    {
        /// <summary>
        /// Gi kuha tanang Mensahe.
        /// </summary>
        /// <returns>Lista sa Mensahe</returns>
        public List<Mensahe> GetAllMessage()
        {
            try
            {
                var allmessge = (from m in context.Mensahes
                                 select m).ToList();

                return allmessge;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gi kuha tanang Mensahe sa TagIya
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns>Lista sa Mensahe sa TagIya</returns>
        public List<Mensahe> GetUserMessages(int UserId)
        {
            try
            {
                var messages = (from m in context.Mensahes
                                join tm in context.MensaheSaTagIyas on m.MessageId equals tm.MessageId
                                where tm.UserId == UserId
                                select m).ToList();

                return messages;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gi kuha ang Mensahe gikan sa ilhanan.
        /// </summary>
        /// <param name="MessageId">MessageId</param>
        /// <returns>Ang Mensahe</returns>
        public Mensahe GetMessage(int MessageId)
        {
            try
            {
                var message = (from m in context.Mensahes
                               where m.MessageId == MessageId
                               select m).FirstOrDefault();

                return message;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gi kuha ang gidaghanon sa Botar sa usa ka Mensahe
        /// </summary>
        /// <param name="MessageId">MessageId</param>
        /// <returns>Gidagahanon sa Botar</returns>
        public int GetMessageVotes(int MessageId)
        {
            try
            {
                var message = (from m in context.BotarSaMensahes
                               where m.MessageId == MessageId
                               select m).Count();

                return message;
            }
            catch
            {
                throw;
            }
        }

        public void InsertMessage(Mensahe Message, TagIya User)
        {
            try
            {
                context.SPI_BuhatUgMensahe(Message.Message, User.UserId);
            }
            catch
            {
                throw;
            }
        }
    }
}
