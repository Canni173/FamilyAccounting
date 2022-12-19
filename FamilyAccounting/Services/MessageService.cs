using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Services
{
    public static class MessageService
    {
        #region Common Methods

        public static void Unregister(object recipient) => Messenger.Default.Unregister(recipient);

        public static void Register<TMessage>(object recipient, Action<TMessage> action)
            => Messenger.Default.Register(recipient, typeof(TMessage), action);

        public static void Send<TMessage>(TMessage message) => Messenger.Default.Send(message, typeof(TMessage));

        public static void Post<TMessage>(TMessage message) => DispatcherService.BeginInvoke(Send, message);
        #endregion

        #region DataModels Messaging



        public static void SendSave<T> ( )
               where T : class

        {
            Post(new SaveMessage<T>() );
        }

      



        #endregion




    }
}
