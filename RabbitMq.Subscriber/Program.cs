using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMq.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://kfsdoqic:jn0RezxxesWFhHedSXKfrxlcieC0PsC_@fish.rmq.cloudamqp.com/kfsdoqic");

          
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            // rabbit mqdan mesajlar kaç kaç alacağımızı belirtme (her subscriber için)
           
            //(0 => herhangi boyuttaki mesajı gönderebilirsin demek
            //1 => her subscribera 1 mesaj gelsin
            //global => true ; kaç subscriber varsa tek seferde  olacak sekilde yollar

            // örn (0,6,true) => tek seferde 3 bir subscribera 3 birine yollar   (2 sub varsa ve 6 mesaj varsa) 
            // örn (0,6,false) =>  her subscribera tek seferde 6 -6 yollar (2 sub varsa ve 6 mesaj varsa) 


            channel.BasicQos(0, 1,false);   // her subscribera bir mesaj gitsin


            var subscriber = new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue", false, subscriber); // hemen silmesin => false haber vermemizi beklesin


            subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500); // consolde görmek için 1.5 sn bekletme
                Console.WriteLine("Gelen mesaj : " + message);

                //BasicAck(deliveryTag,bool multiple): İlgili mesajı silmesi için : ilgili mesaj => ulaştırılan tagı rabbit mq ya gönderiyoruz
                //,rabbit mq hangi tagla  mesajı ulaştırmışsa mesajı bulup kuyruktan siler
                //multiple => true : o anda memoryde işlenmiş ama rabbitmq ya gitmemiş mesajalar varsa o bilgileri  de rabbit mqya haber verir
                channel.BasicAck(e.DeliveryTag,false); // hata alırsa mesajı  rbmqya göndermez
   
            };

            Console.ReadLine();
        }


    }
}